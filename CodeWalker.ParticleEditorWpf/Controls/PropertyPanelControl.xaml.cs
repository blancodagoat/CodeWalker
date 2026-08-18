using CodeWalker.ParticleEditorWpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SDX = SharpDX;
using WF = System.Windows.Forms;

namespace CodeWalker.ParticleEditorWpf.Controls
{
    // WPF-native property editor for the selected particle object. Reflects the public read/write properties,
    // hides resource-plumbing noise (padding/VFT/pointers/lists/keyframe props), and renders a type-appropriate
    // editor per row (checkbox / combo / number / vector / colour swatch / text). Edits write straight back to the
    // model through vm.EditUnderLock so the running preview updates live and can't race the simulation thread.
    public partial class PropertyPanelControl : UserControl
    {
        private ParticleEditorViewModel vm;
        private object target;

        private static readonly Brush LabelBrush = new SolidColorBrush(Color.FromRgb(0xC8, 0xC8, 0xC8));
        private static readonly Brush FieldBg = new SolidColorBrush(Color.FromRgb(0x1E, 0x1E, 0x1E));
        private static readonly Brush FieldFg = new SolidColorBrush(Color.FromRgb(0xE0, 0xE0, 0xE0));
        private static readonly Brush FieldBorder = new SolidColorBrush(Color.FromRgb(0x3F, 0x3F, 0x46));

        // property names we never want to show - pure resource plumbing
        private static readonly string[] HideContains =
        {
            "padding", "unused", "Pointer", "VFT", "FilePosition", "BlockLength", "MemoryUsage",
            "Unknown", "ManualReference", "ManualCount", "EntriesCount", "EntriesCapacity",
            "FileOffset", "IsResource", "Analyzer", "ShortName", "NameHash", "NameLower",
        };

        public PropertyPanelControl()
        {
            InitializeComponent();
            LabelBrush.Freeze(); FieldBg.Freeze(); FieldFg.Freeze(); FieldBorder.Freeze();
        }

        public void Attach(ParticleEditorViewModel viewModel) { vm = viewModel; }

        public void SetTarget(object obj)
        {
            target = obj;
            TypeHeader.Text = obj == null ? "(nothing selected)" : PrettyTypeName(obj.GetType());
            Rebuild();
        }

        private static string PrettyTypeName(Type t)
        {
            var n = t.Name;
            return n.StartsWith("Particle") ? n.Substring("Particle".Length) : n;
        }

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e) => Rebuild();

        private void Rebuild()
        {
            RowsHost.Children.Clear();
            if (target == null) return;
            string filter = FilterBox.Text?.Trim() ?? "";

            foreach (var pi in target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!pi.CanRead || !pi.CanWrite) continue;
                if (pi.GetIndexParameters().Length > 0) continue;
                if (ShouldHide(pi)) continue;
                if (filter.Length > 0 && pi.Name.IndexOf(filter, StringComparison.OrdinalIgnoreCase) < 0) continue;

                var editor = BuildEditor(pi);
                if (editor == null) continue;
                RowsHost.Children.Add(MakeRow(pi.Name, editor));
            }

            if (RowsHost.Children.Count == 0)
                RowsHost.Children.Add(new TextBlock { Text = "  (no editable properties)", Foreground = LabelBrush, Margin = new Thickness(4, 6, 0, 0) });
        }

        private static bool ShouldHide(PropertyInfo pi)
        {
            foreach (var h in HideContains)
                if (pi.Name.IndexOf(h, StringComparison.OrdinalIgnoreCase) >= 0) return true;

            var t = pi.PropertyType;
            if (t.Name == "ParticleKeyframeProp") return true; // edited in the timeline
            if (typeof(System.Collections.IEnumerable).IsAssignableFrom(t) && t != typeof(string)) return true;
            // skip nested resource blocks / complex objects we don't have a flat editor for
            if (t.IsClass && t != typeof(string) && !IsStringR(t)) return true;
            return false;
        }

        private static bool IsStringR(Type t) => t.Name == "string_r";

        private FrameworkElement BuildEditor(PropertyInfo pi)
        {
            var t = pi.PropertyType;
            object val;
            try { val = pi.GetValue(target); } catch { return null; }

            // bool -> checkbox
            if (t == typeof(bool))
            {
                var cb = new CheckBox { IsChecked = (bool)val, VerticalAlignment = VerticalAlignment.Center, Foreground = FieldFg };
                cb.Checked += (s, e) => Commit(() => pi.SetValue(target, true));
                cb.Unchecked += (s, e) => Commit(() => pi.SetValue(target, false));
                return cb;
            }

            // enum -> combo
            if (t.IsEnum)
            {
                var combo = NewCombo();
                combo.ItemsSource = Enum.GetValues(t);
                combo.SelectedItem = val;
                combo.SelectionChanged += (s, e) => { if (combo.SelectedItem != null) Commit(() => pi.SetValue(target, combo.SelectedItem)); };
                return combo;
            }

            // numeric -> text box committing the parsed value
            if (IsNumeric(t))
            {
                var tb = NewTextBox(FormatNumber(val));
                CommitOnEnterOrBlur(tb, text =>
                {
                    if (TryParseNumber(text, t, out object parsed)) Commit(() => pi.SetValue(target, parsed));
                    tb.Text = FormatNumber(pi.GetValue(target));
                });
                return tb;
            }

            // SharpDX vectors -> inline N fields (+ colour swatch when the name looks like a colour)
            if (t == typeof(SDX.Vector2)) return VectorEditor(pi, 2);
            if (t == typeof(SDX.Vector3)) return VectorEditor(pi, 3);
            if (t == typeof(SDX.Vector4)) return VectorEditor(pi, 4);

            // string
            if (t == typeof(string))
            {
                var tb = NewTextBox((string)val ?? "");
                CommitOnEnterOrBlur(tb, text => Commit(() => pi.SetValue(target, text)));
                return tb;
            }

            // string_r (resource string with a .Value)
            if (IsStringR(t))
            {
                var valueProp = t.GetProperty("Value");
                string cur = (val != null ? valueProp?.GetValue(val) as string : null) ?? "";
                var tb = NewTextBox(cur);
                CommitOnEnterOrBlur(tb, text => Commit(() =>
                {
                    var cobj = pi.GetValue(target);
                    if (cobj == null) { cobj = Activator.CreateInstance(t); pi.SetValue(target, cobj); }
                    valueProp?.SetValue(cobj, text);
                }));
                return tb;
            }

            // structs with a hash/identifier - show read-only text so the user can see them
            if (t.IsValueType)
            {
                return new TextBlock { Text = val?.ToString() ?? "", Foreground = LabelBrush, VerticalAlignment = VerticalAlignment.Center };
            }

            return null;
        }

        private FrameworkElement VectorEditor(PropertyInfo pi, int n)
        {
            var panel = new StackPanel { Orientation = Orientation.Horizontal };
            var fields = new TextBox[n];
            string[] labels = LooksLikeColour(pi.Name) ? new[] { "R", "G", "B", "A" } : new[] { "X", "Y", "Z", "W" };

            for (int i = 0; i < n; i++)
            {
                int idx = i;
                panel.Children.Add(new TextBlock { Text = labels[i], Foreground = LabelBrush, FontSize = 10, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(i == 0 ? 0 : 4, 0, 2, 0) });
                var tb = NewTextBox(FormatNumber(GetVecComp(pi.GetValue(target), idx)));
                tb.Width = 52;
                CommitOnEnterOrBlur(tb, text =>
                {
                    if (float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out float f))
                        Commit(() => pi.SetValue(target, SetVecComp(pi.GetValue(target), idx, f)));
                    tb.Text = FormatNumber(GetVecComp(pi.GetValue(target), idx));
                });
                fields[i] = tb;
                panel.Children.Add(tb);
            }

            if (LooksLikeColour(pi.Name) && n >= 3)
            {
                var swatch = new Border { Width = 20, Height = 18, Margin = new Thickness(6, 0, 0, 0), BorderBrush = FieldBorder, BorderThickness = new Thickness(1), Cursor = System.Windows.Input.Cursors.Hand };
                UpdateSwatch(swatch, pi);
                swatch.MouseLeftButtonDown += (s, e) =>
                {
                    var v = pi.GetValue(target);
                    int R = ToByte(GetVecComp(v, 0)), G = ToByte(GetVecComp(v, 1)), B = ToByte(GetVecComp(v, 2));
                    using var dlg = new WF.ColorDialog { Color = System.Drawing.Color.FromArgb(R, G, B), FullOpen = true };
                    if (dlg.ShowDialog() == WF.DialogResult.OK)
                    {
                        Commit(() =>
                        {
                            var vv = pi.GetValue(target);
                            vv = SetVecComp(vv, 0, dlg.Color.R / 255f);
                            vv = SetVecComp(vv, 1, dlg.Color.G / 255f);
                            vv = SetVecComp(vv, 2, dlg.Color.B / 255f);
                            pi.SetValue(target, vv);
                        });
                        for (int i = 0; i < Math.Min(3, fields.Length); i++) fields[i].Text = FormatNumber(GetVecComp(pi.GetValue(target), i));
                        UpdateSwatch(swatch, pi);
                    }
                };
                panel.Children.Add(swatch);
            }
            return panel;
        }

        private void UpdateSwatch(Border swatch, PropertyInfo pi)
        {
            var v = pi.GetValue(target);
            swatch.Background = new SolidColorBrush(Color.FromRgb((byte)ToByte(GetVecComp(v, 0)), (byte)ToByte(GetVecComp(v, 1)), (byte)ToByte(GetVecComp(v, 2))));
        }

        #region row + control factories

        private Grid MakeRow(string name, FrameworkElement editor)
        {
            var g = new Grid { Margin = new Thickness(4, 1, 4, 1) };
            g.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.46, GridUnitType.Star) });
            g.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.54, GridUnitType.Star) });
            var lbl = new TextBlock { Text = name, Foreground = LabelBrush, VerticalAlignment = VerticalAlignment.Center, TextTrimming = TextTrimming.CharacterEllipsis, Margin = new Thickness(0, 0, 6, 0), ToolTip = name };
            Grid.SetColumn(lbl, 0); Grid.SetColumn(editor, 1);
            g.Children.Add(lbl); g.Children.Add(editor);
            return g;
        }

        private ComboBox NewCombo()
        {
            var c = new ComboBox { VerticalContentAlignment = VerticalAlignment.Center };
            if (TryFindResource("DarkCombo") is Style s) c.Style = s; // dark toggle area + light selection text
            // popup items: force light text on the dark popup background
            var itemStyle = new Style(typeof(ComboBoxItem));
            itemStyle.Setters.Add(new Setter(ForegroundProperty, FieldFg));
            itemStyle.Setters.Add(new Setter(BackgroundProperty, FieldBg));
            c.ItemContainerStyle = itemStyle;
            return c;
        }

        private TextBox NewTextBox(string text) => new TextBox
        {
            Text = text ?? "",
            Background = FieldBg,
            Foreground = FieldFg,
            BorderBrush = FieldBorder,
            CaretBrush = FieldFg,
            Padding = new Thickness(2, 1, 2, 1),
            VerticalContentAlignment = VerticalAlignment.Center,
        };

        private void CommitOnEnterOrBlur(TextBox tb, Action<string> commit)
        {
            tb.LostFocus += (s, e) => commit(tb.Text);
            tb.KeyDown += (s, e) => { if (e.Key == System.Windows.Input.Key.Enter) { commit(tb.Text); System.Windows.Input.Keyboard.ClearFocus(); } };
        }

        #endregion

        #region helpers

        private void Commit(Action set) => vm?.EditUnderLock(set);

        private static bool LooksLikeColour(string name) =>
            name.IndexOf("Colour", StringComparison.OrdinalIgnoreCase) >= 0 ||
            name.IndexOf("Color", StringComparison.OrdinalIgnoreCase) >= 0 ||
            name.IndexOf("Tint", StringComparison.OrdinalIgnoreCase) >= 0 ||
            name.IndexOf("RGB", StringComparison.OrdinalIgnoreCase) >= 0;

        private static int ToByte(float f) => Math.Clamp((int)Math.Round(f * 255f), 0, 255);

        private static float GetVecComp(object vec, int i)
        {
            switch (vec)
            {
                case SDX.Vector2 v2: return i == 0 ? v2.X : v2.Y;
                case SDX.Vector3 v3: return i == 0 ? v3.X : i == 1 ? v3.Y : v3.Z;
                case SDX.Vector4 v4: return i == 0 ? v4.X : i == 1 ? v4.Y : i == 2 ? v4.Z : v4.W;
            }
            return 0f;
        }

        private static object SetVecComp(object vec, int i, float f)
        {
            switch (vec)
            {
                case SDX.Vector2 v2: if (i == 0) v2.X = f; else v2.Y = f; return v2;
                case SDX.Vector3 v3: if (i == 0) v3.X = f; else if (i == 1) v3.Y = f; else v3.Z = f; return v3;
                case SDX.Vector4 v4: if (i == 0) v4.X = f; else if (i == 1) v4.Y = f; else if (i == 2) v4.Z = f; else v4.W = f; return v4;
            }
            return vec;
        }

        private static bool IsNumeric(Type t) =>
            t == typeof(byte) || t == typeof(sbyte) || t == typeof(short) || t == typeof(ushort) ||
            t == typeof(int) || t == typeof(uint) || t == typeof(long) || t == typeof(ulong) ||
            t == typeof(float) || t == typeof(double);

        private static string FormatNumber(object v)
        {
            if (v is float f) return f.ToString("0.######", CultureInfo.InvariantCulture);
            if (v is double d) return d.ToString("0.######", CultureInfo.InvariantCulture);
            return Convert.ToString(v, CultureInfo.InvariantCulture) ?? "";
        }

        private static bool TryParseNumber(string text, Type t, out object parsed)
        {
            parsed = null;
            try
            {
                if (t == typeof(float)) { if (float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var f)) { parsed = f; return true; } return false; }
                if (t == typeof(double)) { if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var d)) { parsed = d; return true; } return false; }
                // integer types - allow a decimal string by rounding
                if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var dd))
                {
                    parsed = Convert.ChangeType(Math.Round(dd), t, CultureInfo.InvariantCulture);
                    return true;
                }
            }
            catch { }
            return false;
        }

        #endregion
    }
}

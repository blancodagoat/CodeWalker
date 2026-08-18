using CodeWalker.GameFiles;
using CodeWalker.ParticleEditorWpf.ViewModels;
using CodeWalker.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using SDX = SharpDX;

namespace CodeWalker.ParticleEditorWpf.Controls
{
    // Keyframe timeline / graph editor. Given the selected object (behaviour / domain / emitter rule / effect rule)
    // it reflects out every ParticleKeyframeProp it exposes as a named property and lets you edit the curves:
    // each track's Values are (KeyframeTime.X in 0..1) -> (KeyframeValue, a Vector4 of up to 4 channels). Points are
    // draggable; double-click adds, right-click removes. A playhead reflects the running preview's normalized time.
    public partial class KeyframeEditorControl : UserControl
    {
        private sealed class Track
        {
            public string Name;
            public ParticleKeyframeProp Prop;
            public bool IsColour; // label channels R/G/B/A vs X/Y/Z/W
        }

        private ParticleEditorViewModel vm;
        private readonly List<Track> tracks = new();
        private Track activeTrack;
        private readonly bool[] channelOn = { true, true, true, true };

        // graph metrics
        const double MarginL = 40, MarginR = 10, MarginT = 24, MarginB = 18;
        const double RulerH = 16; // top scrub strip - click/drag here to move the playhead
        private double vmin, vmax;             // visible value (Y) window
        private double tViewMin = 0, tViewMax = 1; // visible time (X) window - scroll wheel zooms both axes

        // interaction
        private int dragIndex = -1;
        private int dragChannel = -1;
        private bool rangeDirty = true;   // recompute the Y-axis range only on real changes, never mid/post-drag
        private bool scrubbing;
        private enum DragAxis { Free, X, Y }
        private DragAxis dragAxis = DragAxis.Free;   // press X / Y while dragging to lock the move to that axis
        private Line playhead;
        private Polygon playheadHandle;

        private static readonly Brush[] ChannelBrush =
        {
            new SolidColorBrush(Color.FromRgb(0xE0,0x60,0x60)),
            new SolidColorBrush(Color.FromRgb(0x60,0xC0,0x60)),
            new SolidColorBrush(Color.FromRgb(0x60,0xA0,0xE0)),
            new SolidColorBrush(Color.FromRgb(0xD0,0xD0,0x60)),
        };

        public KeyframeEditorControl()
        {
            InitializeComponent();
            foreach (var b in ChannelBrush) b.Freeze();
            // let the graph take keyboard focus during a drag so X / Y axis-lock keys reach it
            GraphCanvas.Focusable = true;
            GraphCanvas.FocusVisualStyle = null;
            GraphCanvas.KeyDown += GraphCanvas_KeyDown;
            GraphCanvas.MouseWheel += GraphCanvas_MouseWheel;
            HintLabel.Text = DefaultHint;
        }

        // scroll to rescale the value (Y) axis, centered on the cursor. The time (X) axis stays locked to the
        // full 0..1 timeline - scrolling adjusts the amplitude scale rather than panning into empty space.
        private void GraphCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (activeTrack == null) return;
            var p = e.GetPosition(GraphCanvas);
            double factor = e.Delta > 0 ? 1.0 / 1.15 : 1.15; // wheel up = zoom in (smaller value window = bigger scale)

            double vc = YToVal(p.Y);
            double nvMin = vc - (vc - vmin) * factor;
            double nvMax = vc + (vmax - vc) * factor;
            if ((nvMax - nvMin) > 1e-6) { vmin = nvMin; vmax = nvMax; }

            rangeDirty = false; // keep the scaled window; don't auto-refit the Y range
            Redraw();
            e.Handled = true;
        }

        private void GraphCanvas_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (dragIndex < 0) return; // axis-lock only applies while dragging a keyframe
            if (e.Key == System.Windows.Input.Key.X) { dragAxis = dragAxis == DragAxis.X ? DragAxis.Free : DragAxis.X; e.Handled = true; }
            else if (e.Key == System.Windows.Input.Key.Y) { dragAxis = dragAxis == DragAxis.Y ? DragAxis.Free : DragAxis.Y; e.Handled = true; }
            else return;
            UpdateLockHint();
        }

        private void UpdateLockHint()
        {
            HintLabel.Text = dragAxis switch
            {
                DragAxis.X => "locked to X (time) — press X to release",
                DragAxis.Y => "locked to Y (value) — press Y to release",
                _ => DefaultHint,
            };
        }

        private const string DefaultHint = "drag points · double-click to add · right-click to remove · scroll to scale · X/Y locks axis while dragging";

        public void Attach(ParticleEditorViewModel viewModel) { vm = viewModel; }

        // called when the selected tree node / object changes
        public void SetTarget(object obj)
        {
            tracks.Clear();
            if (obj != null)
            {
                foreach (var pi in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (pi.PropertyType != typeof(ParticleKeyframeProp)) continue;
                    var kp = pi.GetValue(obj) as ParticleKeyframeProp;
                    if (kp == null) continue;
                    bool col = pi.Name.IndexOf("RGB", StringComparison.OrdinalIgnoreCase) >= 0
                            || pi.Name.IndexOf("Colour", StringComparison.OrdinalIgnoreCase) >= 0
                            || pi.Name.IndexOf("Tint", StringComparison.OrdinalIgnoreCase) >= 0;
                    tracks.Add(new Track { Name = pi.Name, Prop = kp, IsColour = col });
                }
            }

            TrackList.ItemsSource = null;
            TrackList.ItemsSource = tracks.Select(t => t.Name).ToList();
            rangeDirty = true;
            if (tracks.Count > 0) TrackList.SelectedIndex = 0;
            else { activeTrack = null; BuildChannelBar(); Redraw(); }
        }

        // called on the transport timer tick to advance the playhead without a full redraw
        public void TickPlayhead()
        {
            if (playhead == null || activeTrack == null || scrubbing) return;
            PositionPlayhead(vm?.CurrentTimeRatio ?? 0f);
        }

        // move the playhead line + caret handle to a normalized time (0..1)
        private void PositionPlayhead(double t)
        {
            if (playhead == null) return;
            double x = TimeToX(t);
            playhead.X1 = playhead.X2 = x;
            if (playheadHandle != null)
            {
                playheadHandle.Points = new PointCollection
                {
                    new Point(x - 5, RulerH - 9), new Point(x + 5, RulerH - 9), new Point(x, RulerH - 1)
                };
            }
        }

        private void TrackList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = TrackList.SelectedIndex;
            activeTrack = (i >= 0 && i < tracks.Count) ? tracks[i] : null;
            // show all channels by default whenever the track changes
            channelOn[0] = channelOn[1] = channelOn[2] = channelOn[3] = true;
            tViewMin = 0; tViewMax = 1; // reset zoom to the full timeline for the new track
            rangeDirty = true;
            BuildChannelBar();
            Redraw();
        }

        private void BuildChannelBar()
        {
            ChannelBar.Children.Clear();
            if (activeTrack == null) return;
            var names = activeTrack.IsColour ? new[] { "R", "G", "B", "A" } : new[] { "X", "Y", "Z", "W" };
            for (int c = 0; c < 4; c++)
            {
                int ci = c;
                var btn = new ToggleButton_Lite(names[c], ChannelBrush[c], channelOn[c]);
                btn.Toggled += on => { channelOn[ci] = on; rangeDirty = true; Redraw(); };
                ChannelBar.Children.Add(btn);
            }
        }

        #region coordinate transforms

        private double PlotW => Math.Max(1, GraphCanvas.ActualWidth - MarginL - MarginR);
        private double PlotH => Math.Max(1, GraphCanvas.ActualHeight - MarginT - MarginB);
        private double TimeToX(double t) => MarginL + (t - tViewMin) / Math.Max(1e-6, tViewMax - tViewMin) * PlotW;
        private double ValToY(double v) => MarginT + (1 - (v - vmin) / Math.Max(1e-6, vmax - vmin)) * PlotH;
        // time values are normalized 0..1, so clamp results that drive keyframe placement / scrubbing
        private double XToTime(double x) => Math.Clamp(tViewMin + (x - MarginL) / PlotW * (tViewMax - tViewMin), 0, 1);
        private double YToVal(double y) => vmin + (1 - (y - MarginT) / PlotH) * (vmax - vmin);

        #endregion

        private static float Comp(SDX.Vector4 v, int c) => c == 0 ? v.X : c == 1 ? v.Y : c == 2 ? v.Z : v.W;
        private static SDX.Vector4 SetComp(SDX.Vector4 v, int c, float f)
        {
            if (c == 0) v.X = f; else if (c == 1) v.Y = f; else if (c == 2) v.Z = f; else v.W = f;
            return v;
        }

        private IEnumerable<int> ActiveChannels()
        {
            for (int c = 0; c < 4; c++) if (channelOn[c]) yield return c;
        }

        private void ComputeValueRange(double padFactor = 0.1)
        {
            vmin = double.MaxValue; vmax = double.MinValue;
            var vals = activeTrack?.Prop?.Values?.data_items;
            if (vals != null)
            {
                foreach (var kv in vals)
                    foreach (int c in ActiveChannels())
                    {
                        float f = Comp(kv.KeyframeValue, c);
                        if (f < vmin) vmin = f;
                        if (f > vmax) vmax = f;
                    }
            }
            if (vmin > vmax) { vmin = 0; vmax = 1; }
            if (vmax - vmin < 1e-4) { vmin -= 0.5; vmax += 0.5; }
            double pad = (vmax - vmin) * padFactor;
            vmin -= pad; vmax += pad;
        }

        private void GraphCanvas_SizeChanged(object sender, SizeChangedEventArgs e) => Redraw();

        public void Redraw()
        {
            GraphCanvas.Children.Clear();
            playhead = null;
            if (GraphCanvas.ActualWidth < 4 || GraphCanvas.ActualHeight < 4) return;

            // Always draw the editor chrome (grid, axes, ruler, playhead) - even with no track / no keyframes - so the
            // timeline stays visible and you can pick a track and double-click to add keyframes instead of a blank panel.
            // Recompute the value (Y) axis only when the data/selection actually changed (rangeDirty). Holding it
            // steady through a drag - and NOT re-fitting on release - keeps the point tracking the cursor with no
            // snap when you let go (re-fitting on release would shift it because the range, hence mapping, changes).
            if (rangeDirty) { ComputeValueRange(); rangeDirty = false; }
            DrawGridAndAxes();

            var vals = activeTrack?.Prop?.Values?.data_items;
            if (activeTrack == null)
            {
                AddText(MarginL + 8, MarginT + 6, "no keyframe tracks on this selection — pick a behaviour, domain or effect rule", "#FF707070");
            }
            else if (vals == null || vals.Length == 0)
            {
                AddText(MarginL + 8, MarginT + 6, "no keyframes — double-click to add one", "#FF707070");
            }
            else
            {
                // draw each enabled channel as a polyline; the lowest enabled channel is the "primary" (gets diamonds)
                foreach (int c in ActiveChannels())
                {
                    var pts = new PointCollection();
                    foreach (var kv in vals) pts.Add(new Point(TimeToX(kv.KeyframeTime.X), ValToY(Comp(kv.KeyframeValue, c))));
                    var pl = new Polyline { Points = pts, Stroke = ChannelBrush[c], StrokeThickness = 1.5 };
                    GraphCanvas.Children.Add(pl);

                    for (int i = 0; i < vals.Length; i++)
                    {
                        double x = TimeToX(vals[i].KeyframeTime.X);
                        double y = ValToY(Comp(vals[i].KeyframeValue, c));
                        GraphCanvas.Children.Add(MakeDiamond(x, y, ChannelBrush[c]));
                    }
                }
            }

            // scrub ruler strip across the top
            var ruler = new System.Windows.Shapes.Rectangle
            {
                Width = Math.Max(0, GraphCanvas.ActualWidth), Height = RulerH,
                Fill = new SolidColorBrush(Color.FromRgb(0x26, 0x26, 0x2A)),
            };
            Canvas.SetLeft(ruler, 0); Canvas.SetTop(ruler, 0);
            GraphCanvas.Children.Add(ruler);
            GraphCanvas.Children.Add(new Line { X1 = MarginL, X2 = MarginL + PlotW, Y1 = RulerH, Y2 = RulerH, Stroke = new SolidColorBrush(Color.FromRgb(0x3F, 0x3F, 0x46)), StrokeThickness = 1 });

            // playhead line through the graph + a caret handle in the ruler
            var phBrush = new SolidColorBrush(Color.FromArgb(0xD0, 0xFF, 0xC0, 0x40));
            playhead = new Line { Y1 = RulerH, Y2 = MarginT + PlotH, Stroke = phBrush, StrokeThickness = 1 };
            GraphCanvas.Children.Add(playhead);
            playheadHandle = new Polygon { Fill = phBrush };
            GraphCanvas.Children.Add(playheadHandle);
            PositionPlayhead(vm?.CurrentTimeRatio ?? 0f);
        }

        private void DrawGridAndAxes()
        {
            var grid = new SolidColorBrush(Color.FromRgb(0x30, 0x30, 0x30)); grid.Freeze();
            // vertical time lines, evenly across the (zoomed) view, labelled with the time at that position
            for (int i = 0; i <= 4; i++)
            {
                double frac = i / 4.0;
                double x = MarginL + frac * PlotW;
                double tval = tViewMin + frac * (tViewMax - tViewMin);
                GraphCanvas.Children.Add(new Line { X1 = x, X2 = x, Y1 = MarginT, Y2 = MarginT + PlotH, Stroke = grid, StrokeThickness = 1 });
                AddText(x - 10, MarginT + PlotH + 2, tval.ToString("0.00"), "#FF707070");
            }
            // horizontal value lines, evenly across the (zoomed) view, labelled with the value at that position
            for (int i = 0; i <= 2; i++)
            {
                double frac = i / 2.0;
                double y = MarginT + frac * PlotH;
                double vval = vmax - frac * (vmax - vmin); // top of the plot is vmax
                GraphCanvas.Children.Add(new Line { X1 = MarginL, X2 = MarginL + PlotW, Y1 = y, Y2 = y, Stroke = grid, StrokeThickness = 1 });
                AddText(2, y - 8, vval.ToString("0.###"), "#FF707070");
            }
        }

        private Polygon MakeDiamond(double x, double y, Brush b)
        {
            const double r = 4.5;
            return new Polygon
            {
                Points = new PointCollection { new Point(x, y - r), new Point(x + r, y), new Point(x, y + r), new Point(x - r, y) },
                Fill = b,
                Stroke = Brushes.Black,
                StrokeThickness = 0.5,
            };
        }

        private void AddText(double x, double y, string text, string hex)
        {
            var tb = new TextBlock { Text = text, FontSize = 10, Foreground = (Brush)new BrushConverter().ConvertFromString(hex) };
            Canvas.SetLeft(tb, x); Canvas.SetTop(tb, y);
            GraphCanvas.Children.Add(tb);
        }

        #region interaction

        // find the keyframe + channel whose diamond is nearest the point (within tolerance)
        private bool HitTest(Point p, out int index, out int channel)
        {
            index = -1; channel = -1;
            var vals = activeTrack?.Prop?.Values?.data_items;
            if (vals == null) return false;
            double best = 9 * 9;
            for (int i = 0; i < vals.Length; i++)
            {
                double x = TimeToX(vals[i].KeyframeTime.X);
                foreach (int c in ActiveChannels())
                {
                    double y = ValToY(Comp(vals[i].KeyframeValue, c));
                    double d = (p.X - x) * (p.X - x) + (p.Y - y) * (p.Y - y);
                    if (d < best) { best = d; index = i; channel = c; }
                }
            }
            return index >= 0;
        }

        private void GraphCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var p = e.GetPosition(GraphCanvas);

            // click/drag in the top ruler strip scrubs the playback
            if (p.Y <= MarginT)
            {
                scrubbing = true;
                ScrubTo(p.X);
                GraphCanvas.CaptureMouse();
                return;
            }

            if (activeTrack == null) return;
            if (e.ClickCount == 2) { AddKeyframeAt(p); return; }

            if (HitTest(p, out int idx, out int ch))
            {
                dragIndex = idx; dragChannel = ch;
                dragAxis = DragAxis.Free;
                GraphCanvas.Focus(); // so X / Y axis-lock keys reach the canvas during the drag
                GraphCanvas.CaptureMouse();
            }
        }

        private void ScrubTo(double x)
        {
            double t = XToTime(x);
            PositionPlayhead(t);        // immediate visual feedback
            vm?.SeekToRatio((float)t);  // reset + re-simulate to that time on the render thread
        }

        private void GraphCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (scrubbing) { ScrubTo(e.GetPosition(GraphCanvas).X); return; }
            if (dragIndex < 0 || activeTrack == null) return;
            var vals = activeTrack.Prop?.Values?.data_items;
            if (vals == null || dragIndex >= vals.Length) return;

            var p = e.GetPosition(GraphCanvas);
            float t = (float)XToTime(p.X);
            float v = (float)YToVal(p.Y);
            int idx = dragIndex, ch = dragChannel;

            // axis lock: keep the constrained component at the keyframe's current value
            var cur = vals[idx];
            if (dragAxis == DragAxis.X) v = Comp(cur.KeyframeValue, ch); // move in time only
            else if (dragAxis == DragAxis.Y) t = cur.KeyframeTime.X;     // move in value only

            EditKeyframe(() =>
            {
                var item = vals[idx];
                var tm = item.KeyframeTime; tm.X = t; item.KeyframeTime = tm;
                item.KeyframeValue = SetComp(item.KeyframeValue, ch, v);
            });
            Redraw();
        }

        private void GraphCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (scrubbing)
            {
                scrubbing = false;
                GraphCanvas.ReleaseMouseCapture();
                return;
            }
            if (dragIndex >= 0)
            {
                SortValuesByTime();
                dragIndex = -1; dragChannel = -1;
                dragAxis = DragAxis.Free;
                HintLabel.Text = DefaultHint;
                GraphCanvas.ReleaseMouseCapture();
                Redraw(); // keep the same range (rangeDirty stays false) so the point doesn't shift on release
            }
        }

        private void GraphCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (activeTrack == null) return;
            var p = e.GetPosition(GraphCanvas);
            if (!HitTest(p, out int idx, out _)) return;
            var prop = activeTrack.Prop;
            if ((prop?.Values?.data_items?.Length ?? 0) <= 1) return; // keep at least one keyframe

            EditKeyframe(() =>
            {
                var list = prop.Values.data_items.ToList();
                if (idx >= 0 && idx < list.Count) list.RemoveAt(idx);
                prop.Values.data_items = list.ToArray();
            });
            rangeDirty = true; // a keyframe was removed - re-fit the axis
            Redraw();
            e.Handled = true;
        }

        private void AddKeyframeAt(Point p)
        {
            var prop = activeTrack.Prop;
            if (prop == null) return;
            float t = (float)XToTime(p.X);
            float v = (float)YToVal(p.Y);

            EditKeyframe(() =>
            {
                // sample the existing curve at t so any DISABLED channels keep continuity, then set every ENABLED
                // channel to the clicked value - so a new keyframe lands a point on all visible channels, not just one.
                var value = ParticleKeyframeEval.Query(prop, t, SDX.Vector4.Zero);
                foreach (int c in ActiveChannels()) value = SetComp(value, c, v);
                var nv = new ParticleKeyframePropValue { KeyframeTime = new SDX.Vector4(t, 0, 0, 0), KeyframeValue = value };
                var list = (prop.Values?.data_items ?? Array.Empty<ParticleKeyframePropValue>()).ToList();
                list.Add(nv);
                list.Sort((a, b) => a.KeyframeTime.X.CompareTo(b.KeyframeTime.X));
                if (prop.Values == null) prop.Values = new ResourceSimpleList64<ParticleKeyframePropValue>();
                prop.Values.data_items = list.ToArray();
            });
            Redraw();
        }

        private void SortValuesByTime()
        {
            var prop = activeTrack?.Prop;
            if (prop?.Values?.data_items == null) return;
            EditKeyframe(() =>
            {
                var list = prop.Values.data_items.ToList();
                list.Sort((a, b) => a.KeyframeTime.X.CompareTo(b.KeyframeTime.X));
                prop.Values.data_items = list.ToArray();
            });
        }

        // apply a keyframe change under the render lock WITHOUT rebuilding the preview - the sim reads KFP Values
        // live each frame, so the edit shows immediately and playback keeps running instead of restarting at 0.
        private void EditKeyframe(Action edit) => vm?.EditUnderLock(edit, rebuildPreview: false);

        #endregion
    }

    // tiny channel toggle button so we don't pull in extra styles
    internal class ToggleButton_Lite : Border
    {
        private bool on;
        private readonly TextBlock label;
        public event Action<bool> Toggled;

        public ToggleButton_Lite(string text, Brush accent, bool initial)
        {
            on = initial;
            Width = 26; Height = 20; Margin = new Thickness(2, 0, 2, 0); CornerRadius = new CornerRadius(3);
            BorderBrush = accent; BorderThickness = new Thickness(1);
            label = new TextBlock { Text = text, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, FontSize = 11 };
            Child = label;
            Cursor = Cursors.Hand;
            UpdateVisual(accent);
            MouseLeftButtonDown += (s, e) => { on = !on; UpdateVisual(accent); Toggled?.Invoke(on); };
        }

        private void UpdateVisual(Brush accent)
        {
            Background = on ? accent : Brushes.Transparent;
            label.Foreground = on ? Brushes.Black : new SolidColorBrush(Color.FromRgb(0xC0, 0xC0, 0xC0));
        }
    }
}

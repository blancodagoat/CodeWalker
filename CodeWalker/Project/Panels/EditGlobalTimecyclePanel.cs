using CodeWalker.GameFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace CodeWalker.Project.Panels
{
    /// <summary>
    /// Global Timecycle/Weather XML editor. Loads GTA V's global time.xml / weather.xml
    /// from the game's RPF archives via GameFileCache, exposes a tree + PropertyGrid view
    /// of attributes and element inner text, and provides a raw XML fallback editor.
    /// Save writes the edited XML to disk (project folder or user chosen location).
    /// Unlike EditYmapTimeCycleModPanel (which edits per-ymap CTimeCycleModifiers),
    /// this editor targets the game-wide time.xml / weather.xml files.
    /// </summary>
    public partial class EditGlobalTimecyclePanel : ProjectPanel
    {
        public ProjectForm ProjectForm;

        private XmlDocument CurrentDocument;
        private string CurrentRpfPath;
        private bool HasChanges;
        private bool SuspendRawSync;
        private readonly Dictionary<TreeNode, XmlElement> nodeMap = new Dictionary<TreeNode, XmlElement>();

        public EditGlobalTimecyclePanel(ProjectForm owner)
        {
            ProjectForm = owner;
            InitializeComponent();
            if (FileComboBox.Items.Count > 0)
            {
                FileComboBox.SelectedIndex = 0;
            }
            UpdateFormTitle();
        }

        private void UpdateFormTitle()
        {
            if (CurrentDocument == null || string.IsNullOrEmpty(CurrentRpfPath))
            {
                Text = "Global Timecycle / Weather";
                HeaderLabel.Text = "(no file loaded) - pick a file and click Load";
                return;
            }
            var dirty = HasChanges ? "*" : string.Empty;
            Text = "Global Timecycle / Weather" + dirty;
            var root = CurrentDocument.DocumentElement?.Name ?? "(empty)";
            HeaderLabel.Text = CurrentRpfPath + "   [root: <" + root + ">]" + dirty;
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            var path = FileComboBox.SelectedItem as string;
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show(this, "Select a file to load.", "Load", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            LoadFromRpf(path);
        }

        private void ReloadButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CurrentRpfPath))
            {
                LoadButton_Click(sender, e);
                return;
            }
            if (HasChanges)
            {
                var res = MessageBox.Show(this, "Discard unsaved changes and reload from game files?", "Reload", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res != DialogResult.Yes) return;
            }
            LoadFromRpf(CurrentRpfPath);
        }

        private void LoadFromRpf(string rpfPath)
        {
            try
            {
                var gfc = ProjectForm?.GameFileCache;
                var rpfman = gfc?.RpfMan;
                if (rpfman == null)
                {
                    MessageBox.Show(this, "GameFileCache / RpfManager is not available.", "Load", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string text = rpfman.GetFileUTF8Text(rpfPath);
                if (string.IsNullOrEmpty(text))
                {
                    MessageBox.Show(this, "File not found or empty in game RPFs:\n" + rpfPath, "Load", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var doc = new XmlDocument();
                doc.LoadXml(text);

                CurrentDocument = doc;
                CurrentRpfPath = rpfPath;
                HasChanges = false;
                PopulateTree();
                RefreshRawXml();
                UpdateFormTitle();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to load XML:\n" + ex.Message, "Load", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateTree()
        {
            XmlTreeView.BeginUpdate();
            XmlTreeView.Nodes.Clear();
            nodeMap.Clear();
            PropertyGrid.SelectedObject = null;

            if (CurrentDocument?.DocumentElement != null)
            {
                var rootNode = BuildTreeNode(CurrentDocument.DocumentElement);
                XmlTreeView.Nodes.Add(rootNode);
                rootNode.Expand();
            }
            XmlTreeView.EndUpdate();
        }

        private TreeNode BuildTreeNode(XmlElement element)
        {
            var label = GetElementLabel(element);
            var node = new TreeNode(label) { Tag = element };
            nodeMap[node] = element;

            foreach (XmlNode child in element.ChildNodes)
            {
                if (child is XmlElement childEl)
                {
                    node.Nodes.Add(BuildTreeNode(childEl));
                }
            }
            return node;
        }

        private static string GetElementLabel(XmlElement element)
        {
            var sb = new StringBuilder();
            sb.Append('<').Append(element.Name).Append('>');

            // try to show a useful hint: "name" attribute or simple inner text
            var nameAttr = element.GetAttribute("name");
            if (!string.IsNullOrEmpty(nameAttr))
            {
                sb.Append("  ").Append(nameAttr);
            }
            else
            {
                bool hasChildElements = false;
                foreach (XmlNode c in element.ChildNodes)
                {
                    if (c is XmlElement) { hasChildElements = true; break; }
                }
                if (!hasChildElements)
                {
                    var txt = element.InnerText?.Trim() ?? string.Empty;
                    if (txt.Length > 0)
                    {
                        if (txt.Length > 40) txt = txt.Substring(0, 40) + "...";
                        sb.Append("  = ").Append(txt);
                    }
                }
            }
            return sb.ToString();
        }

        private void XmlTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null && nodeMap.TryGetValue(e.Node, out var el))
            {
                PropertyGrid.SelectedObject = new XmlElementWrapper(el, this);
            }
            else
            {
                PropertyGrid.SelectedObject = null;
            }
        }

        private void PropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            //label may have changed if "name" attribute or inner text was edited
            var sel = XmlTreeView.SelectedNode;
            if (sel != null && nodeMap.TryGetValue(sel, out var el))
            {
                sel.Text = GetElementLabel(el);
            }
            MarkChanged();
            RefreshRawXml();
        }

        internal void MarkChanged()
        {
            HasChanges = true;
            UpdateFormTitle();
        }

        private void RefreshRawXml()
        {
            if (CurrentDocument == null) { RawXmlTextBox.Text = string.Empty; return; }
            SuspendRawSync = true;
            try
            {
                RawXmlTextBox.Text = FormatXml(CurrentDocument);
            }
            finally
            {
                SuspendRawSync = false;
            }
        }

        private static string FormatXml(XmlDocument doc)
        {
            var sb = new StringBuilder();
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                OmitXmlDeclaration = false,
                NewLineOnAttributes = false,
                Encoding = Encoding.UTF8,
            };
            using (var sw = new StringWriter(sb))
            using (var xw = XmlWriter.Create(sw, settings))
            {
                doc.Save(xw);
            }
            return sb.ToString();
        }

        private void MainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentDocument == null) return;

            if (MainTabControl.SelectedTab == RawTabPage)
            {
                RefreshRawXml();
            }
            else if (MainTabControl.SelectedTab == TreeTabPage)
            {
                //user may have edited raw xml; try re-parse
                TryApplyRawXml(showErrors: true);
            }
        }

        private bool TryApplyRawXml(bool showErrors)
        {
            if (SuspendRawSync || CurrentDocument == null) return true;
            var text = RawXmlTextBox.Text ?? string.Empty;
            if (string.IsNullOrWhiteSpace(text)) return true;
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(text);
                //only replace if actually different
                if (doc.OuterXml != CurrentDocument.OuterXml)
                {
                    CurrentDocument = doc;
                    HasChanges = true;
                    PopulateTree();
                    UpdateFormTitle();
                }
                return true;
            }
            catch (Exception ex)
            {
                if (showErrors)
                {
                    MessageBox.Show(this, "Raw XML could not be parsed - tree not updated:\n" + ex.Message, "XML parse", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return false;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (CurrentDocument == null)
            {
                MessageBox.Show(this, "Nothing to save - load a file first.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //make sure raw edits (if the user is on the raw tab) are applied first
            if (MainTabControl.SelectedTab == RawTabPage)
            {
                if (!TryApplyRawXml(showErrors: true)) return;
            }

            string filename = Path.GetFileName(CurrentRpfPath?.Replace('\\', '/') ?? "timecycle.xml");
            string initialDir = null;
            try
            {
                var projFile = ProjectForm?.CurrentProjectFile;
                if (projFile != null)
                {
                    var projPath = projFile.Filepath;
                    if (!string.IsNullOrEmpty(projPath))
                    {
                        initialDir = Path.GetDirectoryName(projPath);
                    }
                }
            }
            catch { /* ignore */ }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "XML files|*.xml|All files|*.*";
                sfd.FileName = filename;
                if (!string.IsNullOrEmpty(initialDir)) sfd.InitialDirectory = initialDir;
                if (sfd.ShowDialog(this) != DialogResult.OK) return;

                try
                {
                    File.WriteAllText(sfd.FileName, FormatXml(CurrentDocument), new UTF8Encoding(false));
                    HasChanges = false;
                    UpdateFormTitle();
                    MessageBox.Show(this, "Saved to:\n" + sfd.FileName, "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Failed to write file:\n" + ex.Message, "Save", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ExportXmlButton_Click(object sender, EventArgs e)
        {
            //same as Save for now - dump XML to a user-chosen file
            SaveButton_Click(sender, e);
        }
    }

    /// <summary>
    /// PropertyGrid-friendly wrapper around an XmlElement. Exposes each attribute as
    /// an editable string property, plus a synthetic "InnerText" property when the
    /// element has no child elements (leaf nodes like &lt;value&gt;0.5&lt;/value&gt;).
    /// </summary>
    internal sealed class XmlElementWrapper : ICustomTypeDescriptor
    {
        private readonly XmlElement _element;
        private readonly EditGlobalTimecyclePanel _panel;

        public XmlElementWrapper(XmlElement element, EditGlobalTimecyclePanel panel)
        {
            _element = element;
            _panel = panel;
        }

        public override string ToString() => "<" + _element.Name + ">";

        internal XmlElement Element => _element;
        internal EditGlobalTimecyclePanel Panel => _panel;

        //--- ICustomTypeDescriptor ---
        public AttributeCollection GetAttributes() => AttributeCollection.Empty;
        public string GetClassName() => _element.Name;
        public string GetComponentName() => _element.Name;
        public TypeConverter GetConverter() => null;
        public EventDescriptor GetDefaultEvent() => null;
        public PropertyDescriptor GetDefaultProperty() => null;
        public object GetEditor(Type editorBaseType) => null;
        public EventDescriptorCollection GetEvents() => EventDescriptorCollection.Empty;
        public EventDescriptorCollection GetEvents(Attribute[] attributes) => EventDescriptorCollection.Empty;
        public PropertyDescriptorCollection GetProperties() => GetProperties(null);
        public object GetPropertyOwner(PropertyDescriptor pd) => this;

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            var list = new List<PropertyDescriptor>();

            //attributes become editable string properties
            foreach (XmlAttribute a in _element.Attributes)
            {
                list.Add(new XmlAttributePropertyDescriptor(a.Name, "Attributes"));
            }

            //show InnerText for leaf nodes
            bool hasChildElements = false;
            foreach (XmlNode c in _element.ChildNodes)
            {
                if (c is XmlElement) { hasChildElements = true; break; }
            }
            if (!hasChildElements)
            {
                list.Add(new XmlInnerTextPropertyDescriptor());
            }

            return new PropertyDescriptorCollection(list.ToArray());
        }

        private sealed class XmlAttributePropertyDescriptor : PropertyDescriptor
        {
            public XmlAttributePropertyDescriptor(string name, string category)
                : base(name, new Attribute[] { new CategoryAttribute(category) })
            {
            }

            public override Type ComponentType => typeof(XmlElementWrapper);
            public override bool IsReadOnly => false;
            public override Type PropertyType => typeof(string);
            public override bool CanResetValue(object component) => false;
            public override void ResetValue(object component) { }
            public override bool ShouldSerializeValue(object component) => true;

            public override object GetValue(object component)
            {
                var w = (XmlElementWrapper)component;
                return w.Element.GetAttribute(Name) ?? string.Empty;
            }

            public override void SetValue(object component, object value)
            {
                var w = (XmlElementWrapper)component;
                var s = value?.ToString() ?? string.Empty;
                w.Element.SetAttribute(Name, s);
                w.Panel?.MarkChanged();
            }
        }

        private sealed class XmlInnerTextPropertyDescriptor : PropertyDescriptor
        {
            public XmlInnerTextPropertyDescriptor()
                : base("InnerText", new Attribute[] { new CategoryAttribute("Value") })
            {
            }

            public override Type ComponentType => typeof(XmlElementWrapper);
            public override bool IsReadOnly => false;
            public override Type PropertyType => typeof(string);
            public override bool CanResetValue(object component) => false;
            public override void ResetValue(object component) { }
            public override bool ShouldSerializeValue(object component) => true;

            public override object GetValue(object component)
            {
                var w = (XmlElementWrapper)component;
                return w.Element.InnerText ?? string.Empty;
            }

            public override void SetValue(object component, object value)
            {
                var w = (XmlElementWrapper)component;
                w.Element.InnerText = value?.ToString() ?? string.Empty;
                w.Panel?.MarkChanged();
            }
        }
    }
}

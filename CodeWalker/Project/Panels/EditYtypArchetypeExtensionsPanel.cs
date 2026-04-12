using CodeWalker.GameFiles;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CodeWalker.Project.Panels
{
    public partial class EditYtypArchetypeExtensionsPanel : ProjectPanel
    {
        public ProjectForm ProjectForm;
        public Archetype CurrentArchetype { get; set; }

        private bool populatingui = false;

        public EditYtypArchetypeExtensionsPanel(ProjectForm owner)
        {
            ProjectForm = owner;
            InitializeComponent();
        }

        public void SetArchetype(Archetype archetype)
        {
            CurrentArchetype = archetype;
            Tag = archetype;
            UpdateFormTitle();
            LoadExtensions();
        }

        private void UpdateFormTitle()
        {
            Text = (CurrentArchetype?.Name ?? "Archetype") + " Extensions";
        }

        private void LoadExtensions()
        {
            populatingui = true;
            ExtensionsListBox.Items.Clear();
            ExtensionsPropertyGrid.SelectedObject = null;

            var exts = CurrentArchetype?.Extensions;
            if (exts != null)
            {
                foreach (var ext in exts)
                {
                    ExtensionsListBox.Items.Add(ExtensionsEditor.GetDisplayName(ext));
                }
            }
            DeleteButton.Enabled = false;
            populatingui = false;
        }

        private void ArchetypeItemChanged()
        {
            if (CurrentArchetype?.Ytyp != null)
            {
                ProjectForm.SetYtypHasChanged(true);
            }
        }

        private void ExtensionsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (populatingui) return;
            var exts = CurrentArchetype?.Extensions;
            int idx = ExtensionsListBox.SelectedIndex;
            if (exts != null && idx >= 0 && idx < exts.Length)
            {
                ExtensionsPropertyGrid.SelectedObject = ExtensionsEditor.GetEditObject(exts[idx]);
                DeleteButton.Enabled = true;
            }
            else
            {
                ExtensionsPropertyGrid.SelectedObject = null;
                DeleteButton.Enabled = false;
            }
        }

        private void ExtensionsPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (CurrentArchetype == null) return;
            ArchetypeItemChanged();
            int idx = ExtensionsListBox.SelectedIndex;
            if (idx >= 0 && CurrentArchetype.Extensions != null && idx < CurrentArchetype.Extensions.Length)
            {
                var name = ExtensionsEditor.GetDisplayName(CurrentArchetype.Extensions[idx]);
                populatingui = true;
                ExtensionsListBox.Items[idx] = name;
                ExtensionsListBox.SelectedIndex = idx;
                populatingui = false;
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (CurrentArchetype == null) return;
            var menu = new ContextMenuStrip();
            foreach (var t in ExtensionsEditor.KnownExtensionTypes)
            {
                var type = t;
                var mi = new ToolStripMenuItem(ExtensionsEditor.GetDisplayName(type));
                mi.Click += (s2, e2) =>
                {
                    var ext = ExtensionsEditor.CreateExtension(type);
                    if (ext == null) return;
                    lock (ProjectForm.ProjectSyncRoot)
                    {
                        CurrentArchetype.Extensions = ExtensionsEditor.AddExtension(CurrentArchetype.Extensions, ext);
                        ArchetypeItemChanged();
                    }
                    LoadExtensions();
                    int newIdx = (CurrentArchetype.Extensions?.Length ?? 1) - 1;
                    if (newIdx >= 0 && newIdx < ExtensionsListBox.Items.Count)
                    {
                        ExtensionsListBox.SelectedIndex = newIdx;
                    }
                };
                menu.Items.Add(mi);
            }
            menu.Show(AddButton, new System.Drawing.Point(0, AddButton.Height));
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (CurrentArchetype == null) return;
            int idx = ExtensionsListBox.SelectedIndex;
            var exts = CurrentArchetype.Extensions;
            if (exts == null || idx < 0 || idx >= exts.Length) return;
            var toRemove = exts[idx];
            lock (ProjectForm.ProjectSyncRoot)
            {
                CurrentArchetype.Extensions = ExtensionsEditor.RemoveExtension(CurrentArchetype.Extensions, toRemove);
                ArchetypeItemChanged();
            }
            LoadExtensions();
        }
    }
}

using CodeWalker.GameFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CodeWalker.Project.Panels
{
    public partial class EditAudioRelPanel : ProjectPanel
    {
        public ProjectForm ProjectForm;
        public RelFile CurrentFile { get; set; }

        private readonly Dictionary<TreeNode, RelData> nodeMap = new Dictionary<TreeNode, RelData>();

        public EditAudioRelPanel(ProjectForm owner)
        {
            ProjectForm = owner;
            InitializeComponent();
        }

        public void SetFile(RelFile file)
        {
            CurrentFile = file;
            Tag = file;
            UpdateFormTitle();
            PopulateTree();
            PropertyGrid.SelectedObject = null;
        }

        private void UpdateFormTitle()
        {
            if (CurrentFile == null)
            {
                Text = "Edit Audio REL";
                HeaderLabel.Text = "(no file loaded)";
                return;
            }
            Text = CurrentFile.Name ?? "Edit Audio REL";
            var count = CurrentFile.RelDatas?.Length ?? 0;
            HeaderLabel.Text = $"{CurrentFile.Name}  -  {CurrentFile.RelType}  -  {count} item(s)";
        }

        private void PopulateTree()
        {
            RelTreeView.BeginUpdate();
            RelTreeView.Nodes.Clear();
            nodeMap.Clear();

            if (CurrentFile?.RelDatas == null || CurrentFile.RelDatas.Length == 0)
            {
                RelTreeView.EndUpdate();
                return;
            }

            //group items by runtime type so users can navigate long lists
            var groups = CurrentFile.RelDatas
                .GroupBy(d => d?.GetType().Name ?? "Unknown")
                .OrderBy(g => g.Key);

            foreach (var group in groups)
            {
                var groupNode = new TreeNode($"{group.Key}  [{group.Count()}]");
                groupNode.Tag = group.Key;

                foreach (var item in group)
                {
                    if (item == null) continue;
                    var label = GetItemLabel(item);
                    var child = new TreeNode(label);
                    child.Tag = item;
                    nodeMap[child] = item;
                    groupNode.Nodes.Add(child);
                }

                RelTreeView.Nodes.Add(groupNode);
            }

            RelTreeView.EndUpdate();
        }

        private static string GetItemLabel(RelData item)
        {
            var name = !string.IsNullOrEmpty(item.Name) ? item.Name : item.NameHash.ToString();
            return $"{name}  (type {item.TypeID})";
        }

        private void RelTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null && nodeMap.TryGetValue(e.Node, out var rd))
            {
                PropertyGrid.SelectedObject = rd;
            }
            else
            {
                PropertyGrid.SelectedObject = null;
            }
        }

        private void PropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (CurrentFile == null) return;

            //rebuild the currently selected tree node label in case name/hash changed
            var sel = RelTreeView.SelectedNode;
            if (sel != null && nodeMap.TryGetValue(sel, out var rd))
            {
                sel.Text = GetItemLabel(rd);
            }

            ProjectForm?.SetAudioFileHasChanged(true);
        }

        private void SaveChangesButton_Click(object sender, EventArgs e)
        {
            if (CurrentFile == null) return;

            //RelDatas is an array; nothing to rebuild structurally since edits are
            //in-place via PropertyGrid. Just flag the project file as changed so the
            //existing project save path will re-serialize the rel.
            ProjectForm?.SetAudioFileHasChanged(true);

            //refresh labels from current data (name/hash may have changed)
            var selected = PropertyGrid.SelectedObject as RelData;
            PopulateTree();
            if (selected != null)
            {
                foreach (var kvp in nodeMap)
                {
                    if (ReferenceEquals(kvp.Value, selected))
                    {
                        RelTreeView.SelectedNode = kvp.Key;
                        kvp.Key.EnsureVisible();
                        break;
                    }
                }
            }
            UpdateFormTitle();

            MessageBox.Show(this, "Changes committed to the in-memory REL file. Use the project 'Save' action to write to disk.", "Save changes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ExportXmlButton_Click(object sender, EventArgs e)
        {
            if (CurrentFile == null) return;

            string xml;
            try
            {
                xml = RelXml.GetXml(CurrentFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to generate REL XML:\n" + ex.Message, "Export XML", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "XML files|*.xml|All files|*.*";
                sfd.FileName = (CurrentFile.Name ?? "audio") + ".xml";
                if (sfd.ShowDialog(this) != DialogResult.OK) return;

                try
                {
                    File.WriteAllText(sfd.FileName, xml);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Failed to write XML file:\n" + ex.Message, "Export XML", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

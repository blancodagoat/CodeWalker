using CodeWalker.GameFiles;
using CodeWalker.World;
using SharpDX;
using System;
using System.Windows.Forms;

namespace CodeWalker.Project.Panels
{
    public partial class EditYmapTimeCycleModPanel : ProjectPanel
    {
        public ProjectForm ProjectForm;
        public YmapTimeCycleModifier CurrentTimeCycleModifier { get; set; }

        private bool populatingui = false;


        public EditYmapTimeCycleModPanel(ProjectForm owner)
        {
            ProjectForm = owner;
            InitializeComponent();
        }


        public void SetTimeCycleModifier(YmapTimeCycleModifier tcm)
        {
            CurrentTimeCycleModifier = tcm;
            Tag = tcm;
            LoadTimeCycleModifier();
            UpdateFormTitle();
        }

        private void UpdateFormTitle()
        {
            var name = CurrentTimeCycleModifier?.CTimeCycleModifier.name.ToString();
            Text = "Time Cycle Mod: " + (string.IsNullOrEmpty(name) ? "(none)" : name);
        }


        private void LoadTimeCycleModifier()
        {
            if (CurrentTimeCycleModifier == null)
            {
                AddToProjectButton.Enabled = false;
                DeleteButton.Enabled = false;
                NameTextBox.Text = string.Empty;
                NameHashLabel.Text = "Hash: 0";
                PositionTextBox.Text = string.Empty;
                MinExtentsTextBox.Text = string.Empty;
                MaxExtentsTextBox.Text = string.Empty;
                PercentageTextBox.Text = string.Empty;
                RangeTextBox.Text = string.Empty;
                StartHourUpDown.Value = 0;
                EndHourUpDown.Value = 0;
                ModDataPropertyGrid.SelectedObject = null;
            }
            else
            {
                populatingui = true;
                var t = CurrentTimeCycleModifier;
                var c = t.CTimeCycleModifier;

                AddToProjectButton.Enabled = !ProjectForm.YmapExistsInProject(t.Ymap);
                DeleteButton.Enabled = false;

                NameTextBox.Text = c.name.ToString();
                NameHashLabel.Text = "Hash: " + c.name.Hash.ToString();
                var center = (c.minExtents + c.maxExtents) * 0.5f;
                PositionTextBox.Text = FloatUtil.GetVector3String(center);
                MinExtentsTextBox.Text = FloatUtil.GetVector3String(c.minExtents);
                MaxExtentsTextBox.Text = FloatUtil.GetVector3String(c.maxExtents);
                PercentageTextBox.Text = FloatUtil.ToString(c.percentage);
                RangeTextBox.Text = FloatUtil.ToString(c.range);
                StartHourUpDown.Value = Math.Min(24, Math.Max(0, (int)c.startHour));
                EndHourUpDown.Value = Math.Min(24, Math.Max(0, (int)c.endHour));

                ModDataPropertyGrid.SelectedObject = t.TimeCycleModData;

                populatingui = false;

                if (ProjectForm.WorldForm != null)
                {
                    ProjectForm.WorldForm.SelectObject(CurrentTimeCycleModifier);
                }
            }
        }

        private void ProjectItemChanged()
        {
            if (CurrentTimeCycleModifier == null) return;
            if (ProjectForm == null) return;

            ProjectForm.SetProjectItem(CurrentTimeCycleModifier);
            ProjectForm.SetYmapHasChanged(true);
        }

        private void UpdateExtentsInYmap()
        {
            if (CurrentTimeCycleModifier == null) return;
            var c = CurrentTimeCycleModifier.CTimeCycleModifier;
            CurrentTimeCycleModifier.BBMin = c.minExtents;
            CurrentTimeCycleModifier.BBMax = c.maxExtents;

            // keep the underlying CTimeCycleModifiers array in sync
            var ymap = CurrentTimeCycleModifier.Ymap;
            if (ymap?.CTimeCycleModifiers != null && ymap.TimeCycleModifiers != null)
            {
                for (int i = 0; i < ymap.TimeCycleModifiers.Length; i++)
                {
                    if (ReferenceEquals(ymap.TimeCycleModifiers[i], CurrentTimeCycleModifier))
                    {
                        ymap.CTimeCycleModifiers[i] = CurrentTimeCycleModifier.CTimeCycleModifier;
                        break;
                    }
                }
            }
        }

        private void GoToButton_Click(object sender, EventArgs e)
        {
            if (CurrentTimeCycleModifier == null) return;
            if (ProjectForm?.WorldForm == null) return;
            var c = CurrentTimeCycleModifier.CTimeCycleModifier;
            var center = (c.minExtents + c.maxExtents) * 0.5f;
            var size = c.maxExtents - c.minExtents;
            ProjectForm.WorldForm.GoToPosition(center, size);
        }

        private void AddToProjectButton_Click(object sender, EventArgs e)
        {
            if (CurrentTimeCycleModifier == null) return;
            if (ProjectForm == null) return;

            var ymap = CurrentTimeCycleModifier.Ymap;
            if (ymap != null && !ProjectForm.YmapExistsInProject(ymap))
            {
                ymap.HasChanged = true;
                ProjectForm.AddYmapToProject(ymap);
                ProjectForm.SetProjectItem(CurrentTimeCycleModifier);
            }

            AddToProjectButton.Enabled = !ProjectForm.YmapExistsInProject(CurrentTimeCycleModifier.Ymap);
        }

        private void NameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (populatingui) return;
            if (CurrentTimeCycleModifier == null) return;
            var text = NameTextBox.Text ?? string.Empty;
            var lower = text.ToLowerInvariant();
            JenkIndex.Ensure(lower);
            uint hash = JenkHash.GenHash(lower);
            lock (ProjectForm.ProjectSyncRoot)
            {
                var c = CurrentTimeCycleModifier.CTimeCycleModifier;
                if (c.name.Hash != hash)
                {
                    c.name = new MetaHash(hash);
                    CurrentTimeCycleModifier.CTimeCycleModifier = c;
                    UpdateExtentsInYmap();

                    // try to refresh associated mod data
                    var gfc = ProjectForm.GameFileCache;
                    if (gfc?.TimeCycleModsDict != null)
                    {
                        if (gfc.TimeCycleModsDict.TryGetValue(hash, out var mod))
                        {
                            CurrentTimeCycleModifier.TimeCycleModData = mod;
                        }
                    }

                    ProjectItemChanged();
                    populatingui = true;
                    NameHashLabel.Text = "Hash: " + hash.ToString();
                    ModDataPropertyGrid.SelectedObject = CurrentTimeCycleModifier.TimeCycleModData;
                    populatingui = false;
                    UpdateFormTitle();
                }
            }
        }

        private void MinExtentsTextBox_TextChanged(object sender, EventArgs e)
        {
            if (populatingui) return;
            if (CurrentTimeCycleModifier == null) return;
            var v = FloatUtil.ParseVector3String(MinExtentsTextBox.Text);
            lock (ProjectForm.ProjectSyncRoot)
            {
                var c = CurrentTimeCycleModifier.CTimeCycleModifier;
                if (c.minExtents != v)
                {
                    c.minExtents = v;
                    CurrentTimeCycleModifier.CTimeCycleModifier = c;
                    UpdateExtentsInYmap();
                    ProjectItemChanged();
                    populatingui = true;
                    var center = (c.minExtents + c.maxExtents) * 0.5f;
                    PositionTextBox.Text = FloatUtil.GetVector3String(center);
                    populatingui = false;
                }
            }
        }

        private void MaxExtentsTextBox_TextChanged(object sender, EventArgs e)
        {
            if (populatingui) return;
            if (CurrentTimeCycleModifier == null) return;
            var v = FloatUtil.ParseVector3String(MaxExtentsTextBox.Text);
            lock (ProjectForm.ProjectSyncRoot)
            {
                var c = CurrentTimeCycleModifier.CTimeCycleModifier;
                if (c.maxExtents != v)
                {
                    c.maxExtents = v;
                    CurrentTimeCycleModifier.CTimeCycleModifier = c;
                    UpdateExtentsInYmap();
                    ProjectItemChanged();
                    populatingui = true;
                    var center = (c.minExtents + c.maxExtents) * 0.5f;
                    PositionTextBox.Text = FloatUtil.GetVector3String(center);
                    populatingui = false;
                }
            }
        }

        private void PercentageTextBox_TextChanged(object sender, EventArgs e)
        {
            if (populatingui) return;
            if (CurrentTimeCycleModifier == null) return;
            float v = FloatUtil.Parse(PercentageTextBox.Text);
            lock (ProjectForm.ProjectSyncRoot)
            {
                var c = CurrentTimeCycleModifier.CTimeCycleModifier;
                if (c.percentage != v)
                {
                    c.percentage = v;
                    CurrentTimeCycleModifier.CTimeCycleModifier = c;
                    UpdateExtentsInYmap();
                    ProjectItemChanged();
                }
            }
        }

        private void RangeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (populatingui) return;
            if (CurrentTimeCycleModifier == null) return;
            float v = FloatUtil.Parse(RangeTextBox.Text);
            lock (ProjectForm.ProjectSyncRoot)
            {
                var c = CurrentTimeCycleModifier.CTimeCycleModifier;
                if (c.range != v)
                {
                    c.range = v;
                    CurrentTimeCycleModifier.CTimeCycleModifier = c;
                    UpdateExtentsInYmap();
                    ProjectItemChanged();
                }
            }
        }

        private void StartHourUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (populatingui) return;
            if (CurrentTimeCycleModifier == null) return;
            uint v = (uint)StartHourUpDown.Value;
            lock (ProjectForm.ProjectSyncRoot)
            {
                var c = CurrentTimeCycleModifier.CTimeCycleModifier;
                if (c.startHour != v)
                {
                    c.startHour = v;
                    CurrentTimeCycleModifier.CTimeCycleModifier = c;
                    UpdateExtentsInYmap();
                    ProjectItemChanged();
                }
            }
        }

        private void EndHourUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (populatingui) return;
            if (CurrentTimeCycleModifier == null) return;
            uint v = (uint)EndHourUpDown.Value;
            lock (ProjectForm.ProjectSyncRoot)
            {
                var c = CurrentTimeCycleModifier.CTimeCycleModifier;
                if (c.endHour != v)
                {
                    c.endHour = v;
                    CurrentTimeCycleModifier.CTimeCycleModifier = c;
                    UpdateExtentsInYmap();
                    ProjectItemChanged();
                }
            }
        }
    }
}

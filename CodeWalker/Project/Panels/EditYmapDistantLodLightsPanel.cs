using CodeWalker.GameFiles;
using SharpDX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeWalker.Project.Panels
{
    public partial class EditYmapDistantLodLightsPanel : ProjectPanel
    {
        public ProjectForm ProjectForm;
        public YmapDistantLODLights CurrentDistantLodLights { get; set; }

        private bool populatingui = false;
        private int selectedIndex = -1;

        public EditYmapDistantLodLightsPanel(ProjectForm owner)
        {
            ProjectForm = owner;
            InitializeComponent();
        }

        public void SetDistantLodLights(YmapDistantLODLights distLodLights)
        {
            CurrentDistantLodLights = distLodLights;
            Tag = distLodLights;
            selectedIndex = -1;
            LoadDistantLodLights();
            UpdateFormTitle();
        }

        private void UpdateFormTitle()
        {
            Text = "Distant LOD Lights: " + (CurrentDistantLodLights?.Ymap?.ToString() ?? "(none)");
        }

        private void LoadDistantLodLights()
        {
            if (CurrentDistantLodLights == null)
            {
                AddToProjectButton.Enabled = false;
                LightCountLabel.Text = "0";
                NumStreetLightsUpDown.Value = 0;
                CategoryUpDown.Value = 0;
                BBMinTextBox.Text = string.Empty;
                BBMaxTextBox.Text = string.Empty;
                LightIndexUpDown.Maximum = 0;
                LightIndexUpDown.Value = 0;
                LightPositionTextBox.Text = string.Empty;
                LightColourLabel.BackColor = System.Drawing.Color.White;
                LightColourRUpDown.Value = 0;
                LightColourGUpDown.Value = 0;
                LightColourBUpDown.Value = 0;
                LightIntensityUpDown.Value = 0;
            }
            else
            {
                populatingui = true;
                var dll = CurrentDistantLodLights;
                var cdll = dll.CDistantLODLight;

                AddToProjectButton.Enabled = !ProjectForm.YmapExistsInProject(dll.Ymap);

                int count = dll.positions?.Length ?? 0;
                LightCountLabel.Text = count.ToString();
                NumStreetLightsUpDown.Value = cdll.numStreetLights;
                CategoryUpDown.Value = cdll.category;
                BBMinTextBox.Text = FloatUtil.GetVector3String(dll.BBMin);
                BBMaxTextBox.Text = FloatUtil.GetVector3String(dll.BBMax);

                LightIndexUpDown.Maximum = Math.Max(0, count - 1);
                if (selectedIndex < 0 && count > 0) selectedIndex = 0;
                if (selectedIndex >= count) selectedIndex = count - 1;
                LightIndexUpDown.Value = Math.Max(0, selectedIndex);

                LoadSelectedLight();
                populatingui = false;
            }
        }

        private void LoadSelectedLight()
        {
            var dll = CurrentDistantLodLights;
            if (dll == null || dll.positions == null || dll.colours == null) return;
            if (selectedIndex < 0 || selectedIndex >= dll.positions.Length) return;

            var pos = dll.positions[selectedIndex].ToVector3();
            var rgbi = dll.colours[selectedIndex];

            // RGBI is stored as BGRA uint
            var col = SharpDX.Color.FromBgra(rgbi);

            LightPositionTextBox.Text = FloatUtil.GetVector3String(pos);
            LightColourRUpDown.Value = col.R;
            LightColourGUpDown.Value = col.G;
            LightColourBUpDown.Value = col.B;
            LightIntensityUpDown.Value = col.A;
            LightColourLabel.BackColor = System.Drawing.Color.FromArgb(col.R, col.G, col.B);
        }

        private void ProjectItemChanged()
        {
            if (CurrentDistantLodLights == null) return;
            if (ProjectForm == null) return;

            ProjectForm.SetProjectItem(CurrentDistantLodLights);
            ProjectForm.SetYmapHasChanged(true);
        }

        private void AddToProjectButton_Click(object sender, EventArgs e)
        {
            if (CurrentDistantLodLights == null) return;
            if (ProjectForm == null) return;
            ProjectForm.SetProjectItem(CurrentDistantLodLights);
            ProjectForm.AddYmapToProject(CurrentDistantLodLights.Ymap);
        }

        private void GoToButton_Click(object sender, EventArgs e)
        {
            if (CurrentDistantLodLights == null) return;
            if (ProjectForm?.WorldForm == null) return;
            if (selectedIndex >= 0 && CurrentDistantLodLights.positions != null && selectedIndex < CurrentDistantLodLights.positions.Length)
            {
                var pos = CurrentDistantLodLights.positions[selectedIndex].ToVector3();
                ProjectForm.WorldForm.GoToPosition(pos, Vector3.One * 100.0f);
            }
        }

        private void NumStreetLightsUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (populatingui) return;
            if (CurrentDistantLodLights == null) return;
            var cdll = CurrentDistantLodLights.CDistantLODLight;
            cdll.numStreetLights = (ushort)NumStreetLightsUpDown.Value;
            CurrentDistantLodLights.CDistantLODLight = cdll;
            ProjectItemChanged();
        }

        private void CategoryUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (populatingui) return;
            if (CurrentDistantLodLights == null) return;
            var cdll = CurrentDistantLodLights.CDistantLODLight;
            cdll.category = (ushort)CategoryUpDown.Value;
            CurrentDistantLodLights.CDistantLODLight = cdll;
            ProjectItemChanged();
        }

        private void LightIndexUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (populatingui) return;
            selectedIndex = (int)LightIndexUpDown.Value;
            populatingui = true;
            LoadSelectedLight();
            populatingui = false;
        }

        private void LightPositionTextBox_TextChanged(object sender, EventArgs e)
        {
            if (populatingui) return;
            if (CurrentDistantLodLights == null) return;
            if (selectedIndex < 0 || CurrentDistantLodLights.positions == null) return;
            if (selectedIndex >= CurrentDistantLodLights.positions.Length) return;

            Vector3 v = FloatUtil.ParseVector3String(LightPositionTextBox.Text);
            lock (ProjectForm.ProjectSyncRoot)
            {
                CurrentDistantLodLights.positions[selectedIndex] = new MetaVECTOR3(v);
                CurrentDistantLodLights.CalcBB();
                populatingui = true;
                BBMinTextBox.Text = FloatUtil.GetVector3String(CurrentDistantLodLights.BBMin);
                BBMaxTextBox.Text = FloatUtil.GetVector3String(CurrentDistantLodLights.BBMax);
                populatingui = false;
                ProjectItemChanged();
            }
        }

        private void UpdateLightColour()
        {
            if (populatingui) return;
            if (CurrentDistantLodLights == null) return;
            if (selectedIndex < 0 || CurrentDistantLodLights.colours == null) return;
            if (selectedIndex >= CurrentDistantLodLights.colours.Length) return;

            var r = (byte)LightColourRUpDown.Value;
            var g = (byte)LightColourGUpDown.Value;
            var b = (byte)LightColourBUpDown.Value;
            var i = (byte)LightIntensityUpDown.Value;

            LightColourLabel.BackColor = System.Drawing.Color.FromArgb(r, g, b);

            var col = new SharpDX.Color(r, g, b, i);
            CurrentDistantLodLights.colours[selectedIndex] = (uint)col.ToBgra();
            ProjectItemChanged();
        }

        private void LightColourRUpDown_ValueChanged(object sender, EventArgs e)
        {
            UpdateLightColour();
        }

        private void LightColourGUpDown_ValueChanged(object sender, EventArgs e)
        {
            UpdateLightColour();
        }

        private void LightColourBUpDown_ValueChanged(object sender, EventArgs e)
        {
            UpdateLightColour();
        }

        private void LightIntensityUpDown_ValueChanged(object sender, EventArgs e)
        {
            UpdateLightColour();
        }

        private void LightColourLabel_Click(object sender, EventArgs e)
        {
            var colDiag = new ColorDialog { Color = LightColourLabel.BackColor };
            if (colDiag.ShowDialog(this) == DialogResult.OK)
            {
                var c = colDiag.Color;
                populatingui = true;
                LightColourRUpDown.Value = c.R;
                LightColourGUpDown.Value = c.G;
                LightColourBUpDown.Value = c.B;
                populatingui = false;
                UpdateLightColour();
            }
        }
    }
}

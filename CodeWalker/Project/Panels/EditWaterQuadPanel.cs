using CodeWalker.GameFiles;
using CodeWalker.World;
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
    public partial class EditWaterQuadPanel : ProjectPanel
    {
        public ProjectForm ProjectForm;
        public WaterQuad CurrentWaterQuad { get; set; }

        private bool populatingui = false;

        public EditWaterQuadPanel(ProjectForm owner)
        {
            ProjectForm = owner;
            InitializeComponent();
        }

        public void SetWaterQuad(WaterQuad quad)
        {
            CurrentWaterQuad = quad;
            Tag = quad;
            LoadWaterQuad();
            UpdateFormTitle();
        }

        private void UpdateFormTitle()
        {
            Text = "Water Quad: " + (CurrentWaterQuad?.xmlNodeIndex.ToString() ?? "(none)");
        }

        private void LoadWaterQuad()
        {
            if (CurrentWaterQuad == null)
            {
                IndexTextBox.Text = string.Empty;
                MinXUpDown.Value = 0;
                MaxXUpDown.Value = 0;
                MinYUpDown.Value = 0;
                MaxYUpDown.Value = 0;
                ZUpDown.Value = 0;
                TypeUpDown.Value = 0;
                IsInvisibleCheckBox.Checked = false;
                HasLimitedDepthCheckBox.Checked = false;
                NoStencilCheckBox.Checked = false;
                A1UpDown.Value = 0;
                A2UpDown.Value = 0;
                A3UpDown.Value = 0;
                A4UpDown.Value = 0;
            }
            else
            {
                populatingui = true;
                var q = CurrentWaterQuad;

                IndexTextBox.Text = q.xmlNodeIndex.ToString();
                MinXUpDown.Value = (decimal)q.minX;
                MaxXUpDown.Value = (decimal)q.maxX;
                MinYUpDown.Value = (decimal)q.minY;
                MaxYUpDown.Value = (decimal)q.maxY;
                ZUpDown.Value = (decimal)(q.z ?? 0.0f);
                TypeUpDown.Value = q.Type;
                IsInvisibleCheckBox.Checked = q.IsInvisible;
                HasLimitedDepthCheckBox.Checked = q.HasLimitedDepth;
                NoStencilCheckBox.Checked = q.NoStencil;
                A1UpDown.Value = (decimal)q.a1;
                A2UpDown.Value = (decimal)q.a2;
                A3UpDown.Value = (decimal)q.a3;
                A4UpDown.Value = (decimal)q.a4;

                populatingui = false;
            }
        }

        private void ProjectItemChanged()
        {
            if (CurrentWaterQuad == null) return;
            if (ProjectForm == null) return;

            ProjectForm.SetProjectItem(CurrentWaterQuad);
        }

        private void GoToButton_Click(object sender, EventArgs e)
        {
            if (CurrentWaterQuad == null) return;
            if (ProjectForm?.WorldForm == null) return;
            var pos = CurrentWaterQuad.BSCenter;
            ProjectForm.WorldForm.GoToPosition(pos, Vector3.One * 100.0f);
        }

        private void MinXUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (populatingui) return;
            if (CurrentWaterQuad == null) return;
            CurrentWaterQuad.minX = (float)MinXUpDown.Value;
            CurrentWaterQuad.CalcBS();
            ProjectItemChanged();
        }

        private void MaxXUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (populatingui) return;
            if (CurrentWaterQuad == null) return;
            CurrentWaterQuad.maxX = (float)MaxXUpDown.Value;
            CurrentWaterQuad.CalcBS();
            ProjectItemChanged();
        }

        private void MinYUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (populatingui) return;
            if (CurrentWaterQuad == null) return;
            CurrentWaterQuad.minY = (float)MinYUpDown.Value;
            CurrentWaterQuad.CalcBS();
            ProjectItemChanged();
        }

        private void MaxYUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (populatingui) return;
            if (CurrentWaterQuad == null) return;
            CurrentWaterQuad.maxY = (float)MaxYUpDown.Value;
            CurrentWaterQuad.CalcBS();
            ProjectItemChanged();
        }

        private void ZUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (populatingui) return;
            if (CurrentWaterQuad == null) return;
            CurrentWaterQuad.z = (float)ZUpDown.Value;
            CurrentWaterQuad.CalcBS();
            ProjectItemChanged();
        }

        private void TypeUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (populatingui) return;
            if (CurrentWaterQuad == null) return;
            CurrentWaterQuad.Type = (int)TypeUpDown.Value;
            ProjectItemChanged();
        }

        private void IsInvisibleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (populatingui) return;
            if (CurrentWaterQuad == null) return;
            CurrentWaterQuad.IsInvisible = IsInvisibleCheckBox.Checked;
            ProjectItemChanged();
        }

        private void HasLimitedDepthCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (populatingui) return;
            if (CurrentWaterQuad == null) return;
            CurrentWaterQuad.HasLimitedDepth = HasLimitedDepthCheckBox.Checked;
            ProjectItemChanged();
        }

        private void NoStencilCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (populatingui) return;
            if (CurrentWaterQuad == null) return;
            CurrentWaterQuad.NoStencil = NoStencilCheckBox.Checked;
            ProjectItemChanged();
        }

        private void A1UpDown_ValueChanged(object sender, EventArgs e)
        {
            if (populatingui) return;
            if (CurrentWaterQuad == null) return;
            CurrentWaterQuad.a1 = (float)A1UpDown.Value;
            ProjectItemChanged();
        }

        private void A2UpDown_ValueChanged(object sender, EventArgs e)
        {
            if (populatingui) return;
            if (CurrentWaterQuad == null) return;
            CurrentWaterQuad.a2 = (float)A2UpDown.Value;
            ProjectItemChanged();
        }

        private void A3UpDown_ValueChanged(object sender, EventArgs e)
        {
            if (populatingui) return;
            if (CurrentWaterQuad == null) return;
            CurrentWaterQuad.a3 = (float)A3UpDown.Value;
            ProjectItemChanged();
        }

        private void A4UpDown_ValueChanged(object sender, EventArgs e)
        {
            if (populatingui) return;
            if (CurrentWaterQuad == null) return;
            CurrentWaterQuad.a4 = (float)A4UpDown.Value;
            ProjectItemChanged();
        }
    }
}

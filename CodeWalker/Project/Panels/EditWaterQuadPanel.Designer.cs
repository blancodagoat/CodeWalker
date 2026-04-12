
namespace CodeWalker.Project.Panels
{
    partial class EditWaterQuadPanel
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.GoToButton = new System.Windows.Forms.Button();
            this.IndexLabel = new System.Windows.Forms.Label();
            this.IndexTextBox = new System.Windows.Forms.TextBox();
            this.ExtentsGroupBox = new System.Windows.Forms.GroupBox();
            this.ZUpDown = new System.Windows.Forms.NumericUpDown();
            this.ZLabel = new System.Windows.Forms.Label();
            this.MaxYUpDown = new System.Windows.Forms.NumericUpDown();
            this.MaxYLabel = new System.Windows.Forms.Label();
            this.MinYUpDown = new System.Windows.Forms.NumericUpDown();
            this.MinYLabel = new System.Windows.Forms.Label();
            this.MaxXUpDown = new System.Windows.Forms.NumericUpDown();
            this.MaxXLabel = new System.Windows.Forms.Label();
            this.MinXUpDown = new System.Windows.Forms.NumericUpDown();
            this.MinXLabel = new System.Windows.Forms.Label();
            this.PropertiesGroupBox = new System.Windows.Forms.GroupBox();
            this.NoStencilCheckBox = new System.Windows.Forms.CheckBox();
            this.HasLimitedDepthCheckBox = new System.Windows.Forms.CheckBox();
            this.IsInvisibleCheckBox = new System.Windows.Forms.CheckBox();
            this.TypeUpDown = new System.Windows.Forms.NumericUpDown();
            this.TypeLabel = new System.Windows.Forms.Label();
            this.AttributesGroupBox = new System.Windows.Forms.GroupBox();
            this.A4UpDown = new System.Windows.Forms.NumericUpDown();
            this.A4Label = new System.Windows.Forms.Label();
            this.A3UpDown = new System.Windows.Forms.NumericUpDown();
            this.A3Label = new System.Windows.Forms.Label();
            this.A2UpDown = new System.Windows.Forms.NumericUpDown();
            this.A2Label = new System.Windows.Forms.Label();
            this.A1UpDown = new System.Windows.Forms.NumericUpDown();
            this.A1Label = new System.Windows.Forms.Label();
            this.ExtentsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ZUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxYUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinYUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxXUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinXUpDown)).BeginInit();
            this.PropertiesGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TypeUpDown)).BeginInit();
            this.AttributesGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.A4UpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.A3UpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.A2UpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.A1UpDown)).BeginInit();
            this.SuspendLayout();
            //
            // GoToButton
            //
            this.GoToButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GoToButton.Location = new System.Drawing.Point(478, 12);
            this.GoToButton.Name = "GoToButton";
            this.GoToButton.Size = new System.Drawing.Size(68, 23);
            this.GoToButton.TabIndex = 0;
            this.GoToButton.Text = "Go to";
            this.GoToButton.UseVisualStyleBackColor = true;
            this.GoToButton.Click += new System.EventHandler(this.GoToButton_Click);
            //
            // IndexLabel
            //
            this.IndexLabel.AutoSize = true;
            this.IndexLabel.Location = new System.Drawing.Point(12, 17);
            this.IndexLabel.Name = "IndexLabel";
            this.IndexLabel.Size = new System.Drawing.Size(36, 13);
            this.IndexLabel.TabIndex = 1;
            this.IndexLabel.Text = "Index:";
            //
            // IndexTextBox
            //
            this.IndexTextBox.Location = new System.Drawing.Point(100, 14);
            this.IndexTextBox.Name = "IndexTextBox";
            this.IndexTextBox.ReadOnly = true;
            this.IndexTextBox.Size = new System.Drawing.Size(154, 20);
            this.IndexTextBox.TabIndex = 2;
            //
            // ExtentsGroupBox
            //
            this.ExtentsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.ExtentsGroupBox.Controls.Add(this.ZUpDown);
            this.ExtentsGroupBox.Controls.Add(this.ZLabel);
            this.ExtentsGroupBox.Controls.Add(this.MaxYUpDown);
            this.ExtentsGroupBox.Controls.Add(this.MaxYLabel);
            this.ExtentsGroupBox.Controls.Add(this.MinYUpDown);
            this.ExtentsGroupBox.Controls.Add(this.MinYLabel);
            this.ExtentsGroupBox.Controls.Add(this.MaxXUpDown);
            this.ExtentsGroupBox.Controls.Add(this.MaxXLabel);
            this.ExtentsGroupBox.Controls.Add(this.MinXUpDown);
            this.ExtentsGroupBox.Controls.Add(this.MinXLabel);
            this.ExtentsGroupBox.Location = new System.Drawing.Point(12, 46);
            this.ExtentsGroupBox.Name = "ExtentsGroupBox";
            this.ExtentsGroupBox.Size = new System.Drawing.Size(534, 155);
            this.ExtentsGroupBox.TabIndex = 3;
            this.ExtentsGroupBox.TabStop = false;
            this.ExtentsGroupBox.Text = "Extents";
            //
            // MinXLabel
            //
            this.MinXLabel.AutoSize = true;
            this.MinXLabel.Location = new System.Drawing.Point(6, 24);
            this.MinXLabel.Name = "MinXLabel";
            this.MinXLabel.Size = new System.Drawing.Size(36, 13);
            this.MinXLabel.TabIndex = 0;
            this.MinXLabel.Text = "Min X:";
            //
            // MinXUpDown
            //
            this.MinXUpDown.DecimalPlaces = 4;
            this.MinXUpDown.Location = new System.Drawing.Point(88, 22);
            this.MinXUpDown.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.MinXUpDown.Minimum = new decimal(new int[] { 1000000, 0, 0, -2147483648 });
            this.MinXUpDown.Name = "MinXUpDown";
            this.MinXUpDown.Size = new System.Drawing.Size(154, 20);
            this.MinXUpDown.TabIndex = 1;
            this.MinXUpDown.ValueChanged += new System.EventHandler(this.MinXUpDown_ValueChanged);
            //
            // MaxXLabel
            //
            this.MaxXLabel.AutoSize = true;
            this.MaxXLabel.Location = new System.Drawing.Point(260, 24);
            this.MaxXLabel.Name = "MaxXLabel";
            this.MaxXLabel.Size = new System.Drawing.Size(39, 13);
            this.MaxXLabel.TabIndex = 2;
            this.MaxXLabel.Text = "Max X:";
            //
            // MaxXUpDown
            //
            this.MaxXUpDown.DecimalPlaces = 4;
            this.MaxXUpDown.Location = new System.Drawing.Point(325, 22);
            this.MaxXUpDown.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.MaxXUpDown.Minimum = new decimal(new int[] { 1000000, 0, 0, -2147483648 });
            this.MaxXUpDown.Name = "MaxXUpDown";
            this.MaxXUpDown.Size = new System.Drawing.Size(154, 20);
            this.MaxXUpDown.TabIndex = 3;
            this.MaxXUpDown.ValueChanged += new System.EventHandler(this.MaxXUpDown_ValueChanged);
            //
            // MinYLabel
            //
            this.MinYLabel.AutoSize = true;
            this.MinYLabel.Location = new System.Drawing.Point(6, 54);
            this.MinYLabel.Name = "MinYLabel";
            this.MinYLabel.Size = new System.Drawing.Size(36, 13);
            this.MinYLabel.TabIndex = 4;
            this.MinYLabel.Text = "Min Y:";
            //
            // MinYUpDown
            //
            this.MinYUpDown.DecimalPlaces = 4;
            this.MinYUpDown.Location = new System.Drawing.Point(88, 52);
            this.MinYUpDown.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.MinYUpDown.Minimum = new decimal(new int[] { 1000000, 0, 0, -2147483648 });
            this.MinYUpDown.Name = "MinYUpDown";
            this.MinYUpDown.Size = new System.Drawing.Size(154, 20);
            this.MinYUpDown.TabIndex = 5;
            this.MinYUpDown.ValueChanged += new System.EventHandler(this.MinYUpDown_ValueChanged);
            //
            // MaxYLabel
            //
            this.MaxYLabel.AutoSize = true;
            this.MaxYLabel.Location = new System.Drawing.Point(260, 54);
            this.MaxYLabel.Name = "MaxYLabel";
            this.MaxYLabel.Size = new System.Drawing.Size(39, 13);
            this.MaxYLabel.TabIndex = 6;
            this.MaxYLabel.Text = "Max Y:";
            //
            // MaxYUpDown
            //
            this.MaxYUpDown.DecimalPlaces = 4;
            this.MaxYUpDown.Location = new System.Drawing.Point(325, 52);
            this.MaxYUpDown.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.MaxYUpDown.Minimum = new decimal(new int[] { 1000000, 0, 0, -2147483648 });
            this.MaxYUpDown.Name = "MaxYUpDown";
            this.MaxYUpDown.Size = new System.Drawing.Size(154, 20);
            this.MaxYUpDown.TabIndex = 7;
            this.MaxYUpDown.ValueChanged += new System.EventHandler(this.MaxYUpDown_ValueChanged);
            //
            // ZLabel
            //
            this.ZLabel.AutoSize = true;
            this.ZLabel.Location = new System.Drawing.Point(6, 84);
            this.ZLabel.Name = "ZLabel";
            this.ZLabel.Size = new System.Drawing.Size(17, 13);
            this.ZLabel.TabIndex = 8;
            this.ZLabel.Text = "Z:";
            //
            // ZUpDown
            //
            this.ZUpDown.DecimalPlaces = 4;
            this.ZUpDown.Location = new System.Drawing.Point(88, 82);
            this.ZUpDown.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.ZUpDown.Minimum = new decimal(new int[] { 1000000, 0, 0, -2147483648 });
            this.ZUpDown.Name = "ZUpDown";
            this.ZUpDown.Size = new System.Drawing.Size(154, 20);
            this.ZUpDown.TabIndex = 9;
            this.ZUpDown.ValueChanged += new System.EventHandler(this.ZUpDown_ValueChanged);
            //
            // PropertiesGroupBox
            //
            this.PropertiesGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.PropertiesGroupBox.Controls.Add(this.NoStencilCheckBox);
            this.PropertiesGroupBox.Controls.Add(this.HasLimitedDepthCheckBox);
            this.PropertiesGroupBox.Controls.Add(this.IsInvisibleCheckBox);
            this.PropertiesGroupBox.Controls.Add(this.TypeUpDown);
            this.PropertiesGroupBox.Controls.Add(this.TypeLabel);
            this.PropertiesGroupBox.Location = new System.Drawing.Point(12, 207);
            this.PropertiesGroupBox.Name = "PropertiesGroupBox";
            this.PropertiesGroupBox.Size = new System.Drawing.Size(534, 105);
            this.PropertiesGroupBox.TabIndex = 4;
            this.PropertiesGroupBox.TabStop = false;
            this.PropertiesGroupBox.Text = "Properties";
            //
            // TypeLabel
            //
            this.TypeLabel.AutoSize = true;
            this.TypeLabel.Location = new System.Drawing.Point(6, 24);
            this.TypeLabel.Name = "TypeLabel";
            this.TypeLabel.Size = new System.Drawing.Size(34, 13);
            this.TypeLabel.TabIndex = 0;
            this.TypeLabel.Text = "Type:";
            //
            // TypeUpDown
            //
            this.TypeUpDown.Location = new System.Drawing.Point(88, 22);
            this.TypeUpDown.Name = "TypeUpDown";
            this.TypeUpDown.Size = new System.Drawing.Size(154, 20);
            this.TypeUpDown.TabIndex = 1;
            this.TypeUpDown.ValueChanged += new System.EventHandler(this.TypeUpDown_ValueChanged);
            //
            // IsInvisibleCheckBox
            //
            this.IsInvisibleCheckBox.AutoSize = true;
            this.IsInvisibleCheckBox.Location = new System.Drawing.Point(9, 52);
            this.IsInvisibleCheckBox.Name = "IsInvisibleCheckBox";
            this.IsInvisibleCheckBox.Size = new System.Drawing.Size(72, 17);
            this.IsInvisibleCheckBox.TabIndex = 2;
            this.IsInvisibleCheckBox.Text = "Is Invisible";
            this.IsInvisibleCheckBox.UseVisualStyleBackColor = true;
            this.IsInvisibleCheckBox.CheckedChanged += new System.EventHandler(this.IsInvisibleCheckBox_CheckedChanged);
            //
            // HasLimitedDepthCheckBox
            //
            this.HasLimitedDepthCheckBox.AutoSize = true;
            this.HasLimitedDepthCheckBox.Location = new System.Drawing.Point(120, 52);
            this.HasLimitedDepthCheckBox.Name = "HasLimitedDepthCheckBox";
            this.HasLimitedDepthCheckBox.Size = new System.Drawing.Size(113, 17);
            this.HasLimitedDepthCheckBox.TabIndex = 3;
            this.HasLimitedDepthCheckBox.Text = "Has Limited Depth";
            this.HasLimitedDepthCheckBox.UseVisualStyleBackColor = true;
            this.HasLimitedDepthCheckBox.CheckedChanged += new System.EventHandler(this.HasLimitedDepthCheckBox_CheckedChanged);
            //
            // NoStencilCheckBox
            //
            this.NoStencilCheckBox.AutoSize = true;
            this.NoStencilCheckBox.Location = new System.Drawing.Point(270, 52);
            this.NoStencilCheckBox.Name = "NoStencilCheckBox";
            this.NoStencilCheckBox.Size = new System.Drawing.Size(76, 17);
            this.NoStencilCheckBox.TabIndex = 4;
            this.NoStencilCheckBox.Text = "No Stencil";
            this.NoStencilCheckBox.UseVisualStyleBackColor = true;
            this.NoStencilCheckBox.CheckedChanged += new System.EventHandler(this.NoStencilCheckBox_CheckedChanged);
            //
            // AttributesGroupBox
            //
            this.AttributesGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.AttributesGroupBox.Controls.Add(this.A4UpDown);
            this.AttributesGroupBox.Controls.Add(this.A4Label);
            this.AttributesGroupBox.Controls.Add(this.A3UpDown);
            this.AttributesGroupBox.Controls.Add(this.A3Label);
            this.AttributesGroupBox.Controls.Add(this.A2UpDown);
            this.AttributesGroupBox.Controls.Add(this.A2Label);
            this.AttributesGroupBox.Controls.Add(this.A1UpDown);
            this.AttributesGroupBox.Controls.Add(this.A1Label);
            this.AttributesGroupBox.Location = new System.Drawing.Point(12, 318);
            this.AttributesGroupBox.Name = "AttributesGroupBox";
            this.AttributesGroupBox.Size = new System.Drawing.Size(534, 95);
            this.AttributesGroupBox.TabIndex = 5;
            this.AttributesGroupBox.TabStop = false;
            this.AttributesGroupBox.Text = "Corner Attributes (a1-a4)";
            //
            // A1Label
            //
            this.A1Label.AutoSize = true;
            this.A1Label.Location = new System.Drawing.Point(6, 24);
            this.A1Label.Name = "A1Label";
            this.A1Label.Size = new System.Drawing.Size(22, 13);
            this.A1Label.TabIndex = 0;
            this.A1Label.Text = "a1:";
            //
            // A1UpDown
            //
            this.A1UpDown.DecimalPlaces = 4;
            this.A1UpDown.Location = new System.Drawing.Point(88, 22);
            this.A1UpDown.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.A1UpDown.Minimum = new decimal(new int[] { 1000000, 0, 0, -2147483648 });
            this.A1UpDown.Name = "A1UpDown";
            this.A1UpDown.Size = new System.Drawing.Size(154, 20);
            this.A1UpDown.TabIndex = 1;
            this.A1UpDown.ValueChanged += new System.EventHandler(this.A1UpDown_ValueChanged);
            //
            // A2Label
            //
            this.A2Label.AutoSize = true;
            this.A2Label.Location = new System.Drawing.Point(260, 24);
            this.A2Label.Name = "A2Label";
            this.A2Label.Size = new System.Drawing.Size(22, 13);
            this.A2Label.TabIndex = 2;
            this.A2Label.Text = "a2:";
            //
            // A2UpDown
            //
            this.A2UpDown.DecimalPlaces = 4;
            this.A2UpDown.Location = new System.Drawing.Point(325, 22);
            this.A2UpDown.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.A2UpDown.Minimum = new decimal(new int[] { 1000000, 0, 0, -2147483648 });
            this.A2UpDown.Name = "A2UpDown";
            this.A2UpDown.Size = new System.Drawing.Size(154, 20);
            this.A2UpDown.TabIndex = 3;
            this.A2UpDown.ValueChanged += new System.EventHandler(this.A2UpDown_ValueChanged);
            //
            // A3Label
            //
            this.A3Label.AutoSize = true;
            this.A3Label.Location = new System.Drawing.Point(6, 54);
            this.A3Label.Name = "A3Label";
            this.A3Label.Size = new System.Drawing.Size(22, 13);
            this.A3Label.TabIndex = 4;
            this.A3Label.Text = "a3:";
            //
            // A3UpDown
            //
            this.A3UpDown.DecimalPlaces = 4;
            this.A3UpDown.Location = new System.Drawing.Point(88, 52);
            this.A3UpDown.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.A3UpDown.Minimum = new decimal(new int[] { 1000000, 0, 0, -2147483648 });
            this.A3UpDown.Name = "A3UpDown";
            this.A3UpDown.Size = new System.Drawing.Size(154, 20);
            this.A3UpDown.TabIndex = 5;
            this.A3UpDown.ValueChanged += new System.EventHandler(this.A3UpDown_ValueChanged);
            //
            // A4Label
            //
            this.A4Label.AutoSize = true;
            this.A4Label.Location = new System.Drawing.Point(260, 54);
            this.A4Label.Name = "A4Label";
            this.A4Label.Size = new System.Drawing.Size(22, 13);
            this.A4Label.TabIndex = 6;
            this.A4Label.Text = "a4:";
            //
            // A4UpDown
            //
            this.A4UpDown.DecimalPlaces = 4;
            this.A4UpDown.Location = new System.Drawing.Point(325, 52);
            this.A4UpDown.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.A4UpDown.Minimum = new decimal(new int[] { 1000000, 0, 0, -2147483648 });
            this.A4UpDown.Name = "A4UpDown";
            this.A4UpDown.Size = new System.Drawing.Size(154, 20);
            this.A4UpDown.TabIndex = 7;
            this.A4UpDown.ValueChanged += new System.EventHandler(this.A4UpDown_ValueChanged);
            //
            // EditWaterQuadPanel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(553, 430);
            this.Controls.Add(this.AttributesGroupBox);
            this.Controls.Add(this.PropertiesGroupBox);
            this.Controls.Add(this.ExtentsGroupBox);
            this.Controls.Add(this.IndexTextBox);
            this.Controls.Add(this.IndexLabel);
            this.Controls.Add(this.GoToButton);
            this.Name = "EditWaterQuadPanel";
            this.Text = "Water Quad";
            this.ExtentsGroupBox.ResumeLayout(false);
            this.ExtentsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ZUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxYUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinYUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxXUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinXUpDown)).EndInit();
            this.PropertiesGroupBox.ResumeLayout(false);
            this.PropertiesGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TypeUpDown)).EndInit();
            this.AttributesGroupBox.ResumeLayout(false);
            this.AttributesGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.A4UpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.A3UpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.A2UpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.A1UpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button GoToButton;
        private System.Windows.Forms.Label IndexLabel;
        private System.Windows.Forms.TextBox IndexTextBox;
        private System.Windows.Forms.GroupBox ExtentsGroupBox;
        private System.Windows.Forms.NumericUpDown ZUpDown;
        private System.Windows.Forms.Label ZLabel;
        private System.Windows.Forms.NumericUpDown MaxYUpDown;
        private System.Windows.Forms.Label MaxYLabel;
        private System.Windows.Forms.NumericUpDown MinYUpDown;
        private System.Windows.Forms.Label MinYLabel;
        private System.Windows.Forms.NumericUpDown MaxXUpDown;
        private System.Windows.Forms.Label MaxXLabel;
        private System.Windows.Forms.NumericUpDown MinXUpDown;
        private System.Windows.Forms.Label MinXLabel;
        private System.Windows.Forms.GroupBox PropertiesGroupBox;
        private System.Windows.Forms.CheckBox NoStencilCheckBox;
        private System.Windows.Forms.CheckBox HasLimitedDepthCheckBox;
        private System.Windows.Forms.CheckBox IsInvisibleCheckBox;
        private System.Windows.Forms.NumericUpDown TypeUpDown;
        private System.Windows.Forms.Label TypeLabel;
        private System.Windows.Forms.GroupBox AttributesGroupBox;
        private System.Windows.Forms.NumericUpDown A4UpDown;
        private System.Windows.Forms.Label A4Label;
        private System.Windows.Forms.NumericUpDown A3UpDown;
        private System.Windows.Forms.Label A3Label;
        private System.Windows.Forms.NumericUpDown A2UpDown;
        private System.Windows.Forms.Label A2Label;
        private System.Windows.Forms.NumericUpDown A1UpDown;
        private System.Windows.Forms.Label A1Label;
    }
}

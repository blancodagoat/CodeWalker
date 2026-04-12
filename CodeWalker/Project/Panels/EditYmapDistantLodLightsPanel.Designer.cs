
namespace CodeWalker.Project.Panels
{
    partial class EditYmapDistantLodLightsPanel
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
            this.AddToProjectButton = new System.Windows.Forms.Button();
            this.GoToButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.LightCountLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.NumStreetLightsUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.CategoryUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.BBMinTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.BBMaxTextBox = new System.Windows.Forms.TextBox();
            this.SelectedLightGroupBox = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.LightIndexUpDown = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.LightPositionTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.LightColourLabel = new System.Windows.Forms.Label();
            this.LightColourRUpDown = new System.Windows.Forms.NumericUpDown();
            this.LightColourGUpDown = new System.Windows.Forms.NumericUpDown();
            this.LightColourBUpDown = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.LightIntensityUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.NumStreetLightsUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CategoryUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LightIndexUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LightColourRUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LightColourGUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LightColourBUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LightIntensityUpDown)).BeginInit();
            this.SelectedLightGroupBox.SuspendLayout();
            this.SuspendLayout();
            //
            // AddToProjectButton
            //
            this.AddToProjectButton.Location = new System.Drawing.Point(12, 12);
            this.AddToProjectButton.Name = "AddToProjectButton";
            this.AddToProjectButton.Size = new System.Drawing.Size(95, 23);
            this.AddToProjectButton.TabIndex = 0;
            this.AddToProjectButton.Text = "Add to Project";
            this.AddToProjectButton.UseVisualStyleBackColor = true;
            this.AddToProjectButton.Click += new System.EventHandler(this.AddToProjectButton_Click);
            //
            // GoToButton
            //
            this.GoToButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GoToButton.Location = new System.Drawing.Point(478, 12);
            this.GoToButton.Name = "GoToButton";
            this.GoToButton.Size = new System.Drawing.Size(68, 23);
            this.GoToButton.TabIndex = 1;
            this.GoToButton.Text = "Go to";
            this.GoToButton.UseVisualStyleBackColor = true;
            this.GoToButton.Click += new System.EventHandler(this.GoToButton_Click);
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Light count:";
            //
            // LightCountLabel
            //
            this.LightCountLabel.AutoSize = true;
            this.LightCountLabel.Location = new System.Drawing.Point(100, 48);
            this.LightCountLabel.Name = "LightCountLabel";
            this.LightCountLabel.Size = new System.Drawing.Size(13, 13);
            this.LightCountLabel.TabIndex = 3;
            this.LightCountLabel.Text = "0";
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Num street lights:";
            //
            // NumStreetLightsUpDown
            //
            this.NumStreetLightsUpDown.Location = new System.Drawing.Point(100, 69);
            this.NumStreetLightsUpDown.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            this.NumStreetLightsUpDown.Name = "NumStreetLightsUpDown";
            this.NumStreetLightsUpDown.Size = new System.Drawing.Size(154, 20);
            this.NumStreetLightsUpDown.TabIndex = 5;
            this.NumStreetLightsUpDown.ValueChanged += new System.EventHandler(this.NumStreetLightsUpDown_ValueChanged);
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Category:";
            //
            // CategoryUpDown
            //
            this.CategoryUpDown.Location = new System.Drawing.Point(100, 95);
            this.CategoryUpDown.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            this.CategoryUpDown.Name = "CategoryUpDown";
            this.CategoryUpDown.Size = new System.Drawing.Size(154, 20);
            this.CategoryUpDown.TabIndex = 7;
            this.CategoryUpDown.ValueChanged += new System.EventHandler(this.CategoryUpDown_ValueChanged);
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "BB Min:";
            //
            // BBMinTextBox
            //
            this.BBMinTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.BBMinTextBox.Location = new System.Drawing.Point(100, 121);
            this.BBMinTextBox.Name = "BBMinTextBox";
            this.BBMinTextBox.ReadOnly = true;
            this.BBMinTextBox.Size = new System.Drawing.Size(390, 20);
            this.BBMinTextBox.TabIndex = 9;
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 150);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "BB Max:";
            //
            // BBMaxTextBox
            //
            this.BBMaxTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.BBMaxTextBox.Location = new System.Drawing.Point(100, 147);
            this.BBMaxTextBox.Name = "BBMaxTextBox";
            this.BBMaxTextBox.ReadOnly = true;
            this.BBMaxTextBox.Size = new System.Drawing.Size(390, 20);
            this.BBMaxTextBox.TabIndex = 11;
            //
            // SelectedLightGroupBox
            //
            this.SelectedLightGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectedLightGroupBox.Controls.Add(this.LightIntensityUpDown);
            this.SelectedLightGroupBox.Controls.Add(this.label9);
            this.SelectedLightGroupBox.Controls.Add(this.LightColourBUpDown);
            this.SelectedLightGroupBox.Controls.Add(this.LightColourGUpDown);
            this.SelectedLightGroupBox.Controls.Add(this.LightColourRUpDown);
            this.SelectedLightGroupBox.Controls.Add(this.LightColourLabel);
            this.SelectedLightGroupBox.Controls.Add(this.label8);
            this.SelectedLightGroupBox.Controls.Add(this.LightPositionTextBox);
            this.SelectedLightGroupBox.Controls.Add(this.label7);
            this.SelectedLightGroupBox.Controls.Add(this.LightIndexUpDown);
            this.SelectedLightGroupBox.Controls.Add(this.label6);
            this.SelectedLightGroupBox.Location = new System.Drawing.Point(12, 180);
            this.SelectedLightGroupBox.Name = "SelectedLightGroupBox";
            this.SelectedLightGroupBox.Size = new System.Drawing.Size(534, 165);
            this.SelectedLightGroupBox.TabIndex = 12;
            this.SelectedLightGroupBox.TabStop = false;
            this.SelectedLightGroupBox.Text = "Selected Light";
            //
            // label6
            //
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Index:";
            //
            // LightIndexUpDown
            //
            this.LightIndexUpDown.Location = new System.Drawing.Point(88, 22);
            this.LightIndexUpDown.Name = "LightIndexUpDown";
            this.LightIndexUpDown.Size = new System.Drawing.Size(154, 20);
            this.LightIndexUpDown.TabIndex = 1;
            this.LightIndexUpDown.ValueChanged += new System.EventHandler(this.LightIndexUpDown_ValueChanged);
            //
            // label7
            //
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Position:";
            //
            // LightPositionTextBox
            //
            this.LightPositionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.LightPositionTextBox.Location = new System.Drawing.Point(88, 47);
            this.LightPositionTextBox.Name = "LightPositionTextBox";
            this.LightPositionTextBox.Size = new System.Drawing.Size(390, 20);
            this.LightPositionTextBox.TabIndex = 3;
            this.LightPositionTextBox.TextChanged += new System.EventHandler(this.LightPositionTextBox_TextChanged);
            //
            // label8
            //
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 78);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Colour (RGB):";
            //
            // LightColourLabel
            //
            this.LightColourLabel.BackColor = System.Drawing.Color.White;
            this.LightColourLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LightColourLabel.Location = new System.Drawing.Point(88, 75);
            this.LightColourLabel.Name = "LightColourLabel";
            this.LightColourLabel.Size = new System.Drawing.Size(30, 21);
            this.LightColourLabel.TabIndex = 5;
            this.LightColourLabel.Click += new System.EventHandler(this.LightColourLabel_Click);
            //
            // LightColourRUpDown
            //
            this.LightColourRUpDown.Location = new System.Drawing.Point(122, 76);
            this.LightColourRUpDown.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            this.LightColourRUpDown.Name = "LightColourRUpDown";
            this.LightColourRUpDown.Size = new System.Drawing.Size(38, 20);
            this.LightColourRUpDown.TabIndex = 6;
            this.LightColourRUpDown.ValueChanged += new System.EventHandler(this.LightColourRUpDown_ValueChanged);
            //
            // LightColourGUpDown
            //
            this.LightColourGUpDown.Location = new System.Drawing.Point(163, 76);
            this.LightColourGUpDown.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            this.LightColourGUpDown.Name = "LightColourGUpDown";
            this.LightColourGUpDown.Size = new System.Drawing.Size(38, 20);
            this.LightColourGUpDown.TabIndex = 7;
            this.LightColourGUpDown.ValueChanged += new System.EventHandler(this.LightColourGUpDown_ValueChanged);
            //
            // LightColourBUpDown
            //
            this.LightColourBUpDown.Location = new System.Drawing.Point(204, 76);
            this.LightColourBUpDown.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            this.LightColourBUpDown.Name = "LightColourBUpDown";
            this.LightColourBUpDown.Size = new System.Drawing.Size(38, 20);
            this.LightColourBUpDown.TabIndex = 8;
            this.LightColourBUpDown.ValueChanged += new System.EventHandler(this.LightColourBUpDown_ValueChanged);
            //
            // label9
            //
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 106);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 13);
            this.label9.TabIndex = 9;
            this.label9.Text = "Intensity:";
            //
            // LightIntensityUpDown
            //
            this.LightIntensityUpDown.Location = new System.Drawing.Point(88, 104);
            this.LightIntensityUpDown.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            this.LightIntensityUpDown.Name = "LightIntensityUpDown";
            this.LightIntensityUpDown.Size = new System.Drawing.Size(154, 20);
            this.LightIntensityUpDown.TabIndex = 10;
            this.LightIntensityUpDown.ValueChanged += new System.EventHandler(this.LightIntensityUpDown_ValueChanged);
            //
            // EditYmapDistantLodLightsPanel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(553, 360);
            this.Controls.Add(this.SelectedLightGroupBox);
            this.Controls.Add(this.BBMaxTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.BBMinTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.CategoryUpDown);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.NumStreetLightsUpDown);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.LightCountLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GoToButton);
            this.Controls.Add(this.AddToProjectButton);
            this.Name = "EditYmapDistantLodLightsPanel";
            this.Text = "Distant LOD Lights";
            ((System.ComponentModel.ISupportInitialize)(this.NumStreetLightsUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CategoryUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LightIndexUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LightColourRUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LightColourGUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LightColourBUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LightIntensityUpDown)).EndInit();
            this.SelectedLightGroupBox.ResumeLayout(false);
            this.SelectedLightGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AddToProjectButton;
        private System.Windows.Forms.Button GoToButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LightCountLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown NumStreetLightsUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown CategoryUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox BBMinTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox BBMaxTextBox;
        private System.Windows.Forms.GroupBox SelectedLightGroupBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown LightIndexUpDown;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox LightPositionTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label LightColourLabel;
        private System.Windows.Forms.NumericUpDown LightColourRUpDown;
        private System.Windows.Forms.NumericUpDown LightColourGUpDown;
        private System.Windows.Forms.NumericUpDown LightColourBUpDown;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown LightIntensityUpDown;
    }
}

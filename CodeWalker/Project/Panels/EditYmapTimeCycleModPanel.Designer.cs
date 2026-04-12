
namespace CodeWalker.Project.Panels
{
    partial class EditYmapTimeCycleModPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditYmapTimeCycleModPanel));
            this.GoToButton = new System.Windows.Forms.Button();
            this.AddToProjectButton = new System.Windows.Forms.Button();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.NameLabel = new System.Windows.Forms.Label();
            this.NameHashLabel = new System.Windows.Forms.Label();
            this.PositionTextBox = new System.Windows.Forms.TextBox();
            this.PositionLabel = new System.Windows.Forms.Label();
            this.MinExtentsTextBox = new System.Windows.Forms.TextBox();
            this.MinExtentsLabel = new System.Windows.Forms.Label();
            this.MaxExtentsTextBox = new System.Windows.Forms.TextBox();
            this.MaxExtentsLabel = new System.Windows.Forms.Label();
            this.PercentageTextBox = new System.Windows.Forms.TextBox();
            this.PercentageLabel = new System.Windows.Forms.Label();
            this.RangeTextBox = new System.Windows.Forms.TextBox();
            this.RangeLabel = new System.Windows.Forms.Label();
            this.StartHourUpDown = new System.Windows.Forms.NumericUpDown();
            this.StartHourLabel = new System.Windows.Forms.Label();
            this.EndHourUpDown = new System.Windows.Forms.NumericUpDown();
            this.EndHourLabel = new System.Windows.Forms.Label();
            this.ModDataPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.ModDataLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.StartHourUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndHourUpDown)).BeginInit();
            this.SuspendLayout();
            //
            // GoToButton
            //
            this.GoToButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GoToButton.Location = new System.Drawing.Point(478, 6);
            this.GoToButton.Name = "GoToButton";
            this.GoToButton.Size = new System.Drawing.Size(68, 23);
            this.GoToButton.TabIndex = 0;
            this.GoToButton.Text = "Go to";
            this.GoToButton.UseVisualStyleBackColor = true;
            this.GoToButton.Click += new System.EventHandler(this.GoToButton_Click);
            //
            // AddToProjectButton
            //
            this.AddToProjectButton.Location = new System.Drawing.Point(105, 230);
            this.AddToProjectButton.Name = "AddToProjectButton";
            this.AddToProjectButton.Size = new System.Drawing.Size(110, 23);
            this.AddToProjectButton.TabIndex = 1;
            this.AddToProjectButton.Text = "Add Ymap to Project";
            this.AddToProjectButton.UseVisualStyleBackColor = true;
            this.AddToProjectButton.Click += new System.EventHandler(this.AddToProjectButton_Click);
            //
            // DeleteButton
            //
            this.DeleteButton.Location = new System.Drawing.Point(221, 230);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(95, 23);
            this.DeleteButton.TabIndex = 2;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Enabled = false;
            //
            // NameTextBox
            //
            this.NameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NameTextBox.Location = new System.Drawing.Point(105, 8);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(367, 20);
            this.NameTextBox.TabIndex = 3;
            this.NameTextBox.TextChanged += new System.EventHandler(this.NameTextBox_TextChanged);
            //
            // NameLabel
            //
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(4, 11);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(38, 13);
            this.NameLabel.TabIndex = 4;
            this.NameLabel.Text = "Name:";
            //
            // NameHashLabel
            //
            this.NameHashLabel.AutoSize = true;
            this.NameHashLabel.Location = new System.Drawing.Point(102, 32);
            this.NameHashLabel.Name = "NameHashLabel";
            this.NameHashLabel.Size = new System.Drawing.Size(40, 13);
            this.NameHashLabel.TabIndex = 5;
            this.NameHashLabel.Text = "Hash: 0";
            //
            // PositionTextBox
            //
            this.PositionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PositionTextBox.Location = new System.Drawing.Point(105, 52);
            this.PositionTextBox.Name = "PositionTextBox";
            this.PositionTextBox.ReadOnly = true;
            this.PositionTextBox.Size = new System.Drawing.Size(367, 20);
            this.PositionTextBox.TabIndex = 6;
            //
            // PositionLabel
            //
            this.PositionLabel.AutoSize = true;
            this.PositionLabel.Location = new System.Drawing.Point(4, 55);
            this.PositionLabel.Name = "PositionLabel";
            this.PositionLabel.Size = new System.Drawing.Size(49, 13);
            this.PositionLabel.TabIndex = 7;
            this.PositionLabel.Text = "Position:";
            //
            // MinExtentsTextBox
            //
            this.MinExtentsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MinExtentsTextBox.Location = new System.Drawing.Point(105, 78);
            this.MinExtentsTextBox.Name = "MinExtentsTextBox";
            this.MinExtentsTextBox.Size = new System.Drawing.Size(367, 20);
            this.MinExtentsTextBox.TabIndex = 8;
            this.MinExtentsTextBox.TextChanged += new System.EventHandler(this.MinExtentsTextBox_TextChanged);
            //
            // MinExtentsLabel
            //
            this.MinExtentsLabel.AutoSize = true;
            this.MinExtentsLabel.Location = new System.Drawing.Point(4, 81);
            this.MinExtentsLabel.Name = "MinExtentsLabel";
            this.MinExtentsLabel.Size = new System.Drawing.Size(64, 13);
            this.MinExtentsLabel.TabIndex = 9;
            this.MinExtentsLabel.Text = "Min Extents:";
            //
            // MaxExtentsTextBox
            //
            this.MaxExtentsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MaxExtentsTextBox.Location = new System.Drawing.Point(105, 104);
            this.MaxExtentsTextBox.Name = "MaxExtentsTextBox";
            this.MaxExtentsTextBox.Size = new System.Drawing.Size(367, 20);
            this.MaxExtentsTextBox.TabIndex = 10;
            this.MaxExtentsTextBox.TextChanged += new System.EventHandler(this.MaxExtentsTextBox_TextChanged);
            //
            // MaxExtentsLabel
            //
            this.MaxExtentsLabel.AutoSize = true;
            this.MaxExtentsLabel.Location = new System.Drawing.Point(4, 107);
            this.MaxExtentsLabel.Name = "MaxExtentsLabel";
            this.MaxExtentsLabel.Size = new System.Drawing.Size(67, 13);
            this.MaxExtentsLabel.TabIndex = 11;
            this.MaxExtentsLabel.Text = "Max Extents:";
            //
            // PercentageTextBox
            //
            this.PercentageTextBox.Location = new System.Drawing.Point(105, 130);
            this.PercentageTextBox.Name = "PercentageTextBox";
            this.PercentageTextBox.Size = new System.Drawing.Size(100, 20);
            this.PercentageTextBox.TabIndex = 12;
            this.PercentageTextBox.TextChanged += new System.EventHandler(this.PercentageTextBox_TextChanged);
            //
            // PercentageLabel
            //
            this.PercentageLabel.AutoSize = true;
            this.PercentageLabel.Location = new System.Drawing.Point(4, 133);
            this.PercentageLabel.Name = "PercentageLabel";
            this.PercentageLabel.Size = new System.Drawing.Size(65, 13);
            this.PercentageLabel.TabIndex = 13;
            this.PercentageLabel.Text = "Percentage:";
            //
            // RangeTextBox
            //
            this.RangeTextBox.Location = new System.Drawing.Point(105, 156);
            this.RangeTextBox.Name = "RangeTextBox";
            this.RangeTextBox.Size = new System.Drawing.Size(100, 20);
            this.RangeTextBox.TabIndex = 14;
            this.RangeTextBox.TextChanged += new System.EventHandler(this.RangeTextBox_TextChanged);
            //
            // RangeLabel
            //
            this.RangeLabel.AutoSize = true;
            this.RangeLabel.Location = new System.Drawing.Point(4, 159);
            this.RangeLabel.Name = "RangeLabel";
            this.RangeLabel.Size = new System.Drawing.Size(42, 13);
            this.RangeLabel.TabIndex = 15;
            this.RangeLabel.Text = "Range:";
            //
            // StartHourUpDown
            //
            this.StartHourUpDown.Location = new System.Drawing.Point(105, 182);
            this.StartHourUpDown.Maximum = new decimal(new int[] { 24, 0, 0, 0 });
            this.StartHourUpDown.Name = "StartHourUpDown";
            this.StartHourUpDown.Size = new System.Drawing.Size(60, 20);
            this.StartHourUpDown.TabIndex = 16;
            this.StartHourUpDown.ValueChanged += new System.EventHandler(this.StartHourUpDown_ValueChanged);
            //
            // StartHourLabel
            //
            this.StartHourLabel.AutoSize = true;
            this.StartHourLabel.Location = new System.Drawing.Point(4, 184);
            this.StartHourLabel.Name = "StartHourLabel";
            this.StartHourLabel.Size = new System.Drawing.Size(58, 13);
            this.StartHourLabel.TabIndex = 17;
            this.StartHourLabel.Text = "Start Hour:";
            //
            // EndHourUpDown
            //
            this.EndHourUpDown.Location = new System.Drawing.Point(275, 182);
            this.EndHourUpDown.Maximum = new decimal(new int[] { 24, 0, 0, 0 });
            this.EndHourUpDown.Name = "EndHourUpDown";
            this.EndHourUpDown.Size = new System.Drawing.Size(60, 20);
            this.EndHourUpDown.TabIndex = 18;
            this.EndHourUpDown.ValueChanged += new System.EventHandler(this.EndHourUpDown_ValueChanged);
            //
            // EndHourLabel
            //
            this.EndHourLabel.AutoSize = true;
            this.EndHourLabel.Location = new System.Drawing.Point(211, 184);
            this.EndHourLabel.Name = "EndHourLabel";
            this.EndHourLabel.Size = new System.Drawing.Size(55, 13);
            this.EndHourLabel.TabIndex = 19;
            this.EndHourLabel.Text = "End Hour:";
            //
            // ModDataLabel
            //
            this.ModDataLabel.AutoSize = true;
            this.ModDataLabel.Location = new System.Drawing.Point(4, 265);
            this.ModDataLabel.Name = "ModDataLabel";
            this.ModDataLabel.Size = new System.Drawing.Size(107, 13);
            this.ModDataLabel.TabIndex = 20;
            this.ModDataLabel.Text = "Modifier Definition:";
            //
            // ModDataPropertyGrid
            //
            this.ModDataPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ModDataPropertyGrid.HelpVisible = false;
            this.ModDataPropertyGrid.Location = new System.Drawing.Point(7, 281);
            this.ModDataPropertyGrid.Name = "ModDataPropertyGrid";
            this.ModDataPropertyGrid.Size = new System.Drawing.Size(540, 120);
            this.ModDataPropertyGrid.TabIndex = 21;
            this.ModDataPropertyGrid.ToolbarVisible = false;
            //
            // EditYmapTimeCycleModPanel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(553, 407);
            this.Controls.Add(this.ModDataPropertyGrid);
            this.Controls.Add(this.ModDataLabel);
            this.Controls.Add(this.EndHourLabel);
            this.Controls.Add(this.EndHourUpDown);
            this.Controls.Add(this.StartHourLabel);
            this.Controls.Add(this.StartHourUpDown);
            this.Controls.Add(this.RangeLabel);
            this.Controls.Add(this.RangeTextBox);
            this.Controls.Add(this.PercentageLabel);
            this.Controls.Add(this.PercentageTextBox);
            this.Controls.Add(this.MaxExtentsLabel);
            this.Controls.Add(this.MaxExtentsTextBox);
            this.Controls.Add(this.MinExtentsLabel);
            this.Controls.Add(this.MinExtentsTextBox);
            this.Controls.Add(this.PositionLabel);
            this.Controls.Add(this.PositionTextBox);
            this.Controls.Add(this.NameHashLabel);
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this.NameTextBox);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.AddToProjectButton);
            this.Controls.Add(this.GoToButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EditYmapTimeCycleModPanel";
            this.Text = "Time Cycle Modifier";
            ((System.ComponentModel.ISupportInitialize)(this.StartHourUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndHourUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button GoToButton;
        private System.Windows.Forms.Button AddToProjectButton;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.Label NameHashLabel;
        private System.Windows.Forms.TextBox PositionTextBox;
        private System.Windows.Forms.Label PositionLabel;
        private System.Windows.Forms.TextBox MinExtentsTextBox;
        private System.Windows.Forms.Label MinExtentsLabel;
        private System.Windows.Forms.TextBox MaxExtentsTextBox;
        private System.Windows.Forms.Label MaxExtentsLabel;
        private System.Windows.Forms.TextBox PercentageTextBox;
        private System.Windows.Forms.Label PercentageLabel;
        private System.Windows.Forms.TextBox RangeTextBox;
        private System.Windows.Forms.Label RangeLabel;
        private System.Windows.Forms.NumericUpDown StartHourUpDown;
        private System.Windows.Forms.Label StartHourLabel;
        private System.Windows.Forms.NumericUpDown EndHourUpDown;
        private System.Windows.Forms.Label EndHourLabel;
        private System.Windows.Forms.PropertyGrid ModDataPropertyGrid;
        private System.Windows.Forms.Label ModDataLabel;
    }
}

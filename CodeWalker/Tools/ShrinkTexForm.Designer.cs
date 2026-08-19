namespace CodeWalker.Tools
{
    partial class ShrinkTexForm
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
            this.components = new System.ComponentModel.Container();
            this.InputLabel = new System.Windows.Forms.Label();
            this.InputTextBox = new System.Windows.Forms.TextBox();
            this.InputFolderBrowseButton = new System.Windows.Forms.Button();
            this.InputRpfBrowseButton = new System.Windows.Forms.Button();
            this.OutputLabel = new System.Windows.Forms.Label();
            this.OutputTextBox = new System.Windows.Forms.TextBox();
            this.OutputBrowseButton = new System.Windows.Forms.Button();
            this.OutputFolderRadio = new System.Windows.Forms.RadioButton();
            this.OutputRpfRadio = new System.Windows.Forms.RadioButton();
            this.MaxSizeLabel = new System.Windows.Forms.Label();
            this.MaxSizeComboBox = new System.Windows.Forms.ComboBox();
            this.GenLodsCheckBox = new System.Windows.Forms.CheckBox();
            this.ShrinkButton = new System.Windows.Forms.Button();
            this.AbortButton = new System.Windows.Forms.Button();
            this.LogTextBox = new System.Windows.Forms.TextBox();
            this.InfoLabel = new System.Windows.Forms.Label();
            this.FolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.OpenRpfDialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            //
            // InputLabel
            //
            this.InputLabel.AutoSize = true;
            this.InputLabel.Location = new System.Drawing.Point(12, 15);
            this.InputLabel.Name = "InputLabel";
            this.InputLabel.Size = new System.Drawing.Size(38, 15);
            this.InputLabel.TabIndex = 0;
            this.InputLabel.Text = "Input:";
            //
            // InputTextBox
            //
            this.InputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InputTextBox.Location = new System.Drawing.Point(101, 12);
            this.InputTextBox.Name = "InputTextBox";
            this.InputTextBox.Size = new System.Drawing.Size(367, 23);
            this.InputTextBox.TabIndex = 1;
            this.InputTextBox.TextChanged += new System.EventHandler(this.UpdateOutputPath);
            //
            // InputFolderBrowseButton
            //
            this.InputFolderBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.InputFolderBrowseButton.Location = new System.Drawing.Point(474, 11);
            this.InputFolderBrowseButton.Name = "InputFolderBrowseButton";
            this.InputFolderBrowseButton.Size = new System.Drawing.Size(60, 25);
            this.InputFolderBrowseButton.TabIndex = 2;
            this.InputFolderBrowseButton.Text = "Folder...";
            this.InputFolderBrowseButton.UseVisualStyleBackColor = true;
            this.InputFolderBrowseButton.Click += new System.EventHandler(this.InputFolderBrowseButton_Click);
            //
            // InputRpfBrowseButton
            //
            this.InputRpfBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.InputRpfBrowseButton.Location = new System.Drawing.Point(540, 11);
            this.InputRpfBrowseButton.Name = "InputRpfBrowseButton";
            this.InputRpfBrowseButton.Size = new System.Drawing.Size(52, 25);
            this.InputRpfBrowseButton.TabIndex = 3;
            this.InputRpfBrowseButton.Text = "RPF...";
            this.InputRpfBrowseButton.UseVisualStyleBackColor = true;
            this.InputRpfBrowseButton.Click += new System.EventHandler(this.InputRpfBrowseButton_Click);
            //
            // OutputLabel
            //
            this.OutputLabel.AutoSize = true;
            this.OutputLabel.Location = new System.Drawing.Point(12, 44);
            this.OutputLabel.Name = "OutputLabel";
            this.OutputLabel.Size = new System.Drawing.Size(48, 15);
            this.OutputLabel.TabIndex = 4;
            this.OutputLabel.Text = "Output:";
            //
            // OutputTextBox
            //
            this.OutputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputTextBox.Location = new System.Drawing.Point(101, 41);
            this.OutputTextBox.Name = "OutputTextBox";
            this.OutputTextBox.Size = new System.Drawing.Size(433, 23);
            this.OutputTextBox.TabIndex = 5;
            //
            // OutputBrowseButton
            //
            this.OutputBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputBrowseButton.Location = new System.Drawing.Point(540, 40);
            this.OutputBrowseButton.Name = "OutputBrowseButton";
            this.OutputBrowseButton.Size = new System.Drawing.Size(52, 25);
            this.OutputBrowseButton.TabIndex = 6;
            this.OutputBrowseButton.Text = "...";
            this.OutputBrowseButton.UseVisualStyleBackColor = true;
            this.OutputBrowseButton.Click += new System.EventHandler(this.OutputBrowseButton_Click);
            //
            // OutputFolderRadio
            //
            this.OutputFolderRadio.AutoSize = true;
            this.OutputFolderRadio.Location = new System.Drawing.Point(101, 70);
            this.OutputFolderRadio.Name = "OutputFolderRadio";
            this.OutputFolderRadio.Size = new System.Drawing.Size(220, 19);
            this.OutputFolderRadio.TabIndex = 7;
            this.OutputFolderRadio.TabStop = true;
            this.OutputFolderRadio.Text = "Folder for tex_overrides (drag && drop)";
            this.OutputFolderRadio.UseVisualStyleBackColor = true;
            this.OutputFolderRadio.CheckedChanged += new System.EventHandler(this.UpdateOutputPath);
            //
            // OutputRpfRadio
            //
            this.OutputRpfRadio.AutoSize = true;
            this.OutputRpfRadio.Location = new System.Drawing.Point(340, 70);
            this.OutputRpfRadio.Name = "OutputRpfRadio";
            this.OutputRpfRadio.Size = new System.Drawing.Size(170, 19);
            this.OutputRpfRadio.TabIndex = 8;
            this.OutputRpfRadio.TabStop = true;
            this.OutputRpfRadio.Text = "RPF (same layout as input)";
            this.OutputRpfRadio.UseVisualStyleBackColor = true;
            this.OutputRpfRadio.CheckedChanged += new System.EventHandler(this.UpdateOutputPath);
            //
            // MaxSizeLabel
            //
            this.MaxSizeLabel.AutoSize = true;
            this.MaxSizeLabel.Location = new System.Drawing.Point(12, 100);
            this.MaxSizeLabel.Name = "MaxSizeLabel";
            this.MaxSizeLabel.Size = new System.Drawing.Size(85, 15);
            this.MaxSizeLabel.TabIndex = 9;
            this.MaxSizeLabel.Text = "Max size (px):";
            //
            // MaxSizeComboBox
            //
            this.MaxSizeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MaxSizeComboBox.FormattingEnabled = true;
            this.MaxSizeComboBox.Items.AddRange(new object[] {
            "1024",
            "2048",
            "4096"});
            this.MaxSizeComboBox.Location = new System.Drawing.Point(101, 97);
            this.MaxSizeComboBox.Name = "MaxSizeComboBox";
            this.MaxSizeComboBox.Size = new System.Drawing.Size(80, 23);
            this.MaxSizeComboBox.TabIndex = 10;
            //
            // ShrinkButton
            //
            this.ShrinkButton.Location = new System.Drawing.Point(197, 96);
            this.ShrinkButton.Name = "ShrinkButton";
            this.ShrinkButton.Size = new System.Drawing.Size(90, 25);
            this.ShrinkButton.TabIndex = 11;
            this.ShrinkButton.Text = "Shrink";
            this.ShrinkButton.UseVisualStyleBackColor = true;
            this.ShrinkButton.Click += new System.EventHandler(this.ShrinkButton_Click);
            //
            // GenLodsCheckBox
            //
            this.GenLodsCheckBox.AutoSize = true;
            this.GenLodsCheckBox.Location = new System.Drawing.Point(400, 99);
            this.GenLodsCheckBox.Name = "GenLodsCheckBox";
            this.GenLodsCheckBox.Size = new System.Drawing.Size(190, 19);
            this.GenLodsCheckBox.TabIndex = 15;
            this.GenLodsCheckBox.Text = "Generate missing LODs (.ydd)";
            this.GenLodsCheckBox.UseVisualStyleBackColor = true;
            //
            // AbortButton
            //
            this.AbortButton.Location = new System.Drawing.Point(293, 96);
            this.AbortButton.Name = "AbortButton";
            this.AbortButton.Size = new System.Drawing.Size(90, 25);
            this.AbortButton.TabIndex = 12;
            this.AbortButton.Text = "Abort";
            this.AbortButton.UseVisualStyleBackColor = true;
            this.AbortButton.Click += new System.EventHandler(this.AbortButton_Click);
            //
            // InfoLabel
            //
            this.InfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InfoLabel.Location = new System.Drawing.Point(12, 127);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(580, 32);
            this.InfoLabel.TabIndex = 13;
            this.InfoLabel.Text = "Downscales oversized ytd/ydd textures and recompresses uncompressed ones. Textures already " +
                "compressed and within the size cap are carried over unchanged. Input files are never modified.";
            //
            // LogTextBox
            //
            this.LogTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LogTextBox.Font = new System.Drawing.Font("Consolas", 9F);
            this.LogTextBox.Location = new System.Drawing.Point(12, 162);
            this.LogTextBox.Multiline = true;
            this.LogTextBox.Name = "LogTextBox";
            this.LogTextBox.ReadOnly = true;
            this.LogTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.LogTextBox.Size = new System.Drawing.Size(580, 287);
            this.LogTextBox.TabIndex = 14;
            this.LogTextBox.WordWrap = false;
            //
            // FolderBrowserDialog
            //
            this.FolderBrowserDialog.ShowNewFolderButton = true;
            //
            // OpenRpfDialog
            //
            this.OpenRpfDialog.Filter = "RPF archives|*.rpf|All files|*.*";
            this.OpenRpfDialog.Title = "Select an RPF archive";
            //
            // ShrinkTexForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 461);
            this.Controls.Add(this.GenLodsCheckBox);
            this.Controls.Add(this.LogTextBox);
            this.Controls.Add(this.InfoLabel);
            this.Controls.Add(this.AbortButton);
            this.Controls.Add(this.ShrinkButton);
            this.Controls.Add(this.MaxSizeComboBox);
            this.Controls.Add(this.MaxSizeLabel);
            this.Controls.Add(this.OutputRpfRadio);
            this.Controls.Add(this.OutputFolderRadio);
            this.Controls.Add(this.OutputBrowseButton);
            this.Controls.Add(this.OutputTextBox);
            this.Controls.Add(this.OutputLabel);
            this.Controls.Add(this.InputRpfBrowseButton);
            this.Controls.Add(this.InputFolderBrowseButton);
            this.Controls.Add(this.InputTextBox);
            this.Controls.Add(this.InputLabel);
            this.MinimumSize = new System.Drawing.Size(540, 380);
            this.Name = "ShrinkTexForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Shrink Textures - CodeWalker";
            this.Load += new System.EventHandler(this.ShrinkTexForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label InputLabel;
        private System.Windows.Forms.TextBox InputTextBox;
        private System.Windows.Forms.Button InputFolderBrowseButton;
        private System.Windows.Forms.Button InputRpfBrowseButton;
        private System.Windows.Forms.Label OutputLabel;
        private System.Windows.Forms.TextBox OutputTextBox;
        private System.Windows.Forms.Button OutputBrowseButton;
        private System.Windows.Forms.RadioButton OutputFolderRadio;
        private System.Windows.Forms.RadioButton OutputRpfRadio;
        private System.Windows.Forms.Label MaxSizeLabel;
        private System.Windows.Forms.ComboBox MaxSizeComboBox;
        private System.Windows.Forms.CheckBox GenLodsCheckBox;
        private System.Windows.Forms.Button ShrinkButton;
        private System.Windows.Forms.Button AbortButton;
        private System.Windows.Forms.TextBox LogTextBox;
        private System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog;
        private System.Windows.Forms.OpenFileDialog OpenRpfDialog;
    }
}

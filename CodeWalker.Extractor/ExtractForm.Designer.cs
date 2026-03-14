namespace CodeWalker.Extractor
{
    partial class ExtractForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.InputFolderTextBox = new System.Windows.Forms.TextBox();
            this.InputFolderBrowseButton = new System.Windows.Forms.Button();
            this.OutputFolderBrowseButton = new System.Windows.Forms.Button();
            this.OutputFolderTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.MetaCheckbox = new System.Windows.Forms.CheckBox();
            this.CarcolsCheckbox = new System.Windows.Forms.CheckBox();
            this.AudioCheckbox = new System.Windows.Forms.CheckBox();
            this.LangCheckbox = new System.Windows.Forms.CheckBox();
            this.TexturesCheckbox = new System.Windows.Forms.CheckBox();
            this.ModelsCheckbox = new System.Windows.Forms.CheckBox();
            this.YmapsCheckbox = new System.Windows.Forms.CheckBox();
            this.ExtractButton = new System.Windows.Forms.Button();
            this.LogTextBox = new System.Windows.Forms.TextBox();
            this.FolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.ExtractProgressBar = new System.Windows.Forms.ProgressBar();
            this.SelectAllCheckbox = new System.Windows.Forms.CheckBox();
            this.FileTypesGroupBox = new System.Windows.Forms.GroupBox();
            this.RPFOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.ConvertRPFCheckbox = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.FileTypesGroupBox.SuspendLayout();
            this.RPFOptionsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Input folder:";
            // 
            // InputFolderTextBox
            // 
            this.InputFolderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InputFolderTextBox.Location = new System.Drawing.Point(92, 38);
            this.InputFolderTextBox.Name = "InputFolderTextBox";
            this.InputFolderTextBox.Size = new System.Drawing.Size(449, 20);
            this.InputFolderTextBox.TabIndex = 2;
            // 
            // InputFolderBrowseButton
            // 
            this.InputFolderBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.InputFolderBrowseButton.Location = new System.Drawing.Point(556, 37);
            this.InputFolderBrowseButton.Name = "InputFolderBrowseButton";
            this.InputFolderBrowseButton.Size = new System.Drawing.Size(31, 22);
            this.InputFolderBrowseButton.TabIndex = 3;
            this.InputFolderBrowseButton.Text = "...";
            this.InputFolderBrowseButton.UseVisualStyleBackColor = true;
            this.InputFolderBrowseButton.Click += new System.EventHandler(this.InputFolderBrowseButton_Click);
            // 
            // OutputFolderBrowseButton
            // 
            this.OutputFolderBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputFolderBrowseButton.Location = new System.Drawing.Point(556, 63);
            this.OutputFolderBrowseButton.Name = "OutputFolderBrowseButton";
            this.OutputFolderBrowseButton.Size = new System.Drawing.Size(31, 22);
            this.OutputFolderBrowseButton.TabIndex = 6;
            this.OutputFolderBrowseButton.Text = "...";
            this.OutputFolderBrowseButton.UseVisualStyleBackColor = true;
            this.OutputFolderBrowseButton.Click += new System.EventHandler(this.OutputFolderBrowseButton_Click);
            // 
            // OutputFolderTextBox
            // 
            this.OutputFolderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputFolderTextBox.Location = new System.Drawing.Point(92, 64);
            this.OutputFolderTextBox.Name = "OutputFolderTextBox";
            this.OutputFolderTextBox.Size = new System.Drawing.Size(449, 20);
            this.OutputFolderTextBox.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Output folder:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(303, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Extract files from GTA V RPF archives.";
            // 
            // FileTypesGroupBox
            // 
            this.FileTypesGroupBox.Controls.Add(this.ModelsCheckbox);
            this.FileTypesGroupBox.Controls.Add(this.TexturesCheckbox);
            this.FileTypesGroupBox.Controls.Add(this.YmapsCheckbox);
            this.FileTypesGroupBox.Controls.Add(this.LangCheckbox);
            this.FileTypesGroupBox.Controls.Add(this.AudioCheckbox);
            this.FileTypesGroupBox.Controls.Add(this.CarcolsCheckbox);
            this.FileTypesGroupBox.Controls.Add(this.MetaCheckbox);
            this.FileTypesGroupBox.Controls.Add(this.SelectAllCheckbox);
            this.FileTypesGroupBox.Location = new System.Drawing.Point(16, 95);
            this.FileTypesGroupBox.Name = "FileTypesGroupBox";
            this.FileTypesGroupBox.Size = new System.Drawing.Size(571, 140);
            this.FileTypesGroupBox.TabIndex = 12;
            this.FileTypesGroupBox.TabStop = false;
            this.FileTypesGroupBox.Text = "File Types to Extract";
            // 
            // SelectAllCheckbox
            // 
            this.SelectAllCheckbox.AutoSize = true;
            this.SelectAllCheckbox.Location = new System.Drawing.Point(15, 25);
            this.SelectAllCheckbox.Name = "SelectAllCheckbox";
            this.SelectAllCheckbox.Size = new System.Drawing.Size(66, 17);
            this.SelectAllCheckbox.TabIndex = 13;
            this.SelectAllCheckbox.Text = "Select All";
            this.SelectAllCheckbox.UseVisualStyleBackColor = true;
            this.SelectAllCheckbox.CheckedChanged += new System.EventHandler(this.SelectAllCheckbox_CheckedChanged);
            // 
            // MetaCheckbox
            // 
            this.MetaCheckbox.AutoSize = true;
            this.MetaCheckbox.Checked = true;
            this.MetaCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MetaCheckbox.Location = new System.Drawing.Point(15, 50);
            this.MetaCheckbox.Name = "MetaCheckbox";
            this.MetaCheckbox.Size = new System.Drawing.Size(118, 17);
            this.MetaCheckbox.TabIndex = 0;
            this.MetaCheckbox.Text = ".meta files (Metadata)";
            this.MetaCheckbox.UseVisualStyleBackColor = true;
            // 
            // CarcolsCheckbox
            // 
            this.CarcolsCheckbox.AutoSize = true;
            this.CarcolsCheckbox.Checked = true;
            this.CarcolsCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CarcolsCheckbox.Location = new System.Drawing.Point(15, 73);
            this.CarcolsCheckbox.Name = "CarcolsCheckbox";
            this.CarcolsCheckbox.Size = new System.Drawing.Size(169, 17);
            this.CarcolsCheckbox.TabIndex = 1;
            this.CarcolsCheckbox.Text = "carcols, carvariations (Vehicles)";
            this.CarcolsCheckbox.UseVisualStyleBackColor = true;
            // 
            // AudioCheckbox
            // 
            this.AudioCheckbox.AutoSize = true;
            this.AudioCheckbox.Checked = true;
            this.AudioCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AudioCheckbox.Location = new System.Drawing.Point(220, 50);
            this.AudioCheckbox.Name = "AudioCheckbox";
            this.AudioCheckbox.Size = new System.Drawing.Size(136, 17);
            this.AudioCheckbox.TabIndex = 2;
            this.AudioCheckbox.Text = ".rel, .awc (Audio)";
            this.AudioCheckbox.UseVisualStyleBackColor = true;
            // 
            // LangCheckbox
            // 
            this.LangCheckbox.AutoSize = true;
            this.LangCheckbox.Checked = true;
            this.LangCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.LangCheckbox.Location = new System.Drawing.Point(220, 73);
            this.LangCheckbox.Name = "LangCheckbox";
            this.LangCheckbox.Size = new System.Drawing.Size(141, 17);
            this.LangCheckbox.TabIndex = 3;
            this.LangCheckbox.Text = ".gxt2 (Language files)";
            this.LangCheckbox.UseVisualStyleBackColor = true;
            // 
            // TexturesCheckbox
            // 
            this.TexturesCheckbox.AutoSize = true;
            this.TexturesCheckbox.Checked = true;
            this.TexturesCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TexturesCheckbox.Location = new System.Drawing.Point(15, 96);
            this.TexturesCheckbox.Name = "TexturesCheckbox";
            this.TexturesCheckbox.Size = new System.Drawing.Size(151, 17);
            this.TexturesCheckbox.TabIndex = 4;
            this.TexturesCheckbox.Text = ".ytd (Texture files)";
            this.TexturesCheckbox.UseVisualStyleBackColor = true;
            // 
            // ModelsCheckbox
            // 
            this.ModelsCheckbox.AutoSize = true;
            this.ModelsCheckbox.Checked = true;
            this.ModelsCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ModelsCheckbox.Location = new System.Drawing.Point(220, 96);
            this.ModelsCheckbox.Name = "ModelsCheckbox";
            this.ModelsCheckbox.Size = new System.Drawing.Size(189, 17);
            this.ModelsCheckbox.TabIndex = 5;
            this.ModelsCheckbox.Text = ".ydr, .yft, .ydd (Models)";
            this.ModelsCheckbox.UseVisualStyleBackColor = true;
            // 
            // YmapsCheckbox
            // 
            this.YmapsCheckbox.AutoSize = true;
            this.YmapsCheckbox.Location = new System.Drawing.Point(15, 119);
            this.YmapsCheckbox.Name = "YmapsCheckbox";
            this.YmapsCheckbox.Size = new System.Drawing.Size(131, 17);
            this.YmapsCheckbox.TabIndex = 6;
            this.YmapsCheckbox.Text = ".ymap (Map files)";
            this.YmapsCheckbox.UseVisualStyleBackColor = true;
            // 
            // RPFOptionsGroupBox
            // 
            this.RPFOptionsGroupBox.Controls.Add(this.ConvertRPFCheckbox);
            this.RPFOptionsGroupBox.Location = new System.Drawing.Point(16, 241);
            this.RPFOptionsGroupBox.Name = "RPFOptionsGroupBox";
            this.RPFOptionsGroupBox.Size = new System.Drawing.Size(571, 45);
            this.RPFOptionsGroupBox.TabIndex = 14;
            this.RPFOptionsGroupBox.TabStop = false;
            this.RPFOptionsGroupBox.Text = "RPF Options";
            // 
            // ConvertRPFCheckbox
            // 
            this.ConvertRPFCheckbox.AutoSize = true;
            this.ConvertRPFCheckbox.Location = new System.Drawing.Point(15, 19);
            this.ConvertRPFCheckbox.Name = "ConvertRPFCheckbox";
            this.ConvertRPFCheckbox.Size = new System.Drawing.Size(268, 17);
            this.ConvertRPFCheckbox.TabIndex = 0;
            this.ConvertRPFCheckbox.Text = "Convert RPF files to OPEN encryption (defragment)";
            this.ConvertRPFCheckbox.UseVisualStyleBackColor = true;
            // 
            // PreserveFolderStructureCheckbox
            // 
            this.PreserveFolderStructureCheckbox = new System.Windows.Forms.CheckBox();
            this.RPFOptionsGroupBox.Controls.Add(this.PreserveFolderStructureCheckbox);
            this.PreserveFolderStructureCheckbox.AutoSize = true;
            this.PreserveFolderStructureCheckbox.Location = new System.Drawing.Point(290, 19);
            this.PreserveFolderStructureCheckbox.Name = "PreserveFolderStructureCheckbox";
            this.PreserveFolderStructureCheckbox.Size = new System.Drawing.Size(165, 17);
            this.PreserveFolderStructureCheckbox.TabIndex = 1;
            this.PreserveFolderStructureCheckbox.Text = "Preserve folder structure";
            this.PreserveFolderStructureCheckbox.UseVisualStyleBackColor = true;
            // 
            // ExtractButton
            // 
            this.ExtractButton.Location = new System.Drawing.Point(92, 300);
            this.ExtractButton.Name = "ExtractButton";
            this.ExtractButton.Size = new System.Drawing.Size(75, 23);
            this.ExtractButton.TabIndex = 9;
            this.ExtractButton.Text = "Extract";
            this.ExtractButton.UseVisualStyleBackColor = true;
            this.ExtractButton.Click += new System.EventHandler(this.ExtractButton_Click);
            // 
            // LogTextBox
            // 
            this.LogTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LogTextBox.Location = new System.Drawing.Point(16, 340);
            this.LogTextBox.Multiline = true;
            this.LogTextBox.Name = "LogTextBox";
            this.LogTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LogTextBox.Size = new System.Drawing.Size(571, 150);
            this.LogTextBox.TabIndex = 10;
            // 
            // ExtractProgressBar
            // 
            this.ExtractProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExtractProgressBar.Location = new System.Drawing.Point(182, 300);
            this.ExtractProgressBar.Maximum = 1000;
            this.ExtractProgressBar.Name = "ExtractProgressBar";
            this.ExtractProgressBar.Size = new System.Drawing.Size(359, 23);
            this.ExtractProgressBar.TabIndex = 11;
            this.ExtractProgressBar.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 13);
            this.label4.TabIndex = 13;
            // 
            // ExtractForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 510);
            this.Controls.Add(this.RPFOptionsGroupBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.FileTypesGroupBox);
            this.Controls.Add(this.ExtractProgressBar);
            this.Controls.Add(this.LogTextBox);
            this.Controls.Add(this.ExtractButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.OutputFolderBrowseButton);
            this.Controls.Add(this.OutputFolderTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.InputFolderBrowseButton);
            this.Controls.Add(this.InputFolderTextBox);
            this.Controls.Add(this.label1);
            this.Name = "ExtractForm";
            this.Text = "RPF Extractor - CodeWalker by dexyfex";
            this.FileTypesGroupBox.ResumeLayout(false);
            this.FileTypesGroupBox.PerformLayout();
            this.RPFOptionsGroupBox.ResumeLayout(false);
            this.RPFOptionsGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox InputFolderTextBox;
        private System.Windows.Forms.Button InputFolderBrowseButton;
        private System.Windows.Forms.Button OutputFolderBrowseButton;
        private System.Windows.Forms.TextBox OutputFolderTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox MetaCheckbox;
        private System.Windows.Forms.CheckBox CarcolsCheckbox;
        private System.Windows.Forms.CheckBox AudioCheckbox;
        private System.Windows.Forms.CheckBox LangCheckbox;
        private System.Windows.Forms.CheckBox TexturesCheckbox;
        private System.Windows.Forms.CheckBox ModelsCheckbox;
        private System.Windows.Forms.CheckBox YmapsCheckbox;
        private System.Windows.Forms.Button ExtractButton;
        private System.Windows.Forms.TextBox LogTextBox;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog;
        private System.Windows.Forms.ProgressBar ExtractProgressBar;
        private System.Windows.Forms.CheckBox SelectAllCheckbox;
        private System.Windows.Forms.GroupBox FileTypesGroupBox;
        private System.Windows.Forms.GroupBox RPFOptionsGroupBox;
        private System.Windows.Forms.CheckBox ConvertRPFCheckbox;
        private System.Windows.Forms.CheckBox PreserveFolderStructureCheckbox;
        private System.Windows.Forms.Label label4;
    }
}

namespace CodeWalker.Project.Panels
{
    partial class EditGlobalTimecyclePanel
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
            this.HeaderLabel = new System.Windows.Forms.Label();
            this.ButtonPanel = new System.Windows.Forms.Panel();
            this.FileComboBox = new System.Windows.Forms.ComboBox();
            this.FileComboLabel = new System.Windows.Forms.Label();
            this.LoadButton = new System.Windows.Forms.Button();
            this.ReloadButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.ExportXmlButton = new System.Windows.Forms.Button();
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.TreeTabPage = new System.Windows.Forms.TabPage();
            this.MainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.XmlTreeView = new System.Windows.Forms.TreeView();
            this.PropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.RawTabPage = new System.Windows.Forms.TabPage();
            this.RawXmlTextBox = new System.Windows.Forms.TextBox();
            this.ButtonPanel.SuspendLayout();
            this.MainTabControl.SuspendLayout();
            this.TreeTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).BeginInit();
            this.MainSplitContainer.Panel1.SuspendLayout();
            this.MainSplitContainer.Panel2.SuspendLayout();
            this.MainSplitContainer.SuspendLayout();
            this.RawTabPage.SuspendLayout();
            this.SuspendLayout();
            //
            // HeaderLabel
            //
            this.HeaderLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.HeaderLabel.Location = new System.Drawing.Point(0, 0);
            this.HeaderLabel.Name = "HeaderLabel";
            this.HeaderLabel.Padding = new System.Windows.Forms.Padding(8, 8, 8, 4);
            this.HeaderLabel.Size = new System.Drawing.Size(900, 28);
            this.HeaderLabel.TabIndex = 0;
            this.HeaderLabel.Text = "(no file loaded)";
            //
            // ButtonPanel
            //
            this.ButtonPanel.Controls.Add(this.FileComboLabel);
            this.ButtonPanel.Controls.Add(this.FileComboBox);
            this.ButtonPanel.Controls.Add(this.LoadButton);
            this.ButtonPanel.Controls.Add(this.ReloadButton);
            this.ButtonPanel.Controls.Add(this.SaveButton);
            this.ButtonPanel.Controls.Add(this.ExportXmlButton);
            this.ButtonPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ButtonPanel.Location = new System.Drawing.Point(0, 28);
            this.ButtonPanel.Name = "ButtonPanel";
            this.ButtonPanel.Padding = new System.Windows.Forms.Padding(8, 2, 8, 6);
            this.ButtonPanel.Size = new System.Drawing.Size(900, 36);
            this.ButtonPanel.TabIndex = 1;
            //
            // FileComboLabel
            //
            this.FileComboLabel.AutoSize = true;
            this.FileComboLabel.Location = new System.Drawing.Point(10, 9);
            this.FileComboLabel.Name = "FileComboLabel";
            this.FileComboLabel.Size = new System.Drawing.Size(28, 13);
            this.FileComboLabel.TabIndex = 0;
            this.FileComboLabel.Text = "File:";
            //
            // FileComboBox
            //
            this.FileComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FileComboBox.FormattingEnabled = true;
            this.FileComboBox.Items.AddRange(new object[] {
            "common.rpf\\data\\levels\\gta5\\time.xml",
            "common.rpf\\data\\levels\\gta5\\weather.xml",
            "update\\update.rpf\\common\\data\\levels\\gta5\\weather.xml"});
            this.FileComboBox.Location = new System.Drawing.Point(44, 5);
            this.FileComboBox.Name = "FileComboBox";
            this.FileComboBox.Size = new System.Drawing.Size(340, 21);
            this.FileComboBox.TabIndex = 1;
            //
            // LoadButton
            //
            this.LoadButton.Location = new System.Drawing.Point(390, 4);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(75, 23);
            this.LoadButton.TabIndex = 2;
            this.LoadButton.Text = "Load";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            //
            // ReloadButton
            //
            this.ReloadButton.Location = new System.Drawing.Point(470, 4);
            this.ReloadButton.Name = "ReloadButton";
            this.ReloadButton.Size = new System.Drawing.Size(75, 23);
            this.ReloadButton.TabIndex = 3;
            this.ReloadButton.Text = "Reload";
            this.ReloadButton.UseVisualStyleBackColor = true;
            this.ReloadButton.Click += new System.EventHandler(this.ReloadButton_Click);
            //
            // SaveButton
            //
            this.SaveButton.Location = new System.Drawing.Point(550, 4);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(100, 23);
            this.SaveButton.TabIndex = 4;
            this.SaveButton.Text = "Save to Project";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            //
            // ExportXmlButton
            //
            this.ExportXmlButton.Location = new System.Drawing.Point(655, 4);
            this.ExportXmlButton.Name = "ExportXmlButton";
            this.ExportXmlButton.Size = new System.Drawing.Size(100, 23);
            this.ExportXmlButton.TabIndex = 5;
            this.ExportXmlButton.Text = "Export XML...";
            this.ExportXmlButton.UseVisualStyleBackColor = true;
            this.ExportXmlButton.Click += new System.EventHandler(this.ExportXmlButton_Click);
            //
            // MainTabControl
            //
            this.MainTabControl.Controls.Add(this.TreeTabPage);
            this.MainTabControl.Controls.Add(this.RawTabPage);
            this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTabControl.Location = new System.Drawing.Point(0, 64);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(900, 486);
            this.MainTabControl.TabIndex = 2;
            this.MainTabControl.SelectedIndexChanged += new System.EventHandler(this.MainTabControl_SelectedIndexChanged);
            //
            // TreeTabPage
            //
            this.TreeTabPage.Controls.Add(this.MainSplitContainer);
            this.TreeTabPage.Location = new System.Drawing.Point(4, 22);
            this.TreeTabPage.Name = "TreeTabPage";
            this.TreeTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.TreeTabPage.Size = new System.Drawing.Size(892, 460);
            this.TreeTabPage.TabIndex = 0;
            this.TreeTabPage.Text = "Tree / Properties";
            this.TreeTabPage.UseVisualStyleBackColor = true;
            //
            // MainSplitContainer
            //
            this.MainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainSplitContainer.Location = new System.Drawing.Point(3, 3);
            this.MainSplitContainer.Name = "MainSplitContainer";
            //
            // MainSplitContainer.Panel1
            //
            this.MainSplitContainer.Panel1.Controls.Add(this.XmlTreeView);
            //
            // MainSplitContainer.Panel2
            //
            this.MainSplitContainer.Panel2.Controls.Add(this.PropertyGrid);
            this.MainSplitContainer.Size = new System.Drawing.Size(886, 454);
            this.MainSplitContainer.SplitterDistance = 320;
            this.MainSplitContainer.TabIndex = 0;
            //
            // XmlTreeView
            //
            this.XmlTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.XmlTreeView.HideSelection = false;
            this.XmlTreeView.Location = new System.Drawing.Point(0, 0);
            this.XmlTreeView.Name = "XmlTreeView";
            this.XmlTreeView.Size = new System.Drawing.Size(320, 454);
            this.XmlTreeView.TabIndex = 0;
            this.XmlTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.XmlTreeView_AfterSelect);
            //
            // PropertyGrid
            //
            this.PropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.PropertyGrid.Name = "PropertyGrid";
            this.PropertyGrid.Size = new System.Drawing.Size(562, 454);
            this.PropertyGrid.TabIndex = 0;
            this.PropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.PropertyGrid_PropertyValueChanged);
            //
            // RawTabPage
            //
            this.RawTabPage.Controls.Add(this.RawXmlTextBox);
            this.RawTabPage.Location = new System.Drawing.Point(4, 22);
            this.RawTabPage.Name = "RawTabPage";
            this.RawTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.RawTabPage.Size = new System.Drawing.Size(892, 460);
            this.RawTabPage.TabIndex = 1;
            this.RawTabPage.Text = "Raw XML";
            this.RawTabPage.UseVisualStyleBackColor = true;
            //
            // RawXmlTextBox
            //
            this.RawXmlTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RawXmlTextBox.Font = new System.Drawing.Font("Consolas", 9F);
            this.RawXmlTextBox.Location = new System.Drawing.Point(3, 3);
            this.RawXmlTextBox.Multiline = true;
            this.RawXmlTextBox.Name = "RawXmlTextBox";
            this.RawXmlTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.RawXmlTextBox.Size = new System.Drawing.Size(886, 454);
            this.RawXmlTextBox.TabIndex = 0;
            this.RawXmlTextBox.WordWrap = false;
            //
            // EditGlobalTimecyclePanel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 550);
            this.Controls.Add(this.MainTabControl);
            this.Controls.Add(this.ButtonPanel);
            this.Controls.Add(this.HeaderLabel);
            this.Name = "EditGlobalTimecyclePanel";
            this.Text = "Global Timecycle / Weather";
            this.ButtonPanel.ResumeLayout(false);
            this.ButtonPanel.PerformLayout();
            this.MainTabControl.ResumeLayout(false);
            this.TreeTabPage.ResumeLayout(false);
            this.MainSplitContainer.Panel1.ResumeLayout(false);
            this.MainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).EndInit();
            this.MainSplitContainer.ResumeLayout(false);
            this.RawTabPage.ResumeLayout(false);
            this.RawTabPage.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.Panel ButtonPanel;
        private System.Windows.Forms.ComboBox FileComboBox;
        private System.Windows.Forms.Label FileComboLabel;
        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.Button ReloadButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button ExportXmlButton;
        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage TreeTabPage;
        private System.Windows.Forms.SplitContainer MainSplitContainer;
        private System.Windows.Forms.TreeView XmlTreeView;
        private System.Windows.Forms.PropertyGrid PropertyGrid;
        private System.Windows.Forms.TabPage RawTabPage;
        private System.Windows.Forms.TextBox RawXmlTextBox;
    }
}

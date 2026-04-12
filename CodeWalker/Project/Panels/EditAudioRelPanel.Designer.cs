namespace CodeWalker.Project.Panels
{
    partial class EditAudioRelPanel
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
            this.MainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.RelTreeView = new System.Windows.Forms.TreeView();
            this.PropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.HeaderLabel = new System.Windows.Forms.Label();
            this.ButtonPanel = new System.Windows.Forms.Panel();
            this.SaveChangesButton = new System.Windows.Forms.Button();
            this.ExportXmlButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).BeginInit();
            this.MainSplitContainer.Panel1.SuspendLayout();
            this.MainSplitContainer.Panel2.SuspendLayout();
            this.MainSplitContainer.SuspendLayout();
            this.ButtonPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // HeaderLabel
            //
            this.HeaderLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.HeaderLabel.Location = new System.Drawing.Point(0, 0);
            this.HeaderLabel.Name = "HeaderLabel";
            this.HeaderLabel.Padding = new System.Windows.Forms.Padding(8, 8, 8, 4);
            this.HeaderLabel.Size = new System.Drawing.Size(800, 28);
            this.HeaderLabel.TabIndex = 0;
            this.HeaderLabel.Text = "(no file loaded)";
            //
            // ButtonPanel
            //
            this.ButtonPanel.Controls.Add(this.SaveChangesButton);
            this.ButtonPanel.Controls.Add(this.ExportXmlButton);
            this.ButtonPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ButtonPanel.Location = new System.Drawing.Point(0, 28);
            this.ButtonPanel.Name = "ButtonPanel";
            this.ButtonPanel.Padding = new System.Windows.Forms.Padding(8, 2, 8, 6);
            this.ButtonPanel.Size = new System.Drawing.Size(800, 36);
            this.ButtonPanel.TabIndex = 1;
            //
            // ExportXmlButton
            //
            this.ExportXmlButton.Location = new System.Drawing.Point(8, 4);
            this.ExportXmlButton.Name = "ExportXmlButton";
            this.ExportXmlButton.Size = new System.Drawing.Size(100, 23);
            this.ExportXmlButton.TabIndex = 0;
            this.ExportXmlButton.Text = "Export XML...";
            this.ExportXmlButton.UseVisualStyleBackColor = true;
            this.ExportXmlButton.Click += new System.EventHandler(this.ExportXmlButton_Click);
            //
            // SaveChangesButton
            //
            this.SaveChangesButton.Location = new System.Drawing.Point(114, 4);
            this.SaveChangesButton.Name = "SaveChangesButton";
            this.SaveChangesButton.Size = new System.Drawing.Size(100, 23);
            this.SaveChangesButton.TabIndex = 1;
            this.SaveChangesButton.Text = "Save changes";
            this.SaveChangesButton.UseVisualStyleBackColor = true;
            this.SaveChangesButton.Click += new System.EventHandler(this.SaveChangesButton_Click);
            //
            // MainSplitContainer
            //
            this.MainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainSplitContainer.Location = new System.Drawing.Point(0, 64);
            this.MainSplitContainer.Name = "MainSplitContainer";
            //
            // MainSplitContainer.Panel1
            //
            this.MainSplitContainer.Panel1.Controls.Add(this.RelTreeView);
            //
            // MainSplitContainer.Panel2
            //
            this.MainSplitContainer.Panel2.Controls.Add(this.PropertyGrid);
            this.MainSplitContainer.Size = new System.Drawing.Size(800, 486);
            this.MainSplitContainer.SplitterDistance = 280;
            this.MainSplitContainer.TabIndex = 2;
            //
            // RelTreeView
            //
            this.RelTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RelTreeView.HideSelection = false;
            this.RelTreeView.Location = new System.Drawing.Point(0, 0);
            this.RelTreeView.Name = "RelTreeView";
            this.RelTreeView.Size = new System.Drawing.Size(280, 486);
            this.RelTreeView.TabIndex = 0;
            this.RelTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.RelTreeView_AfterSelect);
            //
            // PropertyGrid
            //
            this.PropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.PropertyGrid.Name = "PropertyGrid";
            this.PropertyGrid.Size = new System.Drawing.Size(516, 486);
            this.PropertyGrid.TabIndex = 0;
            this.PropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.PropertyGrid_PropertyValueChanged);
            //
            // EditAudioRelPanel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 550);
            this.Controls.Add(this.MainSplitContainer);
            this.Controls.Add(this.ButtonPanel);
            this.Controls.Add(this.HeaderLabel);
            this.Name = "EditAudioRelPanel";
            this.Text = "Edit Audio REL";
            this.MainSplitContainer.Panel1.ResumeLayout(false);
            this.MainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).EndInit();
            this.MainSplitContainer.ResumeLayout(false);
            this.ButtonPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.SplitContainer MainSplitContainer;
        private System.Windows.Forms.TreeView RelTreeView;
        private System.Windows.Forms.PropertyGrid PropertyGrid;
        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.Panel ButtonPanel;
        private System.Windows.Forms.Button SaveChangesButton;
        private System.Windows.Forms.Button ExportXmlButton;
    }
}

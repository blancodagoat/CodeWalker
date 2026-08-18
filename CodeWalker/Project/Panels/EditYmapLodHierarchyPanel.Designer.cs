namespace CodeWalker.Project.Panels
{
    partial class EditYmapLodHierarchyPanel
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
            this.GraphView = new CodeWalker.Project.Panels.LodHierarchyGraphControl();
            this.DetailsTextBox = new System.Windows.Forms.TextBox();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.FitViewButton = new System.Windows.Forms.Button();
            this.GotoButton = new System.Windows.Forms.Button();
            this.UnlinkParentButton = new System.Windows.Forms.Button();
            this.MarkParentButton = new System.Windows.Forms.Button();
            this.MarkedParentLabel = new System.Windows.Forms.Label();
            this.SetParentButton = new System.Windows.Forms.Button();
            this.DeleteEntityButton = new System.Windows.Forms.Button();
            this.DeleteChildrenCheckBox = new System.Windows.Forms.CheckBox();
            this.RecalcButton = new System.Windows.Forms.Button();
            this.HelpLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).BeginInit();
            this.MainSplitContainer.Panel1.SuspendLayout();
            this.MainSplitContainer.Panel2.SuspendLayout();
            this.MainSplitContainer.SuspendLayout();
            this.SuspendLayout();
            //
            // MainSplitContainer
            //
            this.MainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.MainSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.MainSplitContainer.Name = "MainSplitContainer";
            //
            // MainSplitContainer.Panel1
            //
            this.MainSplitContainer.Panel1.Controls.Add(this.GraphView);
            //
            // MainSplitContainer.Panel2
            //
            this.MainSplitContainer.Panel2.Controls.Add(this.DetailsTextBox);
            this.MainSplitContainer.Panel2.Controls.Add(this.RefreshButton);
            this.MainSplitContainer.Panel2.Controls.Add(this.FitViewButton);
            this.MainSplitContainer.Panel2.Controls.Add(this.GotoButton);
            this.MainSplitContainer.Panel2.Controls.Add(this.UnlinkParentButton);
            this.MainSplitContainer.Panel2.Controls.Add(this.MarkParentButton);
            this.MainSplitContainer.Panel2.Controls.Add(this.MarkedParentLabel);
            this.MainSplitContainer.Panel2.Controls.Add(this.SetParentButton);
            this.MainSplitContainer.Panel2.Controls.Add(this.DeleteEntityButton);
            this.MainSplitContainer.Panel2.Controls.Add(this.DeleteChildrenCheckBox);
            this.MainSplitContainer.Panel2.Controls.Add(this.RecalcButton);
            this.MainSplitContainer.Panel2.Controls.Add(this.HelpLabel);
            this.MainSplitContainer.Size = new System.Drawing.Size(884, 581);
            this.MainSplitContainer.SplitterDistance = 590;
            this.MainSplitContainer.TabIndex = 0;
            //
            // GraphView
            //
            this.GraphView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GraphView.Location = new System.Drawing.Point(0, 0);
            this.GraphView.Name = "GraphView";
            this.GraphView.Size = new System.Drawing.Size(590, 581);
            this.GraphView.TabIndex = 0;
            this.GraphView.SelectionChanged += new System.EventHandler(this.GraphView_SelectionChanged);
            this.GraphView.EntityActivated += new System.EventHandler(this.GraphView_EntityActivated);
            this.GraphView.LinkRequested += new CodeWalker.Project.Panels.LodHierarchyGraphControl.LinkEventHandler(this.GraphView_LinkRequested);
            //
            // DetailsTextBox
            //
            this.DetailsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DetailsTextBox.Location = new System.Drawing.Point(8, 8);
            this.DetailsTextBox.Multiline = true;
            this.DetailsTextBox.Name = "DetailsTextBox";
            this.DetailsTextBox.ReadOnly = true;
            this.DetailsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DetailsTextBox.Size = new System.Drawing.Size(274, 200);
            this.DetailsTextBox.TabIndex = 1;
            this.DetailsTextBox.Text = "(No entity selected)";
            //
            // RefreshButton
            //
            this.RefreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.RefreshButton.Location = new System.Drawing.Point(8, 216);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(86, 23);
            this.RefreshButton.TabIndex = 2;
            this.RefreshButton.Text = "Refresh";
            this.RefreshButton.UseVisualStyleBackColor = true;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            //
            // FitViewButton
            //
            this.FitViewButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.FitViewButton.Location = new System.Drawing.Point(102, 216);
            this.FitViewButton.Name = "FitViewButton";
            this.FitViewButton.Size = new System.Drawing.Size(86, 23);
            this.FitViewButton.TabIndex = 12;
            this.FitViewButton.Text = "Fit View";
            this.FitViewButton.UseVisualStyleBackColor = true;
            this.FitViewButton.Click += new System.EventHandler(this.FitViewButton_Click);
            //
            // GotoButton
            //
            this.GotoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.GotoButton.Location = new System.Drawing.Point(196, 216);
            this.GotoButton.Name = "GotoButton";
            this.GotoButton.Size = new System.Drawing.Size(86, 23);
            this.GotoButton.TabIndex = 3;
            this.GotoButton.Text = "Goto";
            this.GotoButton.UseVisualStyleBackColor = true;
            this.GotoButton.Click += new System.EventHandler(this.GotoButton_Click);
            //
            // UnlinkParentButton
            //
            this.UnlinkParentButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.UnlinkParentButton.Location = new System.Drawing.Point(8, 254);
            this.UnlinkParentButton.Name = "UnlinkParentButton";
            this.UnlinkParentButton.Size = new System.Drawing.Size(274, 23);
            this.UnlinkParentButton.TabIndex = 4;
            this.UnlinkParentButton.Text = "Unlink From Parent (Make Orphan)";
            this.UnlinkParentButton.UseVisualStyleBackColor = true;
            this.UnlinkParentButton.Click += new System.EventHandler(this.UnlinkParentButton_Click);
            //
            // MarkParentButton
            //
            this.MarkParentButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.MarkParentButton.Location = new System.Drawing.Point(8, 283);
            this.MarkParentButton.Name = "MarkParentButton";
            this.MarkParentButton.Size = new System.Drawing.Size(274, 23);
            this.MarkParentButton.TabIndex = 5;
            this.MarkParentButton.Text = "Mark Selected as New Parent";
            this.MarkParentButton.UseVisualStyleBackColor = true;
            this.MarkParentButton.Click += new System.EventHandler(this.MarkParentButton_Click);
            //
            // MarkedParentLabel
            //
            this.MarkedParentLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MarkedParentLabel.AutoEllipsis = true;
            this.MarkedParentLabel.Location = new System.Drawing.Point(8, 309);
            this.MarkedParentLabel.Name = "MarkedParentLabel";
            this.MarkedParentLabel.Size = new System.Drawing.Size(274, 17);
            this.MarkedParentLabel.TabIndex = 6;
            this.MarkedParentLabel.Text = "Marked parent: (none)";
            //
            // SetParentButton
            //
            this.SetParentButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.SetParentButton.Location = new System.Drawing.Point(8, 329);
            this.SetParentButton.Name = "SetParentButton";
            this.SetParentButton.Size = new System.Drawing.Size(274, 23);
            this.SetParentButton.TabIndex = 7;
            this.SetParentButton.Text = "Set Marked as Parent of Selected";
            this.SetParentButton.UseVisualStyleBackColor = true;
            this.SetParentButton.Click += new System.EventHandler(this.SetParentButton_Click);
            //
            // DeleteEntityButton
            //
            this.DeleteEntityButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.DeleteEntityButton.Location = new System.Drawing.Point(8, 369);
            this.DeleteEntityButton.Name = "DeleteEntityButton";
            this.DeleteEntityButton.Size = new System.Drawing.Size(274, 23);
            this.DeleteEntityButton.TabIndex = 8;
            this.DeleteEntityButton.Text = "Delete Entity";
            this.DeleteEntityButton.UseVisualStyleBackColor = true;
            this.DeleteEntityButton.Click += new System.EventHandler(this.DeleteEntityButton_Click);
            //
            // DeleteChildrenCheckBox
            //
            this.DeleteChildrenCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.DeleteChildrenCheckBox.AutoSize = true;
            this.DeleteChildrenCheckBox.Location = new System.Drawing.Point(8, 398);
            this.DeleteChildrenCheckBox.Name = "DeleteChildrenCheckBox";
            this.DeleteChildrenCheckBox.Size = new System.Drawing.Size(180, 19);
            this.DeleteChildrenCheckBox.TabIndex = 9;
            this.DeleteChildrenCheckBox.Text = "Also delete all LOD children";
            this.DeleteChildrenCheckBox.UseVisualStyleBackColor = true;
            //
            // RecalcButton
            //
            this.RecalcButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.RecalcButton.Location = new System.Drawing.Point(8, 432);
            this.RecalcButton.Name = "RecalcButton";
            this.RecalcButton.Size = new System.Drawing.Size(274, 23);
            this.RecalcButton.TabIndex = 10;
            this.RecalcButton.Text = "Recalc numChildren && childLodDist (all)";
            this.RecalcButton.UseVisualStyleBackColor = true;
            this.RecalcButton.Click += new System.EventHandler(this.RecalcButton_Click);
            //
            // HelpLabel
            //
            this.HelpLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HelpLabel.Location = new System.Drawing.Point(8, 466);
            this.HelpLabel.Name = "HelpLabel";
            this.HelpLabel.Size = new System.Drawing.Size(274, 107);
            this.HelpLabel.TabIndex = 11;
            this.HelpLabel.Text = "Click selects (Ctrl+click toggles), left-drag empty space box-selects, double-cl" +
    "ick opens the entity editor. Wheel zooms, right-drag pans, drag nodes to move th" +
    "em.\r\n\r\nDrag from a node\'s sockets to link: top socket onto a parent connects, " +
    "bottom socket onto a child adopts it, drop a top-socket drag in empty space to " +
    "unlink.\r\n\r\nOrange borders mark numChildren / parentIndex mismatches - use Reca" +
    "lc to fix counts. Dashed links are cross-ymap LOD chains.";
            //
            // EditYmapLodHierarchyPanel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 581);
            this.Controls.Add(this.MainSplitContainer);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)
            | WeifenLuo.WinFormsUI.Docking.DockAreas.Document));
            this.Name = "EditYmapLodHierarchyPanel";
            this.Text = "LOD Hierarchy";
            this.MainSplitContainer.Panel1.ResumeLayout(false);
            this.MainSplitContainer.Panel2.ResumeLayout(false);
            this.MainSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).EndInit();
            this.MainSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.SplitContainer MainSplitContainer;
        private LodHierarchyGraphControl GraphView;
        private System.Windows.Forms.TextBox DetailsTextBox;
        private System.Windows.Forms.Button RefreshButton;
        private System.Windows.Forms.Button FitViewButton;
        private System.Windows.Forms.Button GotoButton;
        private System.Windows.Forms.Button UnlinkParentButton;
        private System.Windows.Forms.Button MarkParentButton;
        private System.Windows.Forms.Label MarkedParentLabel;
        private System.Windows.Forms.Button SetParentButton;
        private System.Windows.Forms.Button DeleteEntityButton;
        private System.Windows.Forms.CheckBox DeleteChildrenCheckBox;
        private System.Windows.Forms.Button RecalcButton;
        private System.Windows.Forms.Label HelpLabel;
    }
}

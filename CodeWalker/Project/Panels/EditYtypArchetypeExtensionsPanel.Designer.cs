namespace CodeWalker.Project.Panels
{
    partial class EditYtypArchetypeExtensionsPanel
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            this.HeaderLabel = new System.Windows.Forms.Label();
            this.ExtensionsListBox = new System.Windows.Forms.ListBox();
            this.ExtensionsPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.AddButton = new System.Windows.Forms.Button();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // HeaderLabel
            //
            this.HeaderLabel.AutoSize = true;
            this.HeaderLabel.Location = new System.Drawing.Point(12, 9);
            this.HeaderLabel.Name = "HeaderLabel";
            this.HeaderLabel.Size = new System.Drawing.Size(120, 13);
            this.HeaderLabel.TabIndex = 0;
            this.HeaderLabel.Text = "Archetype Extensions:";
            //
            // ExtensionsListBox
            //
            this.ExtensionsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ExtensionsListBox.FormattingEnabled = true;
            this.ExtensionsListBox.IntegralHeight = false;
            this.ExtensionsListBox.Location = new System.Drawing.Point(12, 28);
            this.ExtensionsListBox.Name = "ExtensionsListBox";
            this.ExtensionsListBox.Size = new System.Drawing.Size(220, 355);
            this.ExtensionsListBox.TabIndex = 1;
            this.ExtensionsListBox.SelectedIndexChanged += new System.EventHandler(this.ExtensionsListBox_SelectedIndexChanged);
            //
            // ExtensionsPropertyGrid
            //
            this.ExtensionsPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExtensionsPropertyGrid.Location = new System.Drawing.Point(238, 28);
            this.ExtensionsPropertyGrid.Name = "ExtensionsPropertyGrid";
            this.ExtensionsPropertyGrid.Size = new System.Drawing.Size(400, 384);
            this.ExtensionsPropertyGrid.TabIndex = 2;
            this.ExtensionsPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.ExtensionsPropertyGrid_PropertyValueChanged);
            //
            // AddButton
            //
            this.AddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddButton.Location = new System.Drawing.Point(12, 389);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(95, 23);
            this.AddButton.TabIndex = 3;
            this.AddButton.Text = "Add...";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            //
            // DeleteButton
            //
            this.DeleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeleteButton.Location = new System.Drawing.Point(137, 389);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(95, 23);
            this.DeleteButton.TabIndex = 4;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            //
            // EditYtypArchetypeExtensionsPanel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 424);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.ExtensionsPropertyGrid);
            this.Controls.Add(this.ExtensionsListBox);
            this.Controls.Add(this.HeaderLabel);
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "EditYtypArchetypeExtensionsPanel";
            this.Text = "Archetype Extensions";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.ListBox ExtensionsListBox;
        private System.Windows.Forms.PropertyGrid ExtensionsPropertyGrid;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button DeleteButton;
    }
}

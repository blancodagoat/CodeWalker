namespace CodeWalker.Forms
{
    partial class AssetBrowserForm
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FilterLabel = new System.Windows.Forms.Label();
            this.FilterTextBox = new System.Windows.Forms.TextBox();
            this.ArchetypeListBox = new System.Windows.Forms.ListBox();
            this.InfoTextBox = new System.Windows.Forms.TextBox();
            this.InfoLabel = new System.Windows.Forms.Label();
            this.OkBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // FilterLabel
            //
            this.FilterLabel.AutoSize = true;
            this.FilterLabel.Location = new System.Drawing.Point(12, 15);
            this.FilterLabel.Name = "FilterLabel";
            this.FilterLabel.Size = new System.Drawing.Size(32, 13);
            this.FilterLabel.TabIndex = 0;
            this.FilterLabel.Text = "Filter:";
            //
            // FilterTextBox
            //
            this.FilterTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FilterTextBox.Location = new System.Drawing.Point(50, 12);
            this.FilterTextBox.Name = "FilterTextBox";
            this.FilterTextBox.Size = new System.Drawing.Size(722, 20);
            this.FilterTextBox.TabIndex = 1;
            this.FilterTextBox.TextChanged += new System.EventHandler(this.FilterTextBox_TextChanged);
            //
            // ArchetypeListBox
            //
            this.ArchetypeListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left))));
            this.ArchetypeListBox.FormattingEnabled = true;
            this.ArchetypeListBox.IntegralHeight = false;
            this.ArchetypeListBox.Location = new System.Drawing.Point(12, 38);
            this.ArchetypeListBox.Name = "ArchetypeListBox";
            this.ArchetypeListBox.Size = new System.Drawing.Size(300, 452);
            this.ArchetypeListBox.TabIndex = 2;
            this.ArchetypeListBox.SelectedIndexChanged += new System.EventHandler(this.ArchetypeListBox_SelectedIndexChanged);
            this.ArchetypeListBox.DoubleClick += new System.EventHandler(this.ArchetypeListBox_DoubleClick);
            //
            // InfoLabel
            //
            this.InfoLabel.AutoSize = true;
            this.InfoLabel.Location = new System.Drawing.Point(318, 38);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(84, 13);
            this.InfoLabel.TabIndex = 3;
            this.InfoLabel.Text = "Archetype info:";
            //
            // InfoTextBox
            //
            this.InfoTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InfoTextBox.Font = new System.Drawing.Font("Consolas", 9F);
            this.InfoTextBox.Location = new System.Drawing.Point(318, 57);
            this.InfoTextBox.Multiline = true;
            this.InfoTextBox.Name = "InfoTextBox";
            this.InfoTextBox.ReadOnly = true;
            this.InfoTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.InfoTextBox.Size = new System.Drawing.Size(454, 433);
            this.InfoTextBox.TabIndex = 4;
            this.InfoTextBox.WordWrap = false;
            //
            // OkBtn
            //
            this.OkBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkBtn.Location = new System.Drawing.Point(616, 505);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 23);
            this.OkBtn.TabIndex = 5;
            this.OkBtn.Text = "OK";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            //
            // CancelBtn
            //
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(697, 505);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 6;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            //
            // StatusLabel
            //
            this.StatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusLabel.Location = new System.Drawing.Point(12, 510);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(598, 13);
            this.StatusLabel.TabIndex = 7;
            this.StatusLabel.Text = "";
            //
            // AssetBrowserForm
            //
            this.AcceptButton = this.OkBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(784, 540);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OkBtn);
            this.Controls.Add(this.InfoTextBox);
            this.Controls.Add(this.InfoLabel);
            this.Controls.Add(this.ArchetypeListBox);
            this.Controls.Add(this.FilterTextBox);
            this.Controls.Add(this.FilterLabel);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "AssetBrowserForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Asset Browser - CodeWalker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label FilterLabel;
        private System.Windows.Forms.TextBox FilterTextBox;
        private System.Windows.Forms.ListBox ArchetypeListBox;
        private System.Windows.Forms.TextBox InfoTextBox;
        private System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Label StatusLabel;
    }
}

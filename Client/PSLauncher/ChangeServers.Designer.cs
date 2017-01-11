namespace PSLauncher
{
    partial class ChangeServers
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
            this.Reset = new System.Windows.Forms.Button();
            this.ManifestDesc = new System.Windows.Forms.TextBox();
            this.ManifestFile = new System.Windows.Forms.TextBox();
            this.ManifestLabel = new System.Windows.Forms.Label();
            this.Cancel = new System.Windows.Forms.Button();
            this.Save = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Reset
            // 
            this.Reset.Location = new System.Drawing.Point(16, 96);
            this.Reset.Name = "Reset";
            this.Reset.Size = new System.Drawing.Size(75, 23);
            this.Reset.TabIndex = 11;
            this.Reset.Text = "Reset";
            this.Reset.UseVisualStyleBackColor = true;
            this.Reset.Click += new System.EventHandler(this.Reset_Click);
            // 
            // ManifestDesc
            // 
            this.ManifestDesc.BackColor = System.Drawing.SystemColors.Control;
            this.ManifestDesc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ManifestDesc.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ManifestDesc.Location = new System.Drawing.Point(16, 58);
            this.ManifestDesc.Multiline = true;
            this.ManifestDesc.Name = "ManifestDesc";
            this.ManifestDesc.Size = new System.Drawing.Size(276, 33);
            this.ManifestDesc.TabIndex = 10;
            this.ManifestDesc.Text = "The manifest file provides this launcher all of the information it needs to conne" +
    "ct you to the custom server.";
            // 
            // ManifestFile
            // 
            this.ManifestFile.Location = new System.Drawing.Point(16, 32);
            this.ManifestFile.Name = "ManifestFile";
            this.ManifestFile.Size = new System.Drawing.Size(276, 20);
            this.ManifestFile.TabIndex = 9;
            // 
            // ManifestLabel
            // 
            this.ManifestLabel.AutoSize = true;
            this.ManifestLabel.Location = new System.Drawing.Point(13, 12);
            this.ManifestLabel.Name = "ManifestLabel";
            this.ManifestLabel.Size = new System.Drawing.Size(66, 13);
            this.ManifestLabel.TabIndex = 8;
            this.ManifestLabel.Text = "Manifest File";
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(217, 97);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 7;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(136, 97);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(75, 23);
            this.Save.TabIndex = 6;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // ChangeServers
            // 
            this.AcceptButton = this.Save;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(304, 132);
            this.ControlBox = false;
            this.Controls.Add(this.Reset);
            this.Controls.Add(this.ManifestDesc);
            this.Controls.Add(this.ManifestFile);
            this.Controls.Add(this.ManifestLabel);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Save);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(320, 171);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(320, 171);
            this.Name = "ChangeServers";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Choose Server";
            this.Load += new System.EventHandler(this.ChangeServers_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button Reset;
        internal System.Windows.Forms.TextBox ManifestDesc;
        internal System.Windows.Forms.TextBox ManifestFile;
        internal System.Windows.Forms.Label ManifestLabel;
        internal System.Windows.Forms.Button Cancel;
        internal System.Windows.Forms.Button Save;
    }
}
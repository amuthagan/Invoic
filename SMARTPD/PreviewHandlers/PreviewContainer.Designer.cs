namespace PreviewHandlers
{
    partial class PreviewContainer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.previewHandlerHost = new DefaultPreviewHandlerHost();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(150, 150);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox.TabIndex = 1;
            this.pictureBox.TabStop = false;
            // 
            // previewHandlerHost
            // 
            this.previewHandlerHost.BackColor = System.Drawing.Color.White;
            this.previewHandlerHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewHandlerHost.Location = new System.Drawing.Point(0, 0);
            this.previewHandlerHost.Name = "previewHandlerHost";
            this.previewHandlerHost.Size = new System.Drawing.Size(150, 150);
            this.previewHandlerHost.TabIndex = 2;
            // 
            // OLEContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.previewHandlerHost);
            this.Controls.Add(this.pictureBox);
            this.Name = "OLEContainer";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion


        private System.Windows.Forms.PictureBox pictureBox;
        private DefaultPreviewHandlerHost previewHandlerHost;
    }
}

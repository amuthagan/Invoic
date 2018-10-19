namespace ProcessDesigner.UICommon
{
    partial class AutoCadUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoCadUserControl));
            this.viewautocad = new AxACCTRLLib.AxAcCtrl();
            ((System.ComponentModel.ISupportInitialize)(this.viewautocad)).BeginInit();
            this.SuspendLayout();
            // 
            // ViewAutoCad
            // 
            this.viewautocad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewautocad.Enabled = true;
            this.viewautocad.Location = new System.Drawing.Point(0, 0);
            this.viewautocad.Name = "ViewAutoCad";
            this.viewautocad.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("ViewAutoCad.OcxState")));
            this.viewautocad.Size = new System.Drawing.Size(150, 150);
            this.viewautocad.TabIndex = 0;
            // 
            // AutoCadUserControl
            // 
            this.Controls.Add(this.viewautocad);
            this.Name = "AutoCadUserControl";
            ((System.ComponentModel.ISupportInitialize)(this.viewautocad)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxACCTRLLib.AxAcCtrl viewautocad;


    }
}

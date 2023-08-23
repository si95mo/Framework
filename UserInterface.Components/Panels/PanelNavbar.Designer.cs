namespace UserInterface.Controls.Panels
{
    partial class PanelNavbar
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
            this.LayoutPanel = new System.Windows.Forms.Panel();
            this.PbxClose = new System.Windows.Forms.PictureBox();
            this.PbxLogo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PbxClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // LayoutPanel
            // 
            this.LayoutPanel.Location = new System.Drawing.Point(-1, -1);
            this.LayoutPanel.Name = "LayoutPanel";
            this.LayoutPanel.Size = new System.Drawing.Size(1728, 62);
            this.LayoutPanel.TabIndex = 1;
            // 
            // PbxClose
            // 
            this.PbxClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PbxClose.Image = global::UserInterface.Controls.Properties.Resources.ImageShutdown;
            this.PbxClose.Location = new System.Drawing.Point(1860, 0);
            this.PbxClose.Name = "PbxClose";
            this.PbxClose.Size = new System.Drawing.Size(60, 60);
            this.PbxClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PbxClose.TabIndex = 3;
            this.PbxClose.TabStop = false;
            this.PbxClose.Click += new System.EventHandler(this.PbxClose_Click);
            this.PbxClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PbxClose_MouseDown);
            this.PbxClose.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PbxClose_MouseUp);
            // 
            // PbxLogo
            // 
            this.PbxLogo.Image = global::UserInterface.Controls.Properties.Resources.META_logo_ORIZZONTALE_colori;
            this.PbxLogo.Location = new System.Drawing.Point(1734, 0);
            this.PbxLogo.Name = "PbxLogo";
            this.PbxLogo.Size = new System.Drawing.Size(120, 60);
            this.PbxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PbxLogo.TabIndex = 2;
            this.PbxLogo.TabStop = false;
            // 
            // PanelNavbar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.Controls.Add(this.LayoutPanel);
            this.Controls.Add(this.PbxClose);
            this.Controls.Add(this.PbxLogo);
            this.MaximumSize = new System.Drawing.Size(1920, 60);
            this.MinimumSize = new System.Drawing.Size(1920, 60);
            this.Name = "PanelNavbar";
            this.Size = new System.Drawing.Size(1920, 60);
            ((System.ComponentModel.ISupportInitialize)(this.PbxClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbxLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        protected System.Windows.Forms.PictureBox PbxLogo;
        protected System.Windows.Forms.PictureBox PbxClose;
        protected System.Windows.Forms.Panel LayoutPanel;
    }
}

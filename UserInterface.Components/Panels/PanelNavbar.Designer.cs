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
            this.LblVersion = new UserInterface.Controls.LabelControl();
            this.PbxAction = new System.Windows.Forms.PictureBox();
            this.PbxClose = new System.Windows.Forms.PictureBox();
            this.PbxLogo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PbxAction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbxClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // LayoutPanel
            // 
            this.LayoutPanel.Location = new System.Drawing.Point(168, -2);
            this.LayoutPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.LayoutPanel.Name = "LayoutPanel";
            this.LayoutPanel.Size = new System.Drawing.Size(2328, 95);
            this.LayoutPanel.TabIndex = 1;
            // 
            // LblVersion
            // 
            this.LblVersion.Font = new System.Drawing.Font("Lucida Sans Unicode", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblVersion.Location = new System.Drawing.Point(0, 0);
            this.LblVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblVersion.Name = "LblVersion";
            this.LblVersion.Size = new System.Drawing.Size(160, 94);
            this.LblVersion.TabIndex = 4;
            this.LblVersion.Text = "v1.2.3";
            this.LblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PbxAction
            // 
            this.PbxAction.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PbxAction.Image = global::UserInterface.Controls.Properties.Resources.action1;
            this.PbxAction.Location = new System.Drawing.Point(2692, 0);
            this.PbxAction.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PbxAction.Name = "PbxAction";
            this.PbxAction.Size = new System.Drawing.Size(90, 92);
            this.PbxAction.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PbxAction.TabIndex = 3;
            this.PbxAction.TabStop = false;
            this.PbxAction.Click += new System.EventHandler(this.PbxAction_Click);
            this.PbxAction.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PbxAction_MouseDown);
            this.PbxAction.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PbxAction_MouseUp);
            // 
            // PbxClose
            // 
            this.PbxClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PbxClose.Image = global::UserInterface.Controls.Properties.Resources.ImageShutdown;
            this.PbxClose.Location = new System.Drawing.Point(2790, 0);
            this.PbxClose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PbxClose.Name = "PbxClose";
            this.PbxClose.Size = new System.Drawing.Size(90, 92);
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
            this.PbxLogo.Location = new System.Drawing.Point(2504, 0);
            this.PbxLogo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PbxLogo.Name = "PbxLogo";
            this.PbxLogo.Size = new System.Drawing.Size(180, 92);
            this.PbxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PbxLogo.TabIndex = 2;
            this.PbxLogo.TabStop = false;
            // 
            // PanelNavbar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.Controls.Add(this.LblVersion);
            this.Controls.Add(this.LayoutPanel);
            this.Controls.Add(this.PbxAction);
            this.Controls.Add(this.PbxClose);
            this.Controls.Add(this.PbxLogo);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(2880, 92);
            this.MinimumSize = new System.Drawing.Size(2880, 92);
            this.Name = "PanelNavbar";
            this.Size = new System.Drawing.Size(2880, 92);
            ((System.ComponentModel.ISupportInitialize)(this.PbxAction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbxClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbxLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        protected System.Windows.Forms.PictureBox PbxLogo;
        protected System.Windows.Forms.PictureBox PbxClose;
        protected System.Windows.Forms.Panel LayoutPanel;
        protected LabelControl LblVersion;
        protected System.Windows.Forms.PictureBox PbxAction;
    }
}

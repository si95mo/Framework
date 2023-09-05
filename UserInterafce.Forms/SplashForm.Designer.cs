namespace UserInterface.Forms
{
    partial class SplashForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.pbxClient = new System.Windows.Forms.PictureBox();
            this.pbxMeta = new System.Windows.Forms.PictureBox();
            this.prbProgress = new CircularProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.pbxClient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxMeta)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(220)))), ((int)(((byte)(4)))));
            this.panel1.Location = new System.Drawing.Point(-2, -1);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(503, 18);
            this.panel1.TabIndex = 14;
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(12, 476);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(476, 15);
            this.lblStatus.TabIndex = 13;
            this.lblStatus.Text = "label1";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbxClient
            // 
            this.pbxClient.Image = ((System.Drawing.Image)(resources.GetObject("pbxClient.Image")));
            this.pbxClient.Location = new System.Drawing.Point(308, 25);
            this.pbxClient.Name = "pbxClient";
            this.pbxClient.Size = new System.Drawing.Size(180, 180);
            this.pbxClient.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxClient.TabIndex = 12;
            this.pbxClient.TabStop = false;
            // 
            // pbxMeta
            // 
            this.pbxMeta.Image = ((System.Drawing.Image)(resources.GetObject("pbxMeta.Image")));
            this.pbxMeta.Location = new System.Drawing.Point(12, 25);
            this.pbxMeta.Name = "pbxMeta";
            this.pbxMeta.Size = new System.Drawing.Size(180, 180);
            this.pbxMeta.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxMeta.TabIndex = 11;
            this.pbxMeta.TabStop = false;
            // 
            // prbProgress
            // 
            this.prbProgress.BackColor = System.Drawing.Color.White;
            this.prbProgress.BarWidth = 14F;
            this.prbProgress.FirstBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(220)))), ((int)(((byte)(4)))));
            this.prbProgress.Font = new System.Drawing.Font("Lucida Sans Unicode", 14F);
            this.prbProgress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.prbProgress.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.prbProgress.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.prbProgress.LineWidth = 1;
            this.prbProgress.Location = new System.Drawing.Point(141, 216);
            this.prbProgress.Maximum = ((long)(100));
            this.prbProgress.MinimumSize = new System.Drawing.Size(117, 117);
            this.prbProgress.Name = "prbProgress";
            this.prbProgress.ProgressBarShape = CircularProgressBar.ProgressShape.Flat;
            this.prbProgress.ProgressBarTextMode = CircularProgressBar.TextMode.Percentage;
            this.prbProgress.SecondBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(220)))), ((int)(((byte)(4)))));
            this.prbProgress.Size = new System.Drawing.Size(220, 220);
            this.prbProgress.TabIndex = 10;
            this.prbProgress.Value = ((long)(0));
            // 
            // SplashForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(500, 500);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pbxClient);
            this.Controls.Add(this.pbxMeta);
            this.Controls.Add(this.prbProgress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SplashForm";
            this.Text = "SplashForm";
            this.Load += new System.EventHandler(this.SplashForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbxClient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxMeta)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.PictureBox pbxClient;
        private System.Windows.Forms.PictureBox pbxMeta;
        private CircularProgressBar prbProgress;
    }
}
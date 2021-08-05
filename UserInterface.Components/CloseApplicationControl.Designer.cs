
namespace UserInterface.Controls
{
    partial class CloseApplicationControl
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
            this.pbxCloseImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbxCloseImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pbxCloseImage
            // 
            this.pbxCloseImage.BackColor = System.Drawing.SystemColors.Info;
            this.pbxCloseImage.Image = global::UserInterface.Controls.Properties.Resources.ImageShutdown;
            this.pbxCloseImage.InitialImage = global::UserInterface.Controls.Properties.Resources.ImageShutdown;
            this.pbxCloseImage.Location = new System.Drawing.Point(0, 0);
            this.pbxCloseImage.Name = "pbxCloseImage";
            this.pbxCloseImage.Size = new System.Drawing.Size(100, 100);
            this.pbxCloseImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbxCloseImage.TabIndex = 0;
            this.pbxCloseImage.TabStop = false;
            this.pbxCloseImage.Click += new System.EventHandler(this.CloseImage_Click);
            // 
            // CloseApplicationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbxCloseImage);
            this.Name = "CloseApplicationControl";
            this.Size = new System.Drawing.Size(100, 100);
            ((System.ComponentModel.ISupportInitialize)(this.pbxCloseImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbxCloseImage;
    }
}

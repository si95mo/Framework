namespace Framework.Tests
{
    partial class MainForm
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
            this.SuspendLayout();
            // 
            // LblFormName
            // 
            this.LblFormName.Location = new System.Drawing.Point(14, 13);
            this.LblFormName.Size = new System.Drawing.Size(155, 25);
            this.LblFormName.Text = "CustomForm";
            // 
            // ControlBox
            // 
            this.ControlBox.Location = new System.Drawing.Point(2386, 10);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 1030);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Controls.SetChildIndex(this.LblFormName, 0);
            this.Controls.SetChildIndex(this.ControlBox, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}


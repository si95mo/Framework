
namespace UserInterface.Controls.Tests
{
    partial class TestForm
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
            this.progressBar = new CircularProgressBar();
            this.ledControl = new UserInterface.Controls.LedControl();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.BackColor = System.Drawing.SystemColors.Control;
            this.progressBar.BarColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(220)))), ((int)(((byte)(4)))));
            this.progressBar.BarColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(220)))), ((int)(((byte)(4)))));
            this.progressBar.BarWidth = 14F;
            this.progressBar.Font = new System.Drawing.Font("Lucida Sans Unicode", 14F);
            this.progressBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.progressBar.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.progressBar.LineColor = System.Drawing.Color.DimGray;
            this.progressBar.LineWidth = 1;
            this.progressBar.Location = new System.Drawing.Point(230, -3);
            this.progressBar.Maximum = ((long)(100));
            this.progressBar.MinimumSize = new System.Drawing.Size(100, 100);
            this.progressBar.Name = "progressBar";
            this.progressBar.ProgressBarShape = CircularProgressBar.ProgressShape.Flat;
            this.progressBar.ProgressBarTextMode = CircularProgressBar.TextMode.Percentage;
            this.progressBar.Size = new System.Drawing.Size(125, 125);
            this.progressBar.TabIndex = 1;
            this.progressBar.Value = ((long)(0));
            // 
            // ledControl
            // 
            this.ledControl.Location = new System.Drawing.Point(12, 12);
            this.ledControl.Name = "ledControl";
            this.ledControl.On = true;
            this.ledControl.Size = new System.Drawing.Size(96, 93);
            this.ledControl.TabIndex = 0;
            this.ledControl.Text = "ledControl";
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 134);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.ledControl);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.ResumeLayout(false);

        }

        #endregion

        private LedControl ledControl;
        private CircularProgressBar progressBar;
    }
}
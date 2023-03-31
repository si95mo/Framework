namespace UserInterface.Controls
{
    partial class SchedulerControl
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
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.chart = new LiveCharts.Wpf.CartesianChart();
            this.lblCode = new UserInterface.Controls.LabelControl();
            this.SuspendLayout();
            // 
            // elementHost1
            // 
            this.elementHost1.Location = new System.Drawing.Point(0, 0);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(520, 173);
            this.elementHost1.TabIndex = 0;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.chart;
            // 
            // lblCode
            // 
            this.lblCode.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblCode.Location = new System.Drawing.Point(0, 176);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(520, 23);
            this.lblCode.TabIndex = 1;
            this.lblCode.Text = "labelControl1";
            this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SchedulerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblCode);
            this.Controls.Add(this.elementHost1);
            this.Name = "SchedulerControl";
            this.Size = new System.Drawing.Size(520, 200);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private LiveCharts.Wpf.CartesianChart chart;
        private LabelControl lblCode;
    }
}

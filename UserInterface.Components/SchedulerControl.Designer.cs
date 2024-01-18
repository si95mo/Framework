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
            this.lblLoad = new UserInterface.Controls.LabelControl();
            this.lblCode = new UserInterface.Controls.PanelWithLed();
            this.SuspendLayout();
            // 
            // elementHost1
            // 
            this.elementHost1.BackColor = System.Drawing.Color.White;
            this.elementHost1.Location = new System.Drawing.Point(0, 0);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(520, 169);
            this.elementHost1.TabIndex = 0;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.chart;
            // 
            // lblLoad
            // 
            this.lblLoad.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblLoad.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoad.Location = new System.Drawing.Point(460, 170);
            this.lblLoad.Name = "lblLoad";
            this.lblLoad.Size = new System.Drawing.Size(60, 29);
            this.lblLoad.TabIndex = 2;
            this.lblLoad.Text = "100.00%";
            this.lblLoad.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCode
            // 
            this.lblCode.BackColor = System.Drawing.Color.White;
            this.lblCode.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblCode.Location = new System.Drawing.Point(0, 170);
            this.lblCode.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.lblCode.MinimumSize = new System.Drawing.Size(200, 30);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(460, 30);
            this.lblCode.TabIndex = 14;
            this.lblCode.Title = "Scheduler code";
            // 
            // SchedulerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblCode);
            this.Controls.Add(this.lblLoad);
            this.Controls.Add(this.elementHost1);
            this.Name = "SchedulerControl";
            this.Size = new System.Drawing.Size(520, 200);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private LiveCharts.Wpf.CartesianChart chart;
        private LabelControl lblLoad;
        private PanelWithLed lblCode;
    }
}

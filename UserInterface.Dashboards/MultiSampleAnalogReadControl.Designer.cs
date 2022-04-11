
namespace UserInterface.Dashboards
{
    partial class MultiSampleAnalogReadControl
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
            this.lblChannelName = new UserInterface.Controls.LabelControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chart = new UserInterface.Controls.ChartControl();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblChannelName
            // 
            this.lblChannelName.AutoSize = true;
            this.lblChannelName.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblChannelName.Location = new System.Drawing.Point(74, 22);
            this.lblChannelName.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblChannelName.Name = "lblChannelName";
            this.lblChannelName.Size = new System.Drawing.Size(123, 20);
            this.lblChannelName.TabIndex = 4;
            this.lblChannelName.Text = "Channel name";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.chart);
            this.panel1.Location = new System.Drawing.Point(5, 47);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(266, 54);
            this.panel1.TabIndex = 5;
            // 
            // chart
            // 
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart.Location = new System.Drawing.Point(0, 0);
            this.chart.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.chart.Name = "chart";
            this.chart.Size = new System.Drawing.Size(266, 54);
            this.chart.TabIndex = 0;
            // 
            // MultiSampleAnalogReadControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblChannelName);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.Name = "MultiSampleAnalogReadControl";
            this.Size = new System.Drawing.Size(276, 116);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.LabelControl lblChannelName;
        private System.Windows.Forms.Panel panel1;
        private Controls.ChartControl chart;
    }
}


namespace UserInterface.Controls.Tests
{
    partial class ChartControlForm
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
            this.chrNumeric = new UserInterface.Controls.ChartControl();
            this.chrWaveform = new UserInterface.Controls.ChartControl();
            this.bgWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // chrNumeric
            // 
            this.chrNumeric.Location = new System.Drawing.Point(12, 2);
            this.chrNumeric.Name = "chrNumeric";
            this.chrNumeric.Size = new System.Drawing.Size(776, 245);
            this.chrNumeric.TabIndex = 0;
            // 
            // chrWaveform
            // 
            this.chrWaveform.Location = new System.Drawing.Point(12, 261);
            this.chrWaveform.Name = "chrWaveform";
            this.chrWaveform.Size = new System.Drawing.Size(776, 245);
            this.chrWaveform.TabIndex = 1;
            // 
            // bgWorker
            // 
            this.bgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BgWorker_DoWork);
            // 
            // ChartControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 518);
            this.Controls.Add(this.chrWaveform);
            this.Controls.Add(this.chrNumeric);
            this.Name = "ChartControlForm";
            this.Text = "ChartControlForm";
            this.ResumeLayout(false);

        }

        #endregion

        private ChartControl chrNumeric;
        private ChartControl chrWaveform;
        private System.ComponentModel.BackgroundWorker bgWorker;
    }
}
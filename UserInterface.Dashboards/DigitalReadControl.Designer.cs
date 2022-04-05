
namespace UserInterface.Dashboards
{
    partial class DigitalReadControl
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
            this.led = new UserInterface.Controls.LedControl();
            this.SuspendLayout();
            // 
            // lblChannelName
            // 
            this.lblChannelName.AutoSize = true;
            this.lblChannelName.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblChannelName.Location = new System.Drawing.Point(5, 16);
            this.lblChannelName.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblChannelName.Name = "lblChannelName";
            this.lblChannelName.Size = new System.Drawing.Size(123, 20);
            this.lblChannelName.TabIndex = 4;
            this.lblChannelName.Text = "Channel name";
            // 
            // led
            // 
            this.led.Location = new System.Drawing.Point(218, 7);
            this.led.Margin = new System.Windows.Forms.Padding(5);
            this.led.Name = "led";
            this.led.On = true;
            this.led.Size = new System.Drawing.Size(42, 42);
            this.led.TabIndex = 5;
            this.led.Text = "ledControl1";
            // 
            // DigitalReadControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.led);
            this.Controls.Add(this.lblChannelName);
            this.Margin = new System.Windows.Forms.Padding(8);
            this.Name = "DigitalReadControl";
            this.Size = new System.Drawing.Size(276, 56);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.LabelControl lblChannelName;
        private Controls.LedControl led;
    }
}

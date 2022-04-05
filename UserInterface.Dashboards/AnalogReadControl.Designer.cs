
namespace UserInterface.Dashboards
{
    partial class AnalogReadControl
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
            this.lblChannelValue = new UserInterface.Controls.LabelControl();
            this.lblChannelMeasureUnit = new UserInterface.Controls.LabelControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblChannelName
            // 
            this.lblChannelName.AutoSize = true;
            this.lblChannelName.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblChannelName.Location = new System.Drawing.Point(74, 22);
            this.lblChannelName.Name = "lblChannelName";
            this.lblChannelName.Size = new System.Drawing.Size(123, 20);
            this.lblChannelName.TabIndex = 0;
            this.lblChannelName.Text = "Channel name";
            // 
            // lblChannelValue
            // 
            this.lblChannelValue.AutoSize = true;
            this.lblChannelValue.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblChannelValue.Location = new System.Drawing.Point(39, 16);
            this.lblChannelValue.Name = "lblChannelValue";
            this.lblChannelValue.Size = new System.Drawing.Size(34, 20);
            this.lblChannelValue.TabIndex = 1;
            this.lblChannelValue.Text = "0.0";
            // 
            // lblChannelMeasureUnit
            // 
            this.lblChannelMeasureUnit.AutoSize = true;
            this.lblChannelMeasureUnit.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblChannelMeasureUnit.Location = new System.Drawing.Point(181, 16);
            this.lblChannelMeasureUnit.Name = "lblChannelMeasureUnit";
            this.lblChannelMeasureUnit.Size = new System.Drawing.Size(54, 20);
            this.lblChannelMeasureUnit.TabIndex = 2;
            this.lblChannelMeasureUnit.Text = "[M.U.]";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblChannelMeasureUnit);
            this.panel1.Controls.Add(this.lblChannelValue);
            this.panel1.Location = new System.Drawing.Point(3, 68);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(274, 49);
            this.panel1.TabIndex = 3;
            // 
            // AnalogReadControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblChannelName);
            this.Name = "AnalogReadControl";
            this.Size = new System.Drawing.Size(278, 118);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.LabelControl lblChannelName;
        private Controls.LabelControl lblChannelValue;
        private Controls.LabelControl lblChannelMeasureUnit;
        private System.Windows.Forms.Panel panel1;
    }
}

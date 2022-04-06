
namespace UserInterface.Dashboards
{
    partial class AnalogWriteControl
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
            this.lblChannelMeasureUnit = new UserInterface.Controls.LabelControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txbChannelValue = new UserInterface.Controls.TextControl();
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
            // lblChannelMeasureUnit
            // 
            this.lblChannelMeasureUnit.AutoSize = true;
            this.lblChannelMeasureUnit.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblChannelMeasureUnit.Location = new System.Drawing.Point(181, 16);
            this.lblChannelMeasureUnit.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblChannelMeasureUnit.Name = "lblChannelMeasureUnit";
            this.lblChannelMeasureUnit.Size = new System.Drawing.Size(54, 20);
            this.lblChannelMeasureUnit.TabIndex = 2;
            this.lblChannelMeasureUnit.Text = "[M.U.]";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txbChannelValue);
            this.panel1.Controls.Add(this.lblChannelMeasureUnit);
            this.panel1.Location = new System.Drawing.Point(3, 68);
            this.panel1.Margin = new System.Windows.Forms.Padding(5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(275, 50);
            this.panel1.TabIndex = 5;
            // 
            // txbChannelValue
            // 
            this.txbChannelValue.BackColor = System.Drawing.SystemColors.Control;
            this.txbChannelValue.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txbChannelValue.Location = new System.Drawing.Point(39, 10);
            this.txbChannelValue.Margin = new System.Windows.Forms.Padding(5);
            this.txbChannelValue.Name = "txbChannelValue";
            this.txbChannelValue.Size = new System.Drawing.Size(95, 32);
            this.txbChannelValue.TabIndex = 3;
            this.txbChannelValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxbChannelValue_KeyDown);
            this.txbChannelValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxbChannelValue_KeyPress);
            this.txbChannelValue.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TxbChannelValue_MouseDown);
            // 
            // AnalogWriteControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblChannelName);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(8);
            this.Name = "AnalogWriteControl";
            this.Size = new System.Drawing.Size(276, 116);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.LabelControl lblChannelName;
        private Controls.LabelControl lblChannelMeasureUnit;
        private System.Windows.Forms.Panel panel1;
        private Controls.TextControl txbChannelValue;
    }
}

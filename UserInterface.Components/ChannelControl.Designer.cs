
namespace UserInterface.Controls
{
    partial class ChannelControl
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
            this.lblTimestamp = new UserInterface.Controls.LabelControl();
            this.lblValue = new UserInterface.Controls.LabelControl();
            this.lblCode = new UserInterface.Controls.LabelControl();
            this.SuspendLayout();
            // 
            // lblTimestamp
            // 
            this.lblTimestamp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTimestamp.AutoSize = true;
            this.lblTimestamp.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblTimestamp.Location = new System.Drawing.Point(610, 6);
            this.lblTimestamp.Name = "lblTimestamp";
            this.lblTimestamp.Size = new System.Drawing.Size(114, 20);
            this.lblTimestamp.TabIndex = 4;
            this.lblTimestamp.Text = "00:00:00.000";
            // 
            // lblValue
            // 
            this.lblValue.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblValue.AutoSize = true;
            this.lblValue.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblValue.Location = new System.Drawing.Point(161, 6);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(117, 20);
            this.lblValue.TabIndex = 3;
            this.lblValue.Text = "labelControl1";
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblCode.Location = new System.Drawing.Point(38, 6);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(117, 20);
            this.lblCode.TabIndex = 2;
            this.lblCode.Text = "labelControl1";
            // 
            // ChannelControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblTimestamp);
            this.Controls.Add(this.lblValue);
            this.Controls.Add(this.lblCode);
            this.Name = "ChannelControl";
            this.Size = new System.Drawing.Size(727, 32);
            this.Load += new System.EventHandler(this.ChannelControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LabelControl lblValue;
        private LabelControl lblCode;
        private LabelControl lblTimestamp;
    }
}

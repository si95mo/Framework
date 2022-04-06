
namespace UserInterface.Dashboards
{
    partial class DigitalWriteControl
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
            this.dgcChannelValue = new UserInterface.Controls.DigitalControl();
            this.lblChannelName = new UserInterface.Controls.LabelControl();
            this.SuspendLayout();
            // 
            // dgcChannelValue
            // 
            this.dgcChannelValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.dgcChannelValue.BackColor = System.Drawing.SystemColors.Control;
            this.dgcChannelValue.Font = new System.Drawing.Font("Lucida Sans Unicode", 8F);
            this.dgcChannelValue.Location = new System.Drawing.Point(50, 63);
            this.dgcChannelValue.Margin = new System.Windows.Forms.Padding(8);
            this.dgcChannelValue.Name = "dgcChannelValue";
            this.dgcChannelValue.Size = new System.Drawing.Size(173, 43);
            this.dgcChannelValue.TabIndex = 8;
            this.dgcChannelValue.Value = false;
            this.dgcChannelValue.ValueChanged += new System.EventHandler<Core.ValueChangedEventArgs>(this.DgcChannelValue_ValueChanged);
            // 
            // lblChannelName
            // 
            this.lblChannelName.AutoSize = true;
            this.lblChannelName.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblChannelName.Location = new System.Drawing.Point(74, 22);
            this.lblChannelName.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblChannelName.Name = "lblChannelName";
            this.lblChannelName.Size = new System.Drawing.Size(123, 20);
            this.lblChannelName.TabIndex = 7;
            this.lblChannelName.Text = "Channel name";
            // 
            // DigitalWriteControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.dgcChannelValue);
            this.Controls.Add(this.lblChannelName);
            this.Margin = new System.Windows.Forms.Padding(8);
            this.Name = "DigitalWriteControl";
            this.Size = new System.Drawing.Size(274, 114);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.DigitalControl dgcChannelValue;
        private Controls.LabelControl lblChannelName;
    }
}

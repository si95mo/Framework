
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
            this.pnlTags = new System.Windows.Forms.Panel();
            this.lblDescription = new UserInterface.Controls.LabelControl();
            this.lblType = new UserInterface.Controls.LabelControl();
            this.lblTimestamp = new UserInterface.Controls.LabelControl();
            this.lblValue = new UserInterface.Controls.LabelControl();
            this.lblCode = new UserInterface.Controls.LabelControl();
            this.SuspendLayout();
            // 
            // pnlTags
            // 
            this.pnlTags.Location = new System.Drawing.Point(788, 6);
            this.pnlTags.Name = "pnlTags";
            this.pnlTags.Size = new System.Drawing.Size(198, 20);
            this.pnlTags.TabIndex = 6;
            // 
            // lblDescription
            // 
            this.lblDescription.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblDescription.Location = new System.Drawing.Point(338, 6);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(224, 20);
            this.lblDescription.TabIndex = 7;
            this.lblDescription.Text = "Channel description";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblType
            // 
            this.lblType.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblType.Location = new System.Drawing.Point(568, 6);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(50, 20);
            this.lblType.TabIndex = 5;
            this.lblType.Text = "MSAI";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTimestamp
            // 
            this.lblTimestamp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTimestamp.AutoSize = true;
            this.lblTimestamp.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblTimestamp.Location = new System.Drawing.Point(992, 6);
            this.lblTimestamp.Name = "lblTimestamp";
            this.lblTimestamp.Size = new System.Drawing.Size(114, 20);
            this.lblTimestamp.TabIndex = 4;
            this.lblTimestamp.Text = "00:00:00.000";
            this.lblTimestamp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblValue
            // 
            this.lblValue.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblValue.Location = new System.Drawing.Point(624, 6);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(158, 20);
            this.lblValue.TabIndex = 3;
            this.lblValue.Text = "Channel value";
            this.lblValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCode
            // 
            this.lblCode.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblCode.Location = new System.Drawing.Point(38, 6);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(294, 20);
            this.lblCode.TabIndex = 2;
            this.lblCode.Text = "Channel code";
            // 
            // ChannelControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.pnlTags);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.lblTimestamp);
            this.Controls.Add(this.lblValue);
            this.Controls.Add(this.lblCode);
            this.Name = "ChannelControl";
            this.Size = new System.Drawing.Size(1144, 32);
            this.Load += new System.EventHandler(this.ChannelControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LabelControl lblValue;
        private LabelControl lblCode;
        private LabelControl lblTimestamp;
        private LabelControl lblType;
        private System.Windows.Forms.Panel pnlTags;
        private LabelControl lblDescription;
    }
}

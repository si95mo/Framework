
namespace UserInterface.Controls
{
    partial class ConditionControl
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
            this.lblCode = new UserInterface.Controls.LabelControl();
            this.ledControl = new UserInterface.Controls.LedControl();
            this.SuspendLayout();
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblCode.Location = new System.Drawing.Point(38, 6);
            this.lblCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(117, 20);
            this.lblCode.TabIndex = 7;
            this.lblCode.Text = "labelControl1";
            // 
            // ledControl
            // 
            this.ledControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ledControl.Color = System.Drawing.SystemColors.Control;
            this.ledControl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ledControl.Location = new System.Drawing.Point(462, 0);
            this.ledControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ledControl.Name = "ledControl";
            this.ledControl.On = true;
            this.ledControl.Size = new System.Drawing.Size(75, 32);
            this.ledControl.TabIndex = 9;
            this.ledControl.Text = "ledControl1";
            // 
            // ConditionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ledControl);
            this.Controls.Add(this.lblCode);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ConditionControl";
            this.Size = new System.Drawing.Size(540, 32);
            this.Load += new System.EventHandler(this.ConditionControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private LabelControl lblCode;
        private LedControl ledControl;
    }
}

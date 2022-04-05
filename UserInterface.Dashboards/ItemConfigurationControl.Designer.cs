
namespace UserInterface.Dashboards
{
    partial class ItemConfigurationControl
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
            this.txbChannelCode = new UserInterface.Controls.TextControl();
            this.lblChannelCode = new UserInterface.Controls.LabelControl();
            this.lblDescription = new UserInterface.Controls.LabelControl();
            this.txbDescription = new UserInterface.Controls.TextControl();
            this.SuspendLayout();
            // 
            // txbChannelCode
            // 
            this.txbChannelCode.BackColor = System.Drawing.SystemColors.Control;
            this.txbChannelCode.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txbChannelCode.Location = new System.Drawing.Point(5, 44);
            this.txbChannelCode.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.txbChannelCode.Name = "txbChannelCode";
            this.txbChannelCode.Size = new System.Drawing.Size(310, 32);
            this.txbChannelCode.TabIndex = 0;
            // 
            // lblChannelCode
            // 
            this.lblChannelCode.AutoSize = true;
            this.lblChannelCode.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblChannelCode.Location = new System.Drawing.Point(101, 19);
            this.lblChannelCode.Name = "lblChannelCode";
            this.lblChannelCode.Size = new System.Drawing.Size(117, 20);
            this.lblChannelCode.TabIndex = 1;
            this.lblChannelCode.Text = "Channel code";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblDescription.Location = new System.Drawing.Point(110, 99);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(100, 20);
            this.lblDescription.TabIndex = 2;
            this.lblDescription.Text = "Description";
            // 
            // txbDescription
            // 
            this.txbDescription.BackColor = System.Drawing.SystemColors.Control;
            this.txbDescription.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txbDescription.Location = new System.Drawing.Point(5, 124);
            this.txbDescription.Margin = new System.Windows.Forms.Padding(5);
            this.txbDescription.Name = "txbDescription";
            this.txbDescription.Size = new System.Drawing.Size(310, 32);
            this.txbDescription.TabIndex = 3;
            // 
            // ItemConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txbDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblChannelCode);
            this.Controls.Add(this.txbChannelCode);
            this.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "ItemConfigurationControl";
            this.Size = new System.Drawing.Size(320, 167);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.TextControl txbChannelCode;
        private Controls.LabelControl lblChannelCode;
        private Controls.LabelControl lblDescription;
        private Controls.TextControl txbDescription;
    }
}


using UserInterface.Controls;

namespace UserInterface.Forms
{
    partial class CustomMessageBox
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
            this.btnClose = new UserInterface.Controls.ButtonControl();
            this.txbMessage = new UserInterface.Controls.TextControl();
            this.SuspendLayout();
            // 
            // LblFormName
            // 
            this.LblFormName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.LblFormName.Size = new System.Drawing.Size(155, 25);
            this.LblFormName.Text = "CustomForm";
            // 
            // ControlBox
            // 
            this.ControlBox.Location = new System.Drawing.Point(295, 13);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(220)))), ((int)(((byte)(4)))));
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnClose.Location = new System.Drawing.Point(591, 204);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(150, 32);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "OK";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // txbMessage
            // 
            this.txbMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.txbMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txbMessage.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.txbMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txbMessage.Location = new System.Drawing.Point(12, 42);
            this.txbMessage.Multiline = true;
            this.txbMessage.Name = "txbMessage";
            this.txbMessage.NumericsOnly = false;
            this.txbMessage.PlaceholderText = "";
            this.txbMessage.ReadOnly = true;
            this.txbMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txbMessage.Size = new System.Drawing.Size(730, 194);
            this.txbMessage.TabIndex = 13;
            // 
            // CustomMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.ClientSize = new System.Drawing.Size(753, 248);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txbMessage);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "CustomMessageBox";
            this.Text = "CustomMessageBox";
            this.Controls.SetChildIndex(this.txbMessage, 0);
            this.Controls.SetChildIndex(this.ControlBox, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.LblFormName, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        protected ButtonControl btnClose;
        private TextControl txbMessage;
    }
}
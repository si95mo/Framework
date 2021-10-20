﻿
namespace UserInterface.Controls
{
    partial class CustomForm
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
            this.lblFormName = new System.Windows.Forms.Label();
            this.controlBox = new MetroSet_UI.Controls.MetroSetControlBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // lblFormName
            // 
            this.lblFormName.AutoSize = true;
            this.lblFormName.Font = new System.Drawing.Font("Lucida Sans Unicode", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFormName.Location = new System.Drawing.Point(16, 14);
            this.lblFormName.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.lblFormName.Name = "lblFormName";
            this.lblFormName.Size = new System.Drawing.Size(60, 25);
            this.lblFormName.TabIndex = 9;
            this.lblFormName.Text = "----";
            // 
            // controlBox
            // 
            this.controlBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.controlBox.CloseHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.controlBox.CloseHoverForeColor = System.Drawing.Color.White;
            this.controlBox.CloseNormalForeColor = System.Drawing.Color.Gray;
            this.controlBox.DisabledForeColor = System.Drawing.Color.DimGray;
            this.controlBox.IsDerivedStyle = true;
            this.controlBox.Location = new System.Drawing.Point(286, 13);
            this.controlBox.Margin = new System.Windows.Forms.Padding(4);
            this.controlBox.MaximizeBox = false;
            this.controlBox.MaximizeHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.controlBox.MaximizeHoverForeColor = System.Drawing.Color.Gray;
            this.controlBox.MaximizeNormalForeColor = System.Drawing.Color.Gray;
            this.controlBox.MinimizeBox = true;
            this.controlBox.MinimizeHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.controlBox.MinimizeHoverForeColor = System.Drawing.Color.Gray;
            this.controlBox.MinimizeNormalForeColor = System.Drawing.Color.Gray;
            this.controlBox.Name = "controlBox";
            this.controlBox.Size = new System.Drawing.Size(100, 25);
            this.controlBox.Style = MetroSet_UI.Enums.Style.Light;
            this.controlBox.StyleManager = null;
            this.controlBox.TabIndex = 10;
            this.controlBox.Text = "metroSetControlBox1";
            this.controlBox.ThemeAuthor = "Narwin";
            this.controlBox.ThemeName = "MetroLite";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(220)))), ((int)(((byte)(4)))));
            this.panel1.Location = new System.Drawing.Point(-2, -2);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1343, 15);
            this.panel1.TabIndex = 11;
            // 
            // CustomForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 261);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.controlBox);
            this.Controls.Add(this.lblFormName);
            this.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CustomForm";
            this.Text = "CustomForm";
            this.Load += new System.EventHandler(this.Form_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        protected System.Windows.Forms.Label lblFormName;
        protected MetroSet_UI.Controls.MetroSetControlBox controlBox;
    }
}
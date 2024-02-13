
namespace UserInterface.Forms
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomForm));
            this.LblFormName = new System.Windows.Forms.Label();
            this.ControlBox = new MetroSet_UI.Controls.MetroSetControlBox();
            this.borderUpPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // LblFormName
            // 
            this.LblFormName.AutoSize = true;
            this.LblFormName.Font = new System.Drawing.Font("Lucida Sans Unicode", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblFormName.Location = new System.Drawing.Point(16, 14);
            this.LblFormName.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.LblFormName.Name = "LblFormName";
            this.LblFormName.Size = new System.Drawing.Size(97, 39);
            this.LblFormName.TabIndex = 9;
            this.LblFormName.Text = "----";
            // 
            // ControlBox
            // 
            this.ControlBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ControlBox.CloseHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ControlBox.CloseHoverForeColor = System.Drawing.Color.White;
            this.ControlBox.CloseNormalForeColor = System.Drawing.Color.Gray;
            this.ControlBox.DisabledForeColor = System.Drawing.Color.DimGray;
            this.ControlBox.IsDerivedStyle = true;
            this.ControlBox.Location = new System.Drawing.Point(286, 14);
            this.ControlBox.Margin = new System.Windows.Forms.Padding(4);
            this.ControlBox.MaximizeBox = true;
            this.ControlBox.MaximizeHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.ControlBox.MaximizeHoverForeColor = System.Drawing.Color.Gray;
            this.ControlBox.MaximizeNormalForeColor = System.Drawing.Color.Gray;
            this.ControlBox.MaximumSize = new System.Drawing.Size(200, 200);
            this.ControlBox.MinimizeBox = true;
            this.ControlBox.MinimizeHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.ControlBox.MinimizeHoverForeColor = System.Drawing.Color.Gray;
            this.ControlBox.MinimizeNormalForeColor = System.Drawing.Color.Gray;
            this.ControlBox.Name = "ControlBox";
            this.ControlBox.Size = new System.Drawing.Size(100, 25);
            this.ControlBox.Style = MetroSet_UI.Enums.Style.Light;
            this.ControlBox.StyleManager = null;
            this.ControlBox.TabIndex = 10;
            this.ControlBox.Text = "metroSetControlBox1";
            this.ControlBox.ThemeAuthor = "Narwin";
            this.ControlBox.ThemeName = "MetroLite";
            // 
            // borderUpPanel
            // 
            this.borderUpPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(209)))), ((int)(((byte)(23)))));
            this.borderUpPanel.Location = new System.Drawing.Point(0, 0);
            this.borderUpPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.borderUpPanel.Name = "borderUpPanel";
            this.borderUpPanel.Size = new System.Drawing.Size(400, 13);
            this.borderUpPanel.TabIndex = 11;
            // 
            // CustomForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 261);
            this.Controls.Add(this.borderUpPanel);
            this.Controls.Add(this.ControlBox);
            this.Controls.Add(this.LblFormName);
            this.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CustomForm";
            this.Text = "CustomForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CustomForm_FormClosing);
            this.Load += new System.EventHandler(this.Form_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel borderUpPanel;
        protected System.Windows.Forms.Label LblFormName;
        protected MetroSet_UI.Controls.MetroSetControlBox ControlBox;
    }
}
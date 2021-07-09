
namespace UserInterface.Controls
{
    partial class ComboControl
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
            this.cbxControl = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cbxControl
            // 
            this.cbxControl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxControl.FormattingEnabled = true;
            this.cbxControl.ItemHeight = 13;
            this.cbxControl.Location = new System.Drawing.Point(0, 2);
            this.cbxControl.Name = "cbxControl";
            this.cbxControl.Size = new System.Drawing.Size(150, 21);
            this.cbxControl.TabIndex = 0;
            // 
            // ComboControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.cbxControl);
            this.Name = "ComboControl";
            this.Size = new System.Drawing.Size(150, 25);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbxControl;
    }
}


namespace UserInterface.Controls
{
    partial class PanelSelectorControl
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
            this.btnSelector = new UserInterface.Controls.ButtonControl();
            this.SuspendLayout();
            // 
            // btnSelector
            // 
            this.btnSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(157)))), ((int)(((byte)(199)))));
            this.btnSelector.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnSelector.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelector.Location = new System.Drawing.Point(55, 38);
            this.btnSelector.Name = "btnSelector";
            this.btnSelector.Size = new System.Drawing.Size(150, 25);
            this.btnSelector.TabIndex = 0;
            this.btnSelector.Text = "Panel name";
            this.btnSelector.UseVisualStyleBackColor = false;
            // 
            // PanelSelectorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.Controls.Add(this.btnSelector);
            this.Name = "PanelSelectorControl";
            this.Size = new System.Drawing.Size(260, 100);
            this.ResumeLayout(false);

        }

        #endregion

        private ButtonControl btnSelector;
    }
}

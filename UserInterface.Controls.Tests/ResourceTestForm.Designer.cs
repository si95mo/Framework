
namespace UserInterface.Controls.Tests
{
    partial class ResourceTestForm
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
            this.layoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // lblFormName
            // 
            this.LblFormName.Size = new System.Drawing.Size(145, 25);
            this.LblFormName.Text = "CustomForm";
            // 
            // controlBox
            // 
            this.ControlBox.Location = new System.Drawing.Point(1030, 13);
            // 
            // flowLayoutPanel
            // 
            this.layoutPanel.Location = new System.Drawing.Point(-2, 42);
            this.layoutPanel.Name = "flowLayoutPanel";
            this.layoutPanel.Size = new System.Drawing.Size(1148, 231);
            this.layoutPanel.TabIndex = 12;
            // 
            // ResourceTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1143, 450);
            this.Controls.Add(this.layoutPanel);
            this.Name = "ResourceTestForm";
            this.Text = "ResourceTestForm";
            this.Load += new System.EventHandler(this.ResourceTestForm_Load);
            this.Controls.SetChildIndex(this.LblFormName, 0);
            this.Controls.SetChildIndex(this.ControlBox, 0);
            this.Controls.SetChildIndex(this.layoutPanel, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel layoutPanel;
    }
}
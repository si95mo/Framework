namespace UserInterface.Controls.Panels
{
    partial class ActionPanel
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
            this.layoutPanel = new UserInterface.Controls.VerticalLayoutPanel();
            this.SuspendLayout();
            // 
            // layoutPanel
            // 
            this.layoutPanel.AutoScroll = true;
            this.layoutPanel.Location = new System.Drawing.Point(0, 0);
            this.layoutPanel.Name = "layoutPanel";
            this.layoutPanel.Size = new System.Drawing.Size(420, 200);
            this.layoutPanel.TabIndex = 0;
            this.layoutPanel.WrapContents = false;
            // 
            // ActionPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutPanel);
            this.Name = "ActionPanel";
            this.Size = new System.Drawing.Size(420, 200);
            this.ResumeLayout(false);

        }

        #endregion

        private VerticalLayoutPanel layoutPanel;
    }
}

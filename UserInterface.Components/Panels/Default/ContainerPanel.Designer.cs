namespace UserInterface.Controls.Panels.Default
{
    partial class ContainerPanel
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
            this.Container = new UserInterface.Controls.VerticalLayoutPanel();
            this.SuspendLayout();
            // 
            // Container
            // 
            this.Container.AutoScroll = true;
            this.Container.Location = new System.Drawing.Point(0, 0);
            this.Container.MaximumSize = new System.Drawing.Size(1920, 840);
            this.Container.MinimumSize = new System.Drawing.Size(1920, 840);
            this.Container.Name = "Container";
            this.Container.Size = new System.Drawing.Size(1920, 840);
            this.Container.TabIndex = 0;
            this.Container.WrapContents = false;
            // 
            // ContainerPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.Container);
            this.MaximumSize = new System.Drawing.Size(1920, 840);
            this.MinimumSize = new System.Drawing.Size(1920, 840);
            this.Name = "ContainerPanel";
            this.Size = new System.Drawing.Size(1920, 840);
            this.ResumeLayout(false);

        }

        #endregion

        protected VerticalLayoutPanel Container;
    }
}

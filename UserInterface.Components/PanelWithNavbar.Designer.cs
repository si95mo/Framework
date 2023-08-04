namespace UserInterface.Controls
{
    partial class PanelWithNavbar
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
            this.panelContainer = new System.Windows.Forms.Panel();
            this.PanelNavbar = new UserInterface.Controls.PanelSubNavbar();
            this.SuspendLayout();
            // 
            // panelContainer
            // 
            this.panelContainer.Location = new System.Drawing.Point(0, 0);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Size = new System.Drawing.Size(1920, 860);
            this.panelContainer.TabIndex = 14;
            // 
            // PanelNavbar
            // 
            this.PanelNavbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.PanelNavbar.ContainerForm = null;
            this.PanelNavbar.LayoutSize = new System.Drawing.Size(1920, 60);
            this.PanelNavbar.Location = new System.Drawing.Point(0, 860);
            this.PanelNavbar.Margin = new System.Windows.Forms.Padding(5);
            this.PanelNavbar.MaximumSize = new System.Drawing.Size(1920, 60);
            this.PanelNavbar.MinimumSize = new System.Drawing.Size(1920, 60);
            this.PanelNavbar.Name = "PanelNavbar";
            this.PanelNavbar.Size = new System.Drawing.Size(1920, 60);
            this.PanelNavbar.TabIndex = 0;
            // 
            // PanelWithNavbar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelContainer);
            this.Controls.Add(this.PanelNavbar);
            this.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "PanelWithNavbar";
            this.Size = new System.Drawing.Size(1920, 920);
            this.ResumeLayout(false);

        }

        #endregion

        protected PanelSubNavbar PanelNavbar;
        private System.Windows.Forms.Panel panelContainer;
    }
}

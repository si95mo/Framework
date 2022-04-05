
namespace UserInterface.Dashboards
{
    partial class DashboardPanel
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
            this.dashboard = new System.Windows.Forms.Panel();
            this.configPanel = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.itemPanel = new System.Windows.Forms.Panel();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dashboard
            // 
            this.dashboard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dashboard.Location = new System.Drawing.Point(305, 0);
            this.dashboard.Name = "dashboard";
            this.dashboard.Size = new System.Drawing.Size(1210, 1077);
            this.dashboard.TabIndex = 0;
            // 
            // configPanel
            // 
            this.configPanel.Location = new System.Drawing.Point(1521, 0);
            this.configPanel.Name = "configPanel";
            this.configPanel.Size = new System.Drawing.Size(396, 1077);
            this.configPanel.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.configPanel);
            this.panel3.Controls.Add(this.itemPanel);
            this.panel3.Controls.Add(this.dashboard);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1920, 1080);
            this.panel3.TabIndex = 2;
            // 
            // itemPanel
            // 
            this.itemPanel.Location = new System.Drawing.Point(3, 0);
            this.itemPanel.Name = "itemPanel";
            this.itemPanel.Size = new System.Drawing.Size(296, 1077);
            this.itemPanel.TabIndex = 1;
            // 
            // DashboardPanel
            // 
            this.Controls.Add(this.panel3);
            this.Name = "DashboardPanel";
            this.Size = new System.Drawing.Size(1920, 1080);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel dashboard;
        private System.Windows.Forms.Panel configPanel;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel itemPanel;
    }
}

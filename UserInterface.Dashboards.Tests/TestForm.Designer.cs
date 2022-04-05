
namespace UserInterface.Dashboards.Tests
{
    partial class TestForm
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
            this.dashboard = new UserInterface.Dashboards.DashboardPanel();
            this.closeApplicationControl1 = new UserInterface.Controls.CloseApplicationControl();
            this.SuspendLayout();
            // 
            // dashboard
            // 
            this.dashboard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dashboard.Location = new System.Drawing.Point(0, 0);
            this.dashboard.Name = "dashboard";
            this.dashboard.Size = new System.Drawing.Size(1038, 450);
            this.dashboard.TabIndex = 0;
            // 
            // closeApplicationControl1
            // 
            this.closeApplicationControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeApplicationControl1.BackColor = System.Drawing.SystemColors.Control;
            this.closeApplicationControl1.Location = new System.Drawing.Point(938, 350);
            this.closeApplicationControl1.Name = "closeApplicationControl1";
            this.closeApplicationControl1.Size = new System.Drawing.Size(100, 100);
            this.closeApplicationControl1.TabIndex = 1;
            this.closeApplicationControl1.Value = null;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1038, 450);
            this.Controls.Add(this.closeApplicationControl1);
            this.Controls.Add(this.dashboard);
            this.Name = "TestForm";
            this.Text = "Dashboard test";
            this.ResumeLayout(false);

        }

        #endregion

        private DashboardPanel dashboard;
        private DraggableControl draggableControl;
        private Controls.CloseApplicationControl closeApplicationControl1;
    }
}



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
            this.btnChangeGrid = new System.Windows.Forms.Button();
            this.dashboard = new UserInterface.Dashboards.DashboardPanel();
            this.draggableControl = new UserInterface.Dashboards.DraggableControl();
            this.SuspendLayout();
            // 
            // btnChangeGrid
            // 
            this.btnChangeGrid.Location = new System.Drawing.Point(806, 12);
            this.btnChangeGrid.Name = "btnChangeGrid";
            this.btnChangeGrid.Size = new System.Drawing.Size(164, 23);
            this.btnChangeGrid.TabIndex = 1;
            this.btnChangeGrid.Text = "Change grid visualization";
            this.btnChangeGrid.UseVisualStyleBackColor = true;
            this.btnChangeGrid.Click += new System.EventHandler(this.BtnChangeGrid_Click);
            // 
            // dashboard
            // 
            this.dashboard.Location = new System.Drawing.Point(0, 0);
            this.dashboard.Name = "dashboard";
            this.dashboard.Size = new System.Drawing.Size(800, 450);
            this.dashboard.TabIndex = 0;
            // 
            // draggableControl
            // 
            this.draggableControl.BackColor = System.Drawing.SystemColors.Highlight;
            this.draggableControl.Location = new System.Drawing.Point(806, 41);
            this.draggableControl.Name = "draggableControl";
            this.draggableControl.Size = new System.Drawing.Size(164, 53);
            this.draggableControl.TabIndex = 2;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 450);
            this.Controls.Add(this.draggableControl);
            this.Controls.Add(this.btnChangeGrid);
            this.Controls.Add(this.dashboard);
            this.Name = "TestForm";
            this.Text = "Dashboard test";
            this.ResumeLayout(false);

        }

        #endregion

        private DashboardPanel dashboard;
        private System.Windows.Forms.Button btnChangeGrid;
        private DraggableControl draggableControl;
    }
}


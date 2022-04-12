
namespace UserInterface.Dashboards
{
    partial class SaveForm
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
            this.labelControl1 = new UserInterface.Controls.LabelControl();
            this.txcDashboardName = new UserInterface.Controls.TextControl();
            this.btcSaveDashboard = new UserInterface.Controls.ButtonControl();
            this.SuspendLayout();
            // 
            // lblFormName
            // 
            this.lblFormName.Size = new System.Drawing.Size(145, 25);
            this.lblFormName.Text = "CustomForm";
            // 
            // controlBox
            // 
            this.controlBox.Location = new System.Drawing.Point(413, 13);
            // 
            // labelControl1
            // 
            this.labelControl1.AutoSize = true;
            this.labelControl1.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.labelControl1.Location = new System.Drawing.Point(12, 73);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(143, 20);
            this.labelControl1.TabIndex = 12;
            this.labelControl1.Text = "Dashboard name";
            // 
            // txcDashboardName
            // 
            this.txcDashboardName.BackColor = System.Drawing.SystemColors.Control;
            this.txcDashboardName.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txcDashboardName.Location = new System.Drawing.Point(221, 70);
            this.txcDashboardName.Name = "txcDashboardName";
            this.txcDashboardName.Size = new System.Drawing.Size(284, 32);
            this.txcDashboardName.TabIndex = 13;
            // 
            // btcSaveDashboard
            // 
            this.btcSaveDashboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btcSaveDashboard.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btcSaveDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btcSaveDashboard.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btcSaveDashboard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btcSaveDashboard.Location = new System.Drawing.Point(221, 121);
            this.btcSaveDashboard.Name = "btcSaveDashboard";
            this.btcSaveDashboard.Size = new System.Drawing.Size(284, 32);
            this.btcSaveDashboard.TabIndex = 14;
            this.btcSaveDashboard.Text = "Save dashboard";
            this.btcSaveDashboard.UseVisualStyleBackColor = false;
            this.btcSaveDashboard.Click += new System.EventHandler(this.BtnSaveDashboard_Click);
            // 
            // SaveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 165);
            this.Controls.Add(this.btcSaveDashboard);
            this.Controls.Add(this.txcDashboardName);
            this.Controls.Add(this.labelControl1);
            this.Name = "SaveForm";
            this.Text = "";
            this.Controls.SetChildIndex(this.lblFormName, 0);
            this.Controls.SetChildIndex(this.controlBox, 0);
            this.Controls.SetChildIndex(this.labelControl1, 0);
            this.Controls.SetChildIndex(this.txcDashboardName, 0);
            this.Controls.SetChildIndex(this.btcSaveDashboard, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.LabelControl labelControl1;
        private Controls.TextControl txcDashboardName;
        private Controls.ButtonControl btcSaveDashboard;
    }
}
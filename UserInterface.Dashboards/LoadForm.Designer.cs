
namespace UserInterface.Dashboards
{
    partial class LoadForm
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
            this.btcLoadDashboard = new UserInterface.Controls.ButtonControl();
            this.txcDashboardName = new UserInterface.Controls.TextControl();
            this.labelControl1 = new UserInterface.Controls.LabelControl();
            this.SuspendLayout();
            // 
            // lblFormName
            // 
            this.LblFormName.Size = new System.Drawing.Size(145, 25);
            this.LblFormName.Text = "CustomForm";
            // 
            // controlBox
            // 
            this.ControlBox.Location = new System.Drawing.Point(413, 13);
            // 
            // btcLoadDashboard
            // 
            this.btcLoadDashboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btcLoadDashboard.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btcLoadDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btcLoadDashboard.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btcLoadDashboard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btcLoadDashboard.Location = new System.Drawing.Point(221, 121);
            this.btcLoadDashboard.Name = "btcLoadDashboard";
            this.btcLoadDashboard.Size = new System.Drawing.Size(284, 32);
            this.btcLoadDashboard.TabIndex = 17;
            this.btcLoadDashboard.Text = "Load dashboard";
            this.btcLoadDashboard.UseVisualStyleBackColor = false;
            this.btcLoadDashboard.Click += new System.EventHandler(this.BtnLoadDashboard_Click);
            // 
            // txcDashboardName
            // 
            this.txcDashboardName.BackColor = System.Drawing.SystemColors.Control;
            this.txcDashboardName.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txcDashboardName.Location = new System.Drawing.Point(221, 70);
            this.txcDashboardName.Name = "txcDashboardName";
            this.txcDashboardName.Size = new System.Drawing.Size(284, 32);
            this.txcDashboardName.TabIndex = 16;
            // 
            // labelControl1
            // 
            this.labelControl1.AutoSize = true;
            this.labelControl1.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.labelControl1.Location = new System.Drawing.Point(12, 73);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(143, 20);
            this.labelControl1.TabIndex = 15;
            this.labelControl1.Text = "Dashboard name";
            // 
            // LoadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 165);
            this.Controls.Add(this.btcLoadDashboard);
            this.Controls.Add(this.txcDashboardName);
            this.Controls.Add(this.labelControl1);
            this.Name = "LoadForm";
            this.Text = "LoadForm";
            this.Controls.SetChildIndex(this.LblFormName, 0);
            this.Controls.SetChildIndex(this.ControlBox, 0);
            this.Controls.SetChildIndex(this.labelControl1, 0);
            this.Controls.SetChildIndex(this.txcDashboardName, 0);
            this.Controls.SetChildIndex(this.btcLoadDashboard, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.ButtonControl btcLoadDashboard;
        private Controls.TextControl txcDashboardName;
        private Controls.LabelControl labelControl1;
    }
}
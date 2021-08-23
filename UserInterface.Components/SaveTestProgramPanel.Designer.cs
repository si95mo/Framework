
namespace UserInterface.Controls
{
    partial class SaveTestProgramPanel
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
            this.lblTestProgramName = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panelContainer = new System.Windows.Forms.Panel();
            this.txbTestProgramName = new UserInterface.Controls.TextControl();
            this.btnSave = new UserInterface.Controls.ButtonControl();
            this.btnCancel = new UserInterface.Controls.ButtonControl();
            this.flowLayoutPanel1.SuspendLayout();
            this.panelContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTestProgramName
            // 
            this.lblTestProgramName.AutoSize = true;
            this.lblTestProgramName.Location = new System.Drawing.Point(44, 21);
            this.lblTestProgramName.Name = "lblTestProgramName";
            this.lblTestProgramName.Size = new System.Drawing.Size(125, 13);
            this.lblTestProgramName.TabIndex = 7;
            this.lblTestProgramName.Text = "Enter test program name:";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnSave);
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(44, 20);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(312, 30);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // panelContainer
            // 
            this.panelContainer.BackColor = System.Drawing.SystemColors.Info;
            this.panelContainer.Controls.Add(this.flowLayoutPanel1);
            this.panelContainer.Location = new System.Drawing.Point(0, 60);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Size = new System.Drawing.Size(400, 70);
            this.panelContainer.TabIndex = 5;
            // 
            // txbTestProgramName
            // 
            this.txbTestProgramName.BackColor = System.Drawing.SystemColors.Control;
            this.txbTestProgramName.Location = new System.Drawing.Point(203, 15);
            this.txbTestProgramName.Name = "txbTestProgramName";
            this.txbTestProgramName.Size = new System.Drawing.Size(150, 25);
            this.txbTestProgramName.TabIndex = 6;
            this.txbTestProgramName.Value = "";
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(157)))), ((int)(((byte)(199)))));
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(3, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(150, 25);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.BtnSaveClick);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(157)))), ((int)(((byte)(199)))));
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(159, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // SaveTestProgramPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblTestProgramName);
            this.Controls.Add(this.txbTestProgramName);
            this.Controls.Add(this.panelContainer);
            this.Name = "SaveTestProgramPanel";
            this.Size = new System.Drawing.Size(398, 129);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panelContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private TextControl txbTestProgramName;
        private System.Windows.Forms.Label lblTestProgramName;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private ButtonControl btnSave;
        private ButtonControl btnCancel;
        private System.Windows.Forms.Panel panelContainer;
    }
}

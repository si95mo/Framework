namespace UserInterface.Controls.Tests
{
    partial class TasksForm
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
            this.taskFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.schedulerFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.btnFireAlarm = new UserInterface.Controls.ButtonControl();
            this.btnResetAlarm = new UserInterface.Controls.ButtonControl();
            this.SuspendLayout();
            // 
            // LblFormName
            // 
            this.LblFormName.Margin = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.LblFormName.Size = new System.Drawing.Size(155, 25);
            this.LblFormName.Text = "CustomForm";
            // 
            // ControlBox
            // 
            this.ControlBox.Location = new System.Drawing.Point(492, 20);
            this.ControlBox.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            // 
            // taskFlowLayout
            // 
            this.taskFlowLayout.AutoScroll = true;
            this.taskFlowLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.taskFlowLayout.Location = new System.Drawing.Point(0, 56);
            this.taskFlowLayout.Margin = new System.Windows.Forms.Padding(5);
            this.taskFlowLayout.Name = "taskFlowLayout";
            this.taskFlowLayout.Size = new System.Drawing.Size(1000, 900);
            this.taskFlowLayout.TabIndex = 1;
            this.taskFlowLayout.WrapContents = false;
            // 
            // schedulerFlowLayout
            // 
            this.schedulerFlowLayout.AutoScroll = true;
            this.schedulerFlowLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.schedulerFlowLayout.Location = new System.Drawing.Point(1010, 56);
            this.schedulerFlowLayout.Margin = new System.Windows.Forms.Padding(5);
            this.schedulerFlowLayout.Name = "schedulerFlowLayout";
            this.schedulerFlowLayout.Size = new System.Drawing.Size(900, 900);
            this.schedulerFlowLayout.TabIndex = 2;
            this.schedulerFlowLayout.WrapContents = false;
            // 
            // btnFireAlarm
            // 
            this.btnFireAlarm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnFireAlarm.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnFireAlarm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(209)))), ((int)(((byte)(23)))));
            this.btnFireAlarm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFireAlarm.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFireAlarm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnFireAlarm.Location = new System.Drawing.Point(850, 964);
            this.btnFireAlarm.Name = "btnFireAlarm";
            this.btnFireAlarm.Size = new System.Drawing.Size(150, 32);
            this.btnFireAlarm.TabIndex = 12;
            this.btnFireAlarm.Text = "Fire alarm";
            this.btnFireAlarm.UseVisualStyleBackColor = false;
            this.btnFireAlarm.Click += new System.EventHandler(this.BtnFireAlarm_Click);
            // 
            // btnResetAlarm
            // 
            this.btnResetAlarm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnResetAlarm.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnResetAlarm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(209)))), ((int)(((byte)(23)))));
            this.btnResetAlarm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetAlarm.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResetAlarm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnResetAlarm.Location = new System.Drawing.Point(1010, 964);
            this.btnResetAlarm.Name = "btnResetAlarm";
            this.btnResetAlarm.Size = new System.Drawing.Size(150, 32);
            this.btnResetAlarm.TabIndex = 12;
            this.btnResetAlarm.Text = "Reset alarm";
            this.btnResetAlarm.UseVisualStyleBackColor = false;
            this.btnResetAlarm.Click += new System.EventHandler(this.BtnReserAlarm_Click);
            // 
            // TasksForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1940, 1100);
            this.Controls.Add(this.btnResetAlarm);
            this.Controls.Add(this.btnFireAlarm);
            this.Controls.Add(this.schedulerFlowLayout);
            this.Controls.Add(this.taskFlowLayout);
            this.Location = new System.Drawing.Point(0, 0);
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Name = "TasksForm";
            this.Text = "TasksForm";
            this.Load += new System.EventHandler(this.TasksForm_Load);
            this.Controls.SetChildIndex(this.taskFlowLayout, 0);
            this.Controls.SetChildIndex(this.schedulerFlowLayout, 0);
            this.Controls.SetChildIndex(this.LblFormName, 0);
            this.Controls.SetChildIndex(this.ControlBox, 0);
            this.Controls.SetChildIndex(this.btnFireAlarm, 0);
            this.Controls.SetChildIndex(this.btnResetAlarm, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel taskFlowLayout;
        private System.Windows.Forms.FlowLayoutPanel schedulerFlowLayout;
        private ButtonControl btnFireAlarm;
        private ButtonControl btnResetAlarm;
    }
}
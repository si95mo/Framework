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
            this.panelWithLed = new UserInterface.Controls.PanelWithLed();
            this.lblSchedulerLoad = new UserInterface.Controls.LabelControl();
            this.btnShowToaster = new UserInterface.Controls.ButtonControl();
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
            this.taskFlowLayout.Size = new System.Drawing.Size(1550, 520);
            this.taskFlowLayout.TabIndex = 1;
            this.taskFlowLayout.WrapContents = false;
            // 
            // schedulerFlowLayout
            // 
            this.schedulerFlowLayout.AutoScroll = true;
            this.schedulerFlowLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.schedulerFlowLayout.Location = new System.Drawing.Point(0, 580);
            this.schedulerFlowLayout.Margin = new System.Windows.Forms.Padding(5);
            this.schedulerFlowLayout.Name = "schedulerFlowLayout";
            this.schedulerFlowLayout.Size = new System.Drawing.Size(1550, 520);
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
            this.btnFireAlarm.Location = new System.Drawing.Point(1568, 56);
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
            this.btnResetAlarm.Location = new System.Drawing.Point(1568, 94);
            this.btnResetAlarm.Name = "btnResetAlarm";
            this.btnResetAlarm.Size = new System.Drawing.Size(150, 32);
            this.btnResetAlarm.TabIndex = 12;
            this.btnResetAlarm.Text = "Reset alarm";
            this.btnResetAlarm.UseVisualStyleBackColor = false;
            this.btnResetAlarm.Click += new System.EventHandler(this.BtnReserAlarm_Click);
            // 
            // panelWithLed
            // 
            this.panelWithLed.BackColor = System.Drawing.Color.White;
            this.panelWithLed.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.panelWithLed.Location = new System.Drawing.Point(1726, 56);
            this.panelWithLed.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.panelWithLed.MinimumSize = new System.Drawing.Size(200, 30);
            this.panelWithLed.Name = "panelWithLed";
            this.panelWithLed.Size = new System.Drawing.Size(200, 120);
            this.panelWithLed.TabIndex = 13;
            this.panelWithLed.Title = "Scheduler load";
            // 
            // lblSchedulerLoad
            // 
            this.lblSchedulerLoad.BackColor = System.Drawing.Color.White;
            this.lblSchedulerLoad.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblSchedulerLoad.Location = new System.Drawing.Point(1738, 90);
            this.lblSchedulerLoad.Name = "lblSchedulerLoad";
            this.lblSchedulerLoad.Size = new System.Drawing.Size(188, 85);
            this.lblSchedulerLoad.TabIndex = 14;
            this.lblSchedulerLoad.Text = "--";
            this.lblSchedulerLoad.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnShowToaster
            // 
            this.btnShowToaster.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnShowToaster.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnShowToaster.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(209)))), ((int)(((byte)(23)))));
            this.btnShowToaster.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowToaster.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowToaster.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnShowToaster.Location = new System.Drawing.Point(1568, 144);
            this.btnShowToaster.Name = "btnShowToaster";
            this.btnShowToaster.Size = new System.Drawing.Size(150, 32);
            this.btnShowToaster.TabIndex = 12;
            this.btnShowToaster.Text = "Show toaster";
            this.btnShowToaster.UseVisualStyleBackColor = false;
            this.btnShowToaster.Click += new System.EventHandler(this.BtnShowToaster_Click);
            // 
            // TasksForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1940, 1100);
            this.Controls.Add(this.lblSchedulerLoad);
            this.Controls.Add(this.btnResetAlarm);
            this.Controls.Add(this.panelWithLed);
            this.Controls.Add(this.btnShowToaster);
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
            this.Controls.SetChildIndex(this.btnFireAlarm, 0);
            this.Controls.SetChildIndex(this.btnShowToaster, 0);
            this.Controls.SetChildIndex(this.panelWithLed, 0);
            this.Controls.SetChildIndex(this.btnResetAlarm, 0);
            this.Controls.SetChildIndex(this.lblSchedulerLoad, 0);
            this.Controls.SetChildIndex(this.LblFormName, 0);
            this.Controls.SetChildIndex(this.ControlBox, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel taskFlowLayout;
        private System.Windows.Forms.FlowLayoutPanel schedulerFlowLayout;
        private ButtonControl btnFireAlarm;
        private ButtonControl btnResetAlarm;
        private PanelWithLed panelWithLed;
        private LabelControl lblSchedulerLoad;
        private ButtonControl btnShowToaster;
    }
}
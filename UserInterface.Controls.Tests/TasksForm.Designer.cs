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
            this.SuspendLayout();
            // 
            // lblFormName
            // 
            this.LblFormName.Margin = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.LblFormName.Size = new System.Drawing.Size(155, 25);
            this.LblFormName.Text = "CustomForm";
            // 
            // controlBox
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
            // TasksForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1940, 1100);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel taskFlowLayout;
        private System.Windows.Forms.FlowLayoutPanel schedulerFlowLayout;
    }
}
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
            // taskFlowLayout
            // 
            this.taskFlowLayout.AutoScroll = true;
            this.taskFlowLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.taskFlowLayout.Location = new System.Drawing.Point(0, 0);
            this.taskFlowLayout.Name = "taskFlowLayout";
            this.taskFlowLayout.Size = new System.Drawing.Size(1181, 498);
            this.taskFlowLayout.TabIndex = 1;
            this.taskFlowLayout.WrapContents = false;
            // 
            // schedulerFlowLayout
            // 
            this.schedulerFlowLayout.AutoScroll = true;
            this.schedulerFlowLayout.Location = new System.Drawing.Point(0, 504);
            this.schedulerFlowLayout.Name = "schedulerFlowLayout";
            this.schedulerFlowLayout.Size = new System.Drawing.Size(1181, 318);
            this.schedulerFlowLayout.TabIndex = 2;
            this.schedulerFlowLayout.WrapContents = false;
            // 
            // TasksForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1181, 822);
            this.Controls.Add(this.schedulerFlowLayout);
            this.Controls.Add(this.taskFlowLayout);
            this.Name = "TasksForm";
            this.Text = "TasksForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel taskFlowLayout;
        private System.Windows.Forms.FlowLayoutPanel schedulerFlowLayout;
    }
}
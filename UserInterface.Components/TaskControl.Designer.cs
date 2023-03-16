namespace UserInterface.Controls
{
    partial class TaskControl
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
            this.btnStart = new System.Windows.Forms.Button();
            this.lblTaskWaitState = new System.Windows.Forms.Label();
            this.lblTaskStatus = new System.Windows.Forms.Label();
            this.lblTaskCode = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.Transparent;
            this.btnStart.FlatAppearance.BorderSize = 0;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Image = global::UserInterface.Controls.Properties.Resources.Start;
            this.btnStart.Location = new System.Drawing.Point(1073, 0);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(32, 32);
            this.btnStart.TabIndex = 9;
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // lblTaskWaitState
            // 
            this.lblTaskWaitState.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblTaskWaitState.Location = new System.Drawing.Point(516, 0);
            this.lblTaskWaitState.Name = "lblTaskWaitState";
            this.lblTaskWaitState.Size = new System.Drawing.Size(551, 30);
            this.lblTaskWaitState.TabIndex = 7;
            this.lblTaskWaitState.Text = "Wait state";
            this.lblTaskWaitState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTaskStatus
            // 
            this.lblTaskStatus.AutoSize = true;
            this.lblTaskStatus.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblTaskStatus.Location = new System.Drawing.Point(360, 4);
            this.lblTaskStatus.Name = "lblTaskStatus";
            this.lblTaskStatus.Size = new System.Drawing.Size(150, 20);
            this.lblTaskStatus.TabIndex = 6;
            this.lblTaskStatus.Text = "RanToCompletion";
            // 
            // lblTaskCode
            // 
            this.lblTaskCode.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblTaskCode.Location = new System.Drawing.Point(5, 0);
            this.lblTaskCode.Name = "lblTaskCode";
            this.lblTaskCode.Size = new System.Drawing.Size(349, 30);
            this.lblTaskCode.TabIndex = 5;
            this.lblTaskCode.Text = "Task code";
            this.lblTaskCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.Transparent;
            this.btnStop.FlatAppearance.BorderSize = 0;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Image = global::UserInterface.Controls.Properties.Resources.Stop;
            this.btnStop.Location = new System.Drawing.Point(1111, 0);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(32, 32);
            this.btnStop.TabIndex = 10;
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // TaskControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblTaskWaitState);
            this.Controls.Add(this.lblTaskStatus);
            this.Controls.Add(this.lblTaskCode);
            this.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "TaskControl";
            this.Size = new System.Drawing.Size(1144, 32);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblTaskWaitState;
        private System.Windows.Forms.Label lblTaskStatus;
        private System.Windows.Forms.Label lblTaskCode;
        private System.Windows.Forms.Button btnStop;
    }
}

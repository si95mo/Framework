
namespace UserInterface.Controls
{
    partial class ResourceControl
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
            this.lblResourceCode = new System.Windows.Forms.Label();
            this.lblResourceStatus = new System.Windows.Forms.Label();
            this.lblResourceFailure = new System.Windows.Forms.Label();
            this.lblFailureTimestamp = new System.Windows.Forms.Label();
            this.btnRestartResource = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblResourceCode
            // 
            this.lblResourceCode.AutoSize = true;
            this.lblResourceCode.Location = new System.Drawing.Point(3, 10);
            this.lblResourceCode.Name = "lblResourceCode";
            this.lblResourceCode.Size = new System.Drawing.Size(80, 13);
            this.lblResourceCode.TabIndex = 0;
            this.lblResourceCode.Text = "Resource code";
            // 
            // lblResourceStatus
            // 
            this.lblResourceStatus.AutoSize = true;
            this.lblResourceStatus.Location = new System.Drawing.Point(138, 10);
            this.lblResourceStatus.Name = "lblResourceStatus";
            this.lblResourceStatus.Size = new System.Drawing.Size(54, 13);
            this.lblResourceStatus.TabIndex = 1;
            this.lblResourceStatus.Text = "Executing";
            // 
            // lblResourceFailure
            // 
            this.lblResourceFailure.Location = new System.Drawing.Point(219, 10);
            this.lblResourceFailure.Name = "lblResourceFailure";
            this.lblResourceFailure.Size = new System.Drawing.Size(345, 13);
            this.lblResourceFailure.TabIndex = 2;
            this.lblResourceFailure.Text = "Resource failure";
            // 
            // lblFailureTimestamp
            // 
            this.lblFailureTimestamp.AutoSize = true;
            this.lblFailureTimestamp.Location = new System.Drawing.Point(570, 10);
            this.lblFailureTimestamp.Name = "lblFailureTimestamp";
            this.lblFailureTimestamp.Size = new System.Drawing.Size(88, 13);
            this.lblFailureTimestamp.TabIndex = 3;
            this.lblFailureTimestamp.Text = "Failure timestamp";
            // 
            // btnRestartResource
            // 
            this.btnRestartResource.BackColor = System.Drawing.Color.Transparent;
            this.btnRestartResource.FlatAppearance.BorderSize = 0;
            this.btnRestartResource.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestartResource.Image = global::UserInterface.Controls.Properties.Resources.Start;
            this.btnRestartResource.Location = new System.Drawing.Point(692, 0);
            this.btnRestartResource.Name = "btnRestartResource";
            this.btnRestartResource.Size = new System.Drawing.Size(32, 32);
            this.btnRestartResource.TabIndex = 4;
            this.btnRestartResource.UseVisualStyleBackColor = false;
            // 
            // ResourceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnRestartResource);
            this.Controls.Add(this.lblFailureTimestamp);
            this.Controls.Add(this.lblResourceFailure);
            this.Controls.Add(this.lblResourceStatus);
            this.Controls.Add(this.lblResourceCode);
            this.Name = "ResourceControl";
            this.Size = new System.Drawing.Size(727, 32);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblResourceCode;
        private System.Windows.Forms.Label lblResourceStatus;
        private System.Windows.Forms.Label lblResourceFailure;
        private System.Windows.Forms.Label lblFailureTimestamp;
        private System.Windows.Forms.Button btnRestartResource;
    }
}

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
            this.lblTaskWaitState = new System.Windows.Forms.Label();
            this.lblTaskStatus = new System.Windows.Forms.Label();
            this.lblTaskCode = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.pbxDetails = new System.Windows.Forms.PictureBox();
            this.outputPanel = new UserInterface.Controls.VerticalLayoutPanel();
            this.inputPanel = new UserInterface.Controls.VerticalLayoutPanel();
            this.lblWaitState8 = new UserInterface.Controls.LabelControl();
            this.lblWaitState7 = new UserInterface.Controls.LabelControl();
            this.lblWaitState6 = new UserInterface.Controls.LabelControl();
            this.lblWaitState5 = new UserInterface.Controls.LabelControl();
            this.lblWaitState4 = new UserInterface.Controls.LabelControl();
            this.lblWaitState3 = new UserInterface.Controls.LabelControl();
            this.lblWaitState2 = new UserInterface.Controls.LabelControl();
            this.lblWaitState1 = new UserInterface.Controls.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.pbxDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTaskWaitState
            // 
            this.lblTaskWaitState.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblTaskWaitState.Location = new System.Drawing.Point(575, 0);
            this.lblTaskWaitState.Name = "lblTaskWaitState";
            this.lblTaskWaitState.Size = new System.Drawing.Size(492, 30);
            this.lblTaskWaitState.TabIndex = 7;
            this.lblTaskWaitState.Text = "Wait state";
            this.lblTaskWaitState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTaskStatus
            // 
            this.lblTaskStatus.AutoSize = true;
            this.lblTaskStatus.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblTaskStatus.Location = new System.Drawing.Point(419, 5);
            this.lblTaskStatus.Name = "lblTaskStatus";
            this.lblTaskStatus.Size = new System.Drawing.Size(150, 20);
            this.lblTaskStatus.TabIndex = 6;
            this.lblTaskStatus.Text = "RanToCompletion";
            // 
            // lblTaskCode
            // 
            this.lblTaskCode.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblTaskCode.Location = new System.Drawing.Point(38, 0);
            this.lblTaskCode.Name = "lblTaskCode";
            this.lblTaskCode.Size = new System.Drawing.Size(375, 30);
            this.lblTaskCode.TabIndex = 5;
            this.lblTaskCode.Text = "Task code";
            this.lblTaskCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.Transparent;
            this.btnStop.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnStop.FlatAppearance.BorderSize = 0;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Image = global::UserInterface.Controls.Properties.Resources.Stop;
            this.btnStop.Location = new System.Drawing.Point(1111, -1);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(32, 32);
            this.btnStop.TabIndex = 10;
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.Transparent;
            this.btnStart.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnStart.FlatAppearance.BorderSize = 0;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Image = global::UserInterface.Controls.Properties.Resources.Start;
            this.btnStart.Location = new System.Drawing.Point(1073, -1);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(32, 32);
            this.btnStart.TabIndex = 9;
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // pbxDetails
            // 
            this.pbxDetails.Image = global::UserInterface.Controls.Properties.Resources.ChevronDown;
            this.pbxDetails.Location = new System.Drawing.Point(0, 0);
            this.pbxDetails.Name = "pbxDetails";
            this.pbxDetails.Size = new System.Drawing.Size(32, 32);
            this.pbxDetails.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbxDetails.TabIndex = 11;
            this.pbxDetails.TabStop = false;
            this.pbxDetails.Click += new System.EventHandler(this.PbxDetails_Click);
            // 
            // outputPanel
            // 
            this.outputPanel.AutoScroll = true;
            this.outputPanel.Location = new System.Drawing.Point(38, 132);
            this.outputPanel.Name = "outputPanel";
            this.outputPanel.Size = new System.Drawing.Size(531, 100);
            this.outputPanel.TabIndex = 23;
            this.outputPanel.WrapContents = false;
            // 
            // inputPanel
            // 
            this.inputPanel.AutoScroll = true;
            this.inputPanel.Location = new System.Drawing.Point(38, 32);
            this.inputPanel.Name = "inputPanel";
            this.inputPanel.Size = new System.Drawing.Size(531, 100);
            this.inputPanel.TabIndex = 22;
            this.inputPanel.WrapContents = false;
            // 
            // lblWaitState8
            // 
            this.lblWaitState8.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblWaitState8.Location = new System.Drawing.Point(579, 202);
            this.lblWaitState8.Margin = new System.Windows.Forms.Padding(0);
            this.lblWaitState8.Name = "lblWaitState8";
            this.lblWaitState8.Size = new System.Drawing.Size(564, 24);
            this.lblWaitState8.TabIndex = 19;
            this.lblWaitState8.Text = "labelControl8";
            // 
            // lblWaitState7
            // 
            this.lblWaitState7.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblWaitState7.Location = new System.Drawing.Point(579, 178);
            this.lblWaitState7.Margin = new System.Windows.Forms.Padding(0);
            this.lblWaitState7.Name = "lblWaitState7";
            this.lblWaitState7.Size = new System.Drawing.Size(564, 24);
            this.lblWaitState7.TabIndex = 18;
            this.lblWaitState7.Text = "labelControl7";
            // 
            // lblWaitState6
            // 
            this.lblWaitState6.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblWaitState6.Location = new System.Drawing.Point(579, 154);
            this.lblWaitState6.Margin = new System.Windows.Forms.Padding(0);
            this.lblWaitState6.Name = "lblWaitState6";
            this.lblWaitState6.Size = new System.Drawing.Size(564, 24);
            this.lblWaitState6.TabIndex = 17;
            this.lblWaitState6.Text = "labelControl6";
            // 
            // lblWaitState5
            // 
            this.lblWaitState5.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblWaitState5.Location = new System.Drawing.Point(579, 130);
            this.lblWaitState5.Margin = new System.Windows.Forms.Padding(0);
            this.lblWaitState5.Name = "lblWaitState5";
            this.lblWaitState5.Size = new System.Drawing.Size(564, 24);
            this.lblWaitState5.TabIndex = 16;
            this.lblWaitState5.Text = "labelControl5";
            // 
            // lblWaitState4
            // 
            this.lblWaitState4.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblWaitState4.Location = new System.Drawing.Point(579, 106);
            this.lblWaitState4.Margin = new System.Windows.Forms.Padding(0);
            this.lblWaitState4.Name = "lblWaitState4";
            this.lblWaitState4.Size = new System.Drawing.Size(564, 24);
            this.lblWaitState4.TabIndex = 15;
            this.lblWaitState4.Text = "labelControl4";
            // 
            // lblWaitState3
            // 
            this.lblWaitState3.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblWaitState3.Location = new System.Drawing.Point(579, 82);
            this.lblWaitState3.Margin = new System.Windows.Forms.Padding(0);
            this.lblWaitState3.Name = "lblWaitState3";
            this.lblWaitState3.Size = new System.Drawing.Size(564, 24);
            this.lblWaitState3.TabIndex = 14;
            this.lblWaitState3.Text = "labelControl3";
            // 
            // lblWaitState2
            // 
            this.lblWaitState2.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblWaitState2.Location = new System.Drawing.Point(579, 58);
            this.lblWaitState2.Margin = new System.Windows.Forms.Padding(0);
            this.lblWaitState2.Name = "lblWaitState2";
            this.lblWaitState2.Size = new System.Drawing.Size(564, 24);
            this.lblWaitState2.TabIndex = 13;
            this.lblWaitState2.Text = "labelControl2";
            // 
            // lblWaitState1
            // 
            this.lblWaitState1.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F);
            this.lblWaitState1.Location = new System.Drawing.Point(579, 34);
            this.lblWaitState1.Margin = new System.Windows.Forms.Padding(0);
            this.lblWaitState1.Name = "lblWaitState1";
            this.lblWaitState1.Size = new System.Drawing.Size(564, 24);
            this.lblWaitState1.TabIndex = 12;
            this.lblWaitState1.Text = "labelControl1";
            // 
            // TaskControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.outputPanel);
            this.Controls.Add(this.inputPanel);
            this.Controls.Add(this.lblWaitState8);
            this.Controls.Add(this.lblWaitState7);
            this.Controls.Add(this.lblWaitState6);
            this.Controls.Add(this.lblWaitState5);
            this.Controls.Add(this.lblWaitState4);
            this.Controls.Add(this.lblWaitState3);
            this.Controls.Add(this.lblWaitState2);
            this.Controls.Add(this.lblWaitState1);
            this.Controls.Add(this.pbxDetails);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblTaskWaitState);
            this.Controls.Add(this.lblTaskStatus);
            this.Controls.Add(this.lblTaskCode);
            this.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "TaskControl";
            this.Size = new System.Drawing.Size(1144, 232);
            this.DoubleClick += new System.EventHandler(this.Control_DoubleClick);
            ((System.ComponentModel.ISupportInitialize)(this.pbxDetails)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblTaskWaitState;
        private System.Windows.Forms.Label lblTaskStatus;
        private System.Windows.Forms.Label lblTaskCode;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.PictureBox pbxDetails;
        private LabelControl lblWaitState1;
        private LabelControl lblWaitState2;
        private LabelControl lblWaitState3;
        private LabelControl lblWaitState4;
        private LabelControl lblWaitState5;
        private LabelControl lblWaitState6;
        private LabelControl lblWaitState7;
        private LabelControl lblWaitState8;
        private VerticalLayoutPanel inputPanel;
        private VerticalLayoutPanel outputPanel;
    }
}

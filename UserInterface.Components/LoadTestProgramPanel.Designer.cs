
namespace UserInterface.Controls
{
    partial class LoadTestProgramPanel
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
            this.panelContainer = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lsvTestPrograms = new System.Windows.Forms.ListView();
            this.btnLoad = new UserInterface.Controls.ButtonControl();
            this.btnCancel = new UserInterface.Controls.ButtonControl();
            this.panelContainer.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelContainer
            // 
            this.panelContainer.BackColor = System.Drawing.SystemColors.Info;
            this.panelContainer.Controls.Add(this.flowLayoutPanel1);
            this.panelContainer.Location = new System.Drawing.Point(0, 350);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Size = new System.Drawing.Size(600, 70);
            this.panelContainer.TabIndex = 6;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnLoad);
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(144, 20);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(312, 30);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // lsvTestPrograms
            // 
            this.lsvTestPrograms.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lsvTestPrograms.GridLines = true;
            this.lsvTestPrograms.HideSelection = false;
            this.lsvTestPrograms.HoverSelection = true;
            this.lsvTestPrograms.Location = new System.Drawing.Point(0, 0);
            this.lsvTestPrograms.Name = "lsvTestPrograms";
            this.lsvTestPrograms.Size = new System.Drawing.Size(600, 351);
            this.lsvTestPrograms.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lsvTestPrograms.TabIndex = 7;
            this.lsvTestPrograms.UseCompatibleStateImageBehavior = false;
            // 
            // btnLoad
            // 
            this.btnLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(157)))), ((int)(((byte)(199)))));
            this.btnLoad.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoad.Location = new System.Drawing.Point(3, 3);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(150, 25);
            this.btnLoad.TabIndex = 1;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = false;
            this.btnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
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
            // LoadTestProgramPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lsvTestPrograms);
            this.Controls.Add(this.panelContainer);
            this.Name = "LoadTestProgramPanel";
            this.Size = new System.Drawing.Size(598, 418);
            this.panelContainer.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelContainer;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private ButtonControl btnLoad;
        private ButtonControl btnCancel;
        private System.Windows.Forms.ListView lsvTestPrograms;
    }
}

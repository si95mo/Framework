
namespace UserInterface.Controls
{
    partial class InstructionControl
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
            this.btnAdd = new System.Windows.Forms.Button();
            this.lblInstructionName = new System.Windows.Forms.Label();
            this.lblMethodNamePlaceholder = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnAdd.Location = new System.Drawing.Point(267, 12);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(36, 36);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // lblInstructionName
            // 
            this.lblInstructionName.Location = new System.Drawing.Point(117, 24);
            this.lblInstructionName.Name = "lblInstructionName";
            this.lblInstructionName.Size = new System.Drawing.Size(150, 13);
            this.lblInstructionName.TabIndex = 4;
            this.lblInstructionName.Text = "label2";
            // 
            // lblMethodNamePlaceholder
            // 
            this.lblMethodNamePlaceholder.AutoSize = true;
            this.lblMethodNamePlaceholder.Location = new System.Drawing.Point(2, 24);
            this.lblMethodNamePlaceholder.Name = "lblMethodNamePlaceholder";
            this.lblMethodNamePlaceholder.Size = new System.Drawing.Size(88, 13);
            this.lblMethodNamePlaceholder.TabIndex = 3;
            this.lblMethodNamePlaceholder.Text = "Instruction name:";
            // 
            // InstructionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lblInstructionName);
            this.Controls.Add(this.lblMethodNamePlaceholder);
            this.Name = "InstructionControl";
            this.Size = new System.Drawing.Size(305, 60);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label lblInstructionName;
        private System.Windows.Forms.Label lblMethodNamePlaceholder;
    }
}


namespace UserInterface.Controls
{
    partial class MethodContainer
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
            this.lblMethodNamePlaceholder = new System.Windows.Forms.Label();
            this.lblMethodName = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblMethodNamePlaceholder
            // 
            this.lblMethodNamePlaceholder.AutoSize = true;
            this.lblMethodNamePlaceholder.Location = new System.Drawing.Point(0, 24);
            this.lblMethodNamePlaceholder.Name = "lblMethodNamePlaceholder";
            this.lblMethodNamePlaceholder.Size = new System.Drawing.Size(75, 13);
            this.lblMethodNamePlaceholder.TabIndex = 0;
            this.lblMethodNamePlaceholder.Text = "Method name:";
            // 
            // lblMethodName
            // 
            this.lblMethodName.Location = new System.Drawing.Point(115, 24);
            this.lblMethodName.Name = "lblMethodName";
            this.lblMethodName.Size = new System.Drawing.Size(150, 13);
            this.lblMethodName.TabIndex = 1;
            this.lblMethodName.Text = "label2";
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnAdd.Location = new System.Drawing.Point(265, 12);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(36, 36);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // MethodContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lblMethodName);
            this.Controls.Add(this.lblMethodNamePlaceholder);
            this.Name = "MethodContainer";
            this.Size = new System.Drawing.Size(305, 60);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMethodNamePlaceholder;
        private System.Windows.Forms.Label lblMethodName;
        private System.Windows.Forms.Button btnAdd;
    }
}

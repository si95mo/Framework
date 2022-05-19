
namespace UserInterface.Controls
{
    partial class MethodControl
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
            this.lblMethodName = new System.Windows.Forms.Label();
            this.btnInvoke = new UserInterface.Controls.ButtonControl();
            this.SuspendLayout();
            // 
            // lblMethodName
            // 
            this.lblMethodName.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMethodName.Location = new System.Drawing.Point(5, 35);
            this.lblMethodName.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblMethodName.Name = "lblMethodName";
            this.lblMethodName.Size = new System.Drawing.Size(207, 20);
            this.lblMethodName.TabIndex = 1;
            this.lblMethodName.Text = "label2";
            // 
            // btnInvoke
            // 
            this.btnInvoke.BackColor = System.Drawing.Color.Transparent;
            this.btnInvoke.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnInvoke.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInvoke.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInvoke.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnInvoke.Image = global::UserInterface.Controls.Properties.Resources.Start;
            this.btnInvoke.Location = new System.Drawing.Point(533, 18);
            this.btnInvoke.Name = "btnInvoke";
            this.btnInvoke.Size = new System.Drawing.Size(60, 55);
            this.btnInvoke.TabIndex = 3;
            this.btnInvoke.UseVisualStyleBackColor = false;
            this.btnInvoke.Click += new System.EventHandler(this.BtnInvoke_Click);
            // 
            // MethodControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnInvoke);
            this.Controls.Add(this.lblMethodName);
            this.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "MethodControl";
            this.Size = new System.Drawing.Size(602, 94);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblMethodName;
        private ButtonControl btnInvoke;
    }
}


namespace UserInterface.Controls
{
    partial class DigitalControl
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
            this.panel = new System.Windows.Forms.Panel();
            this.btnValue = new UserInterface.Controls.ButtonControl();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel.Controls.Add(this.btnValue);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel.MaximumSize = new System.Drawing.Size(128, 32);
            this.panel.MinimumSize = new System.Drawing.Size(128, 32);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(128, 32);
            this.panel.TabIndex = 0;
            this.panel.Click += new System.EventHandler(this.Control_Cick);
            // 
            // btnValue
            // 
            this.btnValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnValue.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnValue.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnValue.Location = new System.Drawing.Point(-1, -1);
            this.btnValue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnValue.MaximumSize = new System.Drawing.Size(64, 32);
            this.btnValue.MinimumSize = new System.Drawing.Size(64, 32);
            this.btnValue.Name = "btnValue";
            this.btnValue.Size = new System.Drawing.Size(64, 32);
            this.btnValue.TabIndex = 0;
            this.btnValue.UseVisualStyleBackColor = false;
            this.btnValue.Click += new System.EventHandler(this.Control_Cick);
            // 
            // DigitalControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel);
            this.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(128, 32);
            this.MinimumSize = new System.Drawing.Size(128, 32);
            this.Name = "DigitalControl";
            this.Size = new System.Drawing.Size(128, 32);
            this.panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private ButtonControl btnValue;
    }
}

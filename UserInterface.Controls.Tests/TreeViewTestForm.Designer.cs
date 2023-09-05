namespace UserInterface.Controls.Tests
{
    partial class TreeViewTestForm
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
            this.treeViewControl = new UserInterface.Controls.TreeViewControl();
            this.SuspendLayout();
            // 
            // treeViewControl
            // 
            this.treeViewControl.BackColor = System.Drawing.SystemColors.Control;
            this.treeViewControl.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.treeViewControl.Location = new System.Drawing.Point(12, 12);
            this.treeViewControl.Name = "treeViewControl";
            this.treeViewControl.Size = new System.Drawing.Size(233, 278);
            this.treeViewControl.TabIndex = 0;
            // 
            // TreeViewTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 302);
            this.Controls.Add(this.treeViewControl);
            this.Name = "TreeViewTestForm";
            this.Text = "TreeViewTestForm";
            this.ResumeLayout(false);

        }

        #endregion

        private TreeViewControl treeViewControl;
    }
}
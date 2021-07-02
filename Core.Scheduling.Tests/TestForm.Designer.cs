
namespace Core.Scheduling.Tests
{
    partial class TestForm
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
            this.txbConsole = new System.Windows.Forms.TextBox();
            this.panelMethods = new System.Windows.Forms.FlowLayoutPanel();
            this.lbxInput = new System.Windows.Forms.ListBox();
            this.lbxOutput = new System.Windows.Forms.ListBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnClean = new UserInterface.Controls.ButtonControl();
            this.btnExecute = new UserInterface.Controls.ButtonControl();
            this.btnLoadTest = new UserInterface.Controls.ButtonControl();
            this.btnSaveTest = new UserInterface.Controls.ButtonControl();
            this.btnClose = new UserInterface.Controls.ButtonControl();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txbConsole
            // 
            this.txbConsole.Location = new System.Drawing.Point(370, 538);
            this.txbConsole.Multiline = true;
            this.txbConsole.Name = "txbConsole";
            this.txbConsole.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txbConsole.Size = new System.Drawing.Size(781, 530);
            this.txbConsole.TabIndex = 6;
            this.txbConsole.DoubleClick += new System.EventHandler(this.TxbConsole_DoubleClick);
            // 
            // panelMethods
            // 
            this.panelMethods.AutoScroll = true;
            this.panelMethods.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.panelMethods.Location = new System.Drawing.Point(13, 12);
            this.panelMethods.Name = "panelMethods";
            this.panelMethods.Size = new System.Drawing.Size(351, 1056);
            this.panelMethods.TabIndex = 12;
            this.panelMethods.WrapContents = false;
            // 
            // lbxInput
            // 
            this.lbxInput.FormattingEnabled = true;
            this.lbxInput.HorizontalScrollbar = true;
            this.lbxInput.Location = new System.Drawing.Point(370, 50);
            this.lbxInput.Name = "lbxInput";
            this.lbxInput.Size = new System.Drawing.Size(781, 238);
            this.lbxInput.TabIndex = 0;
            // 
            // lbxOutput
            // 
            this.lbxOutput.FormattingEnabled = true;
            this.lbxOutput.HorizontalScrollbar = true;
            this.lbxOutput.Location = new System.Drawing.Point(370, 294);
            this.lbxOutput.Name = "lbxOutput";
            this.lbxOutput.Size = new System.Drawing.Size(781, 238);
            this.lbxOutput.TabIndex = 1;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnClean);
            this.flowLayoutPanel1.Controls.Add(this.btnExecute);
            this.flowLayoutPanel1.Controls.Add(this.btnLoadTest);
            this.flowLayoutPanel1.Controls.Add(this.btnSaveTest);
            this.flowLayoutPanel1.Controls.Add(this.btnClose);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(370, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(781, 31);
            this.flowLayoutPanel1.TabIndex = 11;
            // 
            // btnClean
            // 
            this.btnClean.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(157)))), ((int)(((byte)(199)))));
            this.btnClean.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnClean.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClean.Location = new System.Drawing.Point(3, 3);
            this.btnClean.Name = "btnClean";
            this.btnClean.Size = new System.Drawing.Size(150, 25);
            this.btnClean.TabIndex = 11;
            this.btnClean.Text = "Clear";
            this.btnClean.UseVisualStyleBackColor = false;
            this.btnClean.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // btnExecute
            // 
            this.btnExecute.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(157)))), ((int)(((byte)(199)))));
            this.btnExecute.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnExecute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExecute.Location = new System.Drawing.Point(159, 3);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(150, 25);
            this.btnExecute.TabIndex = 12;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = false;
            this.btnExecute.Click += new System.EventHandler(this.BtnExecute_Click);
            // 
            // btnLoadTest
            // 
            this.btnLoadTest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(157)))), ((int)(((byte)(199)))));
            this.btnLoadTest.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnLoadTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadTest.Location = new System.Drawing.Point(315, 3);
            this.btnLoadTest.Name = "btnLoadTest";
            this.btnLoadTest.Size = new System.Drawing.Size(150, 25);
            this.btnLoadTest.TabIndex = 13;
            this.btnLoadTest.Text = "Load test";
            this.btnLoadTest.UseVisualStyleBackColor = false;
            this.btnLoadTest.Click += new System.EventHandler(this.BtnLoadTest_Click);
            // 
            // btnSaveTest
            // 
            this.btnSaveTest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(157)))), ((int)(((byte)(199)))));
            this.btnSaveTest.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnSaveTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveTest.Location = new System.Drawing.Point(471, 3);
            this.btnSaveTest.Name = "btnSaveTest";
            this.btnSaveTest.Size = new System.Drawing.Size(150, 25);
            this.btnSaveTest.TabIndex = 14;
            this.btnSaveTest.Text = "Save test";
            this.btnSaveTest.UseVisualStyleBackColor = false;
            this.btnSaveTest.Click += new System.EventHandler(this.BtnSaveTest_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(157)))), ((int)(((byte)(199)))));
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(627, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(150, 25);
            this.btnClose.TabIndex = 15;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.panelMethods);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.txbConsole);
            this.Controls.Add(this.lbxOutput);
            this.Controls.Add(this.lbxInput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TestForm";
            this.Text = "Test";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txbConsole;
        private System.Windows.Forms.FlowLayoutPanel panelMethods;
        private System.Windows.Forms.ListBox lbxInput;
        private System.Windows.Forms.ListBox lbxOutput;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private UserInterface.Controls.ButtonControl btnClean;
        private UserInterface.Controls.ButtonControl btnExecute;
        private UserInterface.Controls.ButtonControl btnLoadTest;
        private UserInterface.Controls.ButtonControl btnSaveTest;
        private UserInterface.Controls.ButtonControl btnClose;
    }
}
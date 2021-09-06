
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
            this.btnLoadProgram = new UserInterface.Controls.ButtonControl();
            this.btnSaveProgram = new UserInterface.Controls.ButtonControl();
            this.btnClose = new UserInterface.Controls.ButtonControl();
            this.panelInstructions = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txbConsole
            // 
            this.txbConsole.BackColor = System.Drawing.SystemColors.InfoText;
            this.txbConsole.ForeColor = System.Drawing.SystemColors.Info;
            this.txbConsole.Location = new System.Drawing.Point(727, 537);
            this.txbConsole.Multiline = true;
            this.txbConsole.Name = "txbConsole";
            this.txbConsole.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txbConsole.Size = new System.Drawing.Size(781, 530);
            this.txbConsole.TabIndex = 6;
            this.txbConsole.TextChanged += new System.EventHandler(this.txbConsole_TextChanged);
            this.txbConsole.DoubleClick += new System.EventHandler(this.TxbConsole_DoubleClick);
            // 
            // panelMethods
            // 
            this.panelMethods.AutoScroll = true;
            this.panelMethods.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
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
            this.lbxInput.Location = new System.Drawing.Point(727, 49);
            this.lbxInput.Name = "lbxInput";
            this.lbxInput.Size = new System.Drawing.Size(781, 238);
            this.lbxInput.TabIndex = 0;
            this.lbxInput.SelectedIndexChanged += new System.EventHandler(this.lbxInput_SelectedIndexChanged);
            // 
            // lbxOutput
            // 
            this.lbxOutput.FormattingEnabled = true;
            this.lbxOutput.HorizontalScrollbar = true;
            this.lbxOutput.Location = new System.Drawing.Point(727, 293);
            this.lbxOutput.Name = "lbxOutput";
            this.lbxOutput.Size = new System.Drawing.Size(781, 238);
            this.lbxOutput.TabIndex = 1;
            this.lbxOutput.SelectedIndexChanged += new System.EventHandler(this.lbxOutput_SelectedIndexChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnClean);
            this.flowLayoutPanel1.Controls.Add(this.btnExecute);
            this.flowLayoutPanel1.Controls.Add(this.btnLoadProgram);
            this.flowLayoutPanel1.Controls.Add(this.btnSaveProgram);
            this.flowLayoutPanel1.Controls.Add(this.btnClose);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(727, 11);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(781, 31);
            this.flowLayoutPanel1.TabIndex = 11;
            this.flowLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.flowLayoutPanel1_Paint);
            // 
            // btnClean
            // 
            this.btnClean.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(157)))), ((int)(((byte)(199)))));
            this.btnClean.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnClean.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClean.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.btnExecute.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExecute.Location = new System.Drawing.Point(159, 3);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(150, 25);
            this.btnExecute.TabIndex = 12;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = false;
            this.btnExecute.Click += new System.EventHandler(this.BtnExecute_Click);
            // 
            // btnLoadProgram
            // 
            this.btnLoadProgram.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(157)))), ((int)(((byte)(199)))));
            this.btnLoadProgram.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnLoadProgram.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadProgram.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadProgram.Location = new System.Drawing.Point(315, 3);
            this.btnLoadProgram.Name = "btnLoadProgram";
            this.btnLoadProgram.Size = new System.Drawing.Size(150, 25);
            this.btnLoadProgram.TabIndex = 13;
            this.btnLoadProgram.Text = "Load program";
            this.btnLoadProgram.UseVisualStyleBackColor = false;
            this.btnLoadProgram.Click += new System.EventHandler(this.BtnLoadProgram_Click);
            // 
            // btnSaveProgram
            // 
            this.btnSaveProgram.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(157)))), ((int)(((byte)(199)))));
            this.btnSaveProgram.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnSaveProgram.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveProgram.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveProgram.Location = new System.Drawing.Point(471, 3);
            this.btnSaveProgram.Name = "btnSaveProgram";
            this.btnSaveProgram.Size = new System.Drawing.Size(150, 25);
            this.btnSaveProgram.TabIndex = 14;
            this.btnSaveProgram.Text = "Save program";
            this.btnSaveProgram.UseVisualStyleBackColor = false;
            this.btnSaveProgram.Click += new System.EventHandler(this.BtnSaveProgram_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(157)))), ((int)(((byte)(199)))));
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(627, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(150, 25);
            this.btnClose.TabIndex = 15;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // panelInstructions
            // 
            this.panelInstructions.AutoScroll = true;
            this.panelInstructions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelInstructions.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.panelInstructions.Location = new System.Drawing.Point(370, 12);
            this.panelInstructions.Name = "panelInstructions";
            this.panelInstructions.Size = new System.Drawing.Size(351, 1056);
            this.panelInstructions.TabIndex = 13;
            this.panelInstructions.WrapContents = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1514, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Input";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1514, 293);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Output";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1514, 537);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Console";
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panelInstructions);
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
        private UserInterface.Controls.ButtonControl btnLoadProgram;
        private UserInterface.Controls.ButtonControl btnSaveProgram;
        private UserInterface.Controls.ButtonControl btnClose;
        private System.Windows.Forms.FlowLayoutPanel panelInstructions;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}
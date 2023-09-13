
using UserInterface.Controls.Panels;

namespace UserInterface.Controls.Tests
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvControl = new UserInterface.Controls.DataGridControl();
            this.progressBar = new CircularProgressBar();
            this.ledControl = new UserInterface.Controls.LedControl();
            this.btnCustomMessageBox = new UserInterface.Controls.ButtonControl();
            this.panelControl = new UserInterface.Controls.Panels.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.dgvControl)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFormName
            // 
            this.LblFormName.Margin = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.LblFormName.Size = new System.Drawing.Size(145, 25);
            this.LblFormName.Text = "CustomForm";
            // 
            // controlBox
            // 
            this.ControlBox.Location = new System.Drawing.Point(631, 14);
            // 
            // dgvControl
            // 
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.dgvControl.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvControl.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvControl.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvControl.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(220)))), ((int)(((byte)(4)))));
            this.dgvControl.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvControl.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            this.dgvControl.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(220)))), ((int)(((byte)(4)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvControl.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvControl.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(52)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvControl.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgvControl.EnableHeadersVisualStyles = false;
            this.dgvControl.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.dgvControl.Location = new System.Drawing.Point(355, 48);
            this.dgvControl.Margin = new System.Windows.Forms.Padding(5);
            this.dgvControl.Name = "dgvControl";
            this.dgvControl.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(220)))), ((int)(((byte)(4)))));
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvControl.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvControl.Size = new System.Drawing.Size(375, 142);
            this.dgvControl.TabIndex = 2;
            // 
            // progressBar
            // 
            this.progressBar.BackColor = System.Drawing.SystemColors.Control;
            this.progressBar.FirstBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(220)))), ((int)(((byte)(4)))));
            this.progressBar.SecondBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(220)))), ((int)(((byte)(4)))));
            this.progressBar.BarWidth = 14F;
            this.progressBar.Font = new System.Drawing.Font("Lucida Sans Unicode", 14F);
            this.progressBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.progressBar.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.progressBar.LineColor = System.Drawing.Color.DimGray;
            this.progressBar.LineWidth = 1;
            this.progressBar.Location = new System.Drawing.Point(178, 23);
            this.progressBar.Margin = new System.Windows.Forms.Padding(5);
            this.progressBar.Maximum = ((long)(100));
            this.progressBar.MinimumSize = new System.Drawing.Size(167, 154);
            this.progressBar.Name = "progressBar";
            this.progressBar.ProgressBarShape = CircularProgressBar.ProgressShape.Flat;
            this.progressBar.ProgressBarTextMode = CircularProgressBar.TextMode.Percentage;
            this.progressBar.Size = new System.Drawing.Size(167, 167);
            this.progressBar.TabIndex = 1;
            this.progressBar.Value = ((long)(0));
            // 
            // ledControl
            // 
            this.ledControl.Location = new System.Drawing.Point(21, 65);
            this.ledControl.Margin = new System.Windows.Forms.Padding(5);
            this.ledControl.Name = "ledControl";
            this.ledControl.On = true;
            this.ledControl.Size = new System.Drawing.Size(78, 70);
            this.ledControl.TabIndex = 0;
            this.ledControl.Text = "ledControl";
            // 
            // btnCustomMessageBox
            // 
            this.btnCustomMessageBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnCustomMessageBox.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnCustomMessageBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCustomMessageBox.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomMessageBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnCustomMessageBox.Location = new System.Drawing.Point(12, 198);
            this.btnCustomMessageBox.Name = "btnCustomMessageBox";
            this.btnCustomMessageBox.Size = new System.Drawing.Size(280, 60);
            this.btnCustomMessageBox.TabIndex = 12;
            this.btnCustomMessageBox.Text = "Show CustomMessageBox";
            this.btnCustomMessageBox.UseVisualStyleBackColor = false;
            this.btnCustomMessageBox.Click += new System.EventHandler(this.BtnShowCustomMessageBox_Click);
            // 
            // panelControl
            // 
            this.panelControl.BackColor = System.Drawing.SystemColors.Control;
            this.panelControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelControl.Enabled = false;
            this.panelControl.Location = new System.Drawing.Point(12, 266);
            this.panelControl.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(719, 216);
            this.panelControl.TabIndex = 13;
            this.panelControl.Value = null;
            this.panelControl.Visible = false;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 496);
            this.Controls.Add(this.panelControl);
            this.Controls.Add(this.btnCustomMessageBox);
            this.Controls.Add(this.dgvControl);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.ledControl);
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.Load += new System.EventHandler(this.TestForm_Load);
            this.Controls.SetChildIndex(this.ledControl, 0);
            this.Controls.SetChildIndex(this.progressBar, 0);
            this.Controls.SetChildIndex(this.dgvControl, 0);
            this.Controls.SetChildIndex(this.LblFormName, 0);
            this.Controls.SetChildIndex(this.ControlBox, 0);
            this.Controls.SetChildIndex(this.btnCustomMessageBox, 0);
            this.Controls.SetChildIndex(this.panelControl, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgvControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LedControl ledControl;
        private CircularProgressBar progressBar;
        private DataGridControl dgvControl;
        private ButtonControl btnCustomMessageBox;
        private PanelControl panelControl;
    }
}
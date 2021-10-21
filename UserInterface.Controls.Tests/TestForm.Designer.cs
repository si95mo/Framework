
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvControl = new UserInterface.Controls.DataGridControl();
            this.progressBar = new CircularProgressBar();
            this.ledControl = new UserInterface.Controls.LedControl();
            this.btnCustomMessageBox = new UserInterface.Controls.ButtonControl();
            ((System.ComponentModel.ISupportInitialize)(this.dgvControl)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFormName
            // 
            this.lblFormName.Margin = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.lblFormName.Size = new System.Drawing.Size(145, 25);
            this.lblFormName.Text = "CustomForm";
            // 
            // controlBox
            // 
            this.controlBox.Location = new System.Drawing.Point(499, 14);
            // 
            // dgvControl
            // 
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.dgvControl.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvControl.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvControl.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvControl.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(220)))), ((int)(((byte)(4)))));
            this.dgvControl.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvControl.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            this.dgvControl.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(220)))), ((int)(((byte)(4)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvControl.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvControl.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(52)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvControl.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvControl.EnableHeadersVisualStyles = false;
            this.dgvControl.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.dgvControl.Location = new System.Drawing.Point(21, 242);
            this.dgvControl.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.dgvControl.Name = "dgvControl";
            this.dgvControl.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(220)))), ((int)(((byte)(4)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvControl.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvControl.Size = new System.Drawing.Size(577, 133);
            this.dgvControl.TabIndex = 2;
            // 
            // progressBar
            // 
            this.progressBar.BackColor = System.Drawing.SystemColors.Control;
            this.progressBar.BarColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(220)))), ((int)(((byte)(4)))));
            this.progressBar.BarColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(220)))), ((int)(((byte)(4)))));
            this.progressBar.BarWidth = 14F;
            this.progressBar.Font = new System.Drawing.Font("Lucida Sans Unicode", 14F);
            this.progressBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.progressBar.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.progressBar.LineColor = System.Drawing.Color.DimGray;
            this.progressBar.LineWidth = 1;
            this.progressBar.Location = new System.Drawing.Point(431, 65);
            this.progressBar.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
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
            this.ledControl.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
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
            this.btnCustomMessageBox.Location = new System.Drawing.Point(21, 383);
            this.btnCustomMessageBox.Name = "btnCustomMessageBox";
            this.btnCustomMessageBox.Size = new System.Drawing.Size(280, 60);
            this.btnCustomMessageBox.TabIndex = 12;
            this.btnCustomMessageBox.Text = "Show CustomMessageBox";
            this.btnCustomMessageBox.UseVisualStyleBackColor = false;
            this.btnCustomMessageBox.Click += new System.EventHandler(this.BtnShowCustomMessageBox_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 515);
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
            this.Controls.SetChildIndex(this.lblFormName, 0);
            this.Controls.SetChildIndex(this.controlBox, 0);
            this.Controls.SetChildIndex(this.btnCustomMessageBox, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgvControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LedControl ledControl;
        private CircularProgressBar progressBar;
        private DataGridControl dgvControl;
        private ButtonControl btnCustomMessageBox;
    }
}
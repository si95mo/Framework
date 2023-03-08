namespace UserInterface.Controls.Tests
{
    partial class HardwareControlsForm
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
            this.tbControl = new System.Windows.Forms.TabControl();
            this.tbChannels = new System.Windows.Forms.TabPage();
            this.tbResources = new System.Windows.Forms.TabPage();
            this.resourceFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.channelsLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.tbControl.SuspendLayout();
            this.tbChannels.SuspendLayout();
            this.tbResources.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbControl
            // 
            this.tbControl.Controls.Add(this.tbResources);
            this.tbControl.Controls.Add(this.tbChannels);
            this.tbControl.Location = new System.Drawing.Point(3, 1);
            this.tbControl.Name = "tbControl";
            this.tbControl.SelectedIndex = 0;
            this.tbControl.Size = new System.Drawing.Size(1426, 814);
            this.tbControl.TabIndex = 0;
            // 
            // tbChannels
            // 
            this.tbChannels.Controls.Add(this.channelsLayoutPanel);
            this.tbChannels.Location = new System.Drawing.Point(4, 22);
            this.tbChannels.Name = "tbChannels";
            this.tbChannels.Padding = new System.Windows.Forms.Padding(3);
            this.tbChannels.Size = new System.Drawing.Size(1418, 788);
            this.tbChannels.TabIndex = 1;
            this.tbChannels.Text = "Channels";
            this.tbChannels.UseVisualStyleBackColor = true;
            // 
            // tbResources
            // 
            this.tbResources.Controls.Add(this.resourceFlowLayout);
            this.tbResources.Location = new System.Drawing.Point(4, 22);
            this.tbResources.Name = "tbResources";
            this.tbResources.Padding = new System.Windows.Forms.Padding(3);
            this.tbResources.Size = new System.Drawing.Size(1418, 788);
            this.tbResources.TabIndex = 0;
            this.tbResources.Text = "Resources";
            this.tbResources.UseVisualStyleBackColor = true;
            // 
            // resourceFlowLayout
            // 
            this.resourceFlowLayout.AutoScroll = true;
            this.resourceFlowLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.resourceFlowLayout.Location = new System.Drawing.Point(7, 7);
            this.resourceFlowLayout.Name = "resourceFlowLayout";
            this.resourceFlowLayout.Size = new System.Drawing.Size(1408, 775);
            this.resourceFlowLayout.TabIndex = 0;
            this.resourceFlowLayout.WrapContents = false;
            // 
            // channelsLayoutPanel
            // 
            this.channelsLayoutPanel.AutoScroll = true;
            this.channelsLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.channelsLayoutPanel.Location = new System.Drawing.Point(6, 7);
            this.channelsLayoutPanel.Name = "channelsLayoutPanel";
            this.channelsLayoutPanel.Size = new System.Drawing.Size(1408, 775);
            this.channelsLayoutPanel.TabIndex = 1;
            this.channelsLayoutPanel.WrapContents = false;
            // 
            // HardwareControlsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1429, 818);
            this.Controls.Add(this.tbControl);
            this.Name = "HardwareControlsForm";
            this.Text = "HardwareControlsForm";
            this.tbControl.ResumeLayout(false);
            this.tbChannels.ResumeLayout(false);
            this.tbResources.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbControl;
        private System.Windows.Forms.TabPage tbChannels;
        private System.Windows.Forms.TabPage tbResources;
        private System.Windows.Forms.FlowLayoutPanel resourceFlowLayout;
        private System.Windows.Forms.FlowLayoutPanel channelsLayoutPanel;
    }
}
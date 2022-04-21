using Extensions;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace UserInterface.Dashboards
{
    /// <summary>
    /// Implement a draggable control
    /// </summary>
    public partial class DraggableControl : UserControl
    {
        /// <summary>
        /// <see langword="true"/> if the <see cref="DraggableControl"/> is draggable,
        /// <see langword="false"/> otherwise
        /// </summary>
        public bool IsDraggable 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Create a <see cref="DraggableControl"/>
        /// </summary>
        public DraggableControl()
        {
            InitializeComponent();

            IsDraggable = false;
        }

        private void DraggableControl_Load(object sender, EventArgs e)
            => this.SetDraggable(true);

        private void Control_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Cancel || e.KeyCode == Keys.Delete)
            {
                if (Parent != null)
                {
                    if (Parent.Parent.GetType() != typeof(DashboardItemControl))
                    {
                        if (IsDraggable)
                        {
                            Parent.Controls.Remove(this);
                            Dispose();
                        }
                    }
                }
            }
        }

        private void Control_MouseDown(object sender, MouseEventArgs e)
            => BackColor = SystemColors.ControlLight;

        private void Control_MouseUp(object sender, MouseEventArgs e)
            => BackColor = SystemColors.Control;
    }
}
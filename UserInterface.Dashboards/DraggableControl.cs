using Hardware;
using System;
using System.Windows.Forms;

namespace UserInterface.Dashboards
{
    /// <summary>
    /// Implement a draggable control
    /// </summary>
    public partial class DraggableControl : UserControl
    {
        /// <summary>
        /// Create a <see cref="DraggableControl"/>
        /// </summary>
        protected DraggableControl()
        {
            InitializeComponent();
        }

        private void DraggableControl_Load(object sender, EventArgs e)
            => this.Draggable(true);
    }
}

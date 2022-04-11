using Extensions;
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
        /// <see langword="true"/> if the <see cref="DraggableControl"/> is draggable,
        /// <see langword="false"/> otherwise
        /// </summary>
        public bool IsDraggable { get; set; }

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
    }
}
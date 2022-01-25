using System.Drawing;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    /// <summary>
    /// Class that implements a control that automatically display all of its 
    /// children control by applying a vertical layout (from top to bottom).
    /// See also <see cref="FlowLayoutPanel"/>
    /// </summary>
    public partial class VerticalLayoutPanel : FlowLayoutPanel
    {
        /// <summary>
        /// The vertical scrollbar horizontal size
        /// </summary>
        internal const int SCROLLBAR_HORIZONTAL_SIZE = 23;

        /// <summary>
        /// Get the auto scroll option
        /// </summary>
        public new bool AutoScroll { get; }

        /// <summary>
        /// Get the <see cref="System.Windows.FlowDirection"/> of the control
        /// </summary>
        public new FlowDirection FlowDirection { get; }

        /// <summary>
        /// Get the wrap contents option
        /// </summary>
        public new bool WrapContents { get; }

        public VerticalLayoutPanel()
        {
            InitializeComponent();

            // Set the properties in order to have a vertical layout
            AutoScroll = true;
            FlowDirection = FlowDirection.TopDown;
            WrapContents = false;
        }

        /// <summary>
        /// Add a <see cref="Control"/> to the controls collection and resize it
        /// to fit horizontally
        /// </summary>
        /// <param name="control">The <see cref="Control"/> to add</param>
        /// <remarks>
        /// This method only resize the horizontal size of the <see cref="Control"/>,
        /// so the vertical value must be already set before the call of this method!
        /// </remarks>
        public void AddControl(Control control)
        {
            // Resize the control
            // Note: 23 is the vertical scrollbar horizontal size
            control.Size = new Size(Size.Width - SCROLLBAR_HORIZONTAL_SIZE, control.Size.Height);

            // Then add the resized control
            Controls.Add(control);
        }
    }
}

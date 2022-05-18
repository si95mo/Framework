using Core;
using Core.Conditions;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    /// <summary>
    /// Create an <see cref="UserControl"/> for <see cref="ICondition"/>
    /// </summary>
    public partial class ConditionControl : UserControl
    {
        private ICondition condition;
        private Panel parent;

        private ConditionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Create a new instance of <see cref="ConditionControl"/>
        /// </summary>
        /// <remarks>
        /// It's necessary to pass the <paramref name="parent"/> container as a parameter
        /// in order to correctly resize the <see cref="ConditionControl"/>!
        /// </remarks>
        /// <param name="condition">The <see cref="ICondition"/></param>
        /// <param name="parent">The <see cref="ChannelControl"/> parent <see cref="Panel"/></param>
        public ConditionControl(ICondition condition, Panel parent = null) : this()
        {
            this.condition = condition;
            this.parent = parent;

            lblCode.Text = condition.Code;
            ledControl.Color = condition.Value ? Color.Black : SystemColors.Control;
        }

        private void ConditionControl_Load(object sender, EventArgs e)
        {
            if (parent != null)
            {
                int width = parent.Size.Width - SystemInformation.VerticalScrollBarWidth - 4;
                int heigth = Size.Height;
                Size = new Size(width, heigth);
            }

            condition.ValueChanged += Condition_ValueChanged;
        }

        private void Condition_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            ledControl.Color = condition.Value ? Color.Black : SystemColors.Control;
        }
    }
}

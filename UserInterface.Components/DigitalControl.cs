using Core;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    /// <summary>
    /// Define a control for handling digital inputs.
    /// See <see cref="BaseControl"/>
    /// </summary>
    public partial class DigitalControl : BaseControl
    {
        private object objectLock = new object();

        /// <summary>
        /// The <see cref="DigitalControl"/> value
        /// </summary>
        public override object Value
        {
            get => (bool)value;
            set
            {
                if (!value.Equals(this.value))
                {
                    object oldValue = this.value;
                    this.value = value;
                    OnValueChanged(new ValueChangedEventArgs(oldValue, this.value));
                }
            }
        }

        /// <summary>
        /// The value changed event handler
        /// </summary>
        protected EventHandler<ValueChangedEventArgs> ValueChangedHandler;

        /// <summary>
        /// Create a new instance of <see cref="DigitalControl"/>
        /// </summary>
        public DigitalControl()
        {
            InitializeComponent();

            value = false;

            btnValue.BackColor = Colors.TextColor;
            btnValue.ForeColor = Colors.Grey;
        }

        /// <summary>
        /// Handle the click event
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="EventArgs"/></param>
        private void Control_Cick(object sender, EventArgs e)
        {
            Point p;
            string text;

            if (btnValue.Location.X == -1)
            {
                p = new Point(panel.Size.Width - btnValue.Size.Width, -1);
                text = "True";
                panel.BackColor = Colors.Green;
                btnValue.ForeColor = Colors.Green;
            }
            else
            {
                p = new Point(-1, -1);
                text = "False";
                panel.BackColor = Colors.Grey;
                btnValue.ForeColor = Colors.Grey;
            }

            // Trigger the value changed event
            Value = !(bool)value;

            btnValue.Location = p;
            btnValue.Text = text;
        }

        /// <summary>
        /// The on paint event handler
        /// </summary>
        /// <param name="e">THe <see cref="PaintEventArgs"/></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        /// <summary>
        /// On value changed event
        /// </summary>
        /// <param name="e">The <see cref="ValueChangedEventArgs"/></param>
        protected virtual void OnValueChanged(ValueChangedEventArgs e)
        {
            ValueChangedHandler?.Invoke(this, e);
        }

        /// <summary>
        /// The <see cref="ValueChanged"/> event handler
        /// for the <see cref="Value"/> property
        /// </summary>
        public event EventHandler<ValueChangedEventArgs> ValueChanged
        {
            add
            {
                lock (objectLock)
                {
                    ValueChangedHandler += value;
                }
            }

            remove
            {
                lock (objectLock)
                {
                    ValueChangedHandler -= value;
                }
            }
        }
    }
}
using Core;
using Core.Parameters;
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
        private const string True = "T";
        private const string False = "F";

        private object objectLock = new object();

        private BoolParameter parameter;

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

            Value = false;
            UpdatePosition();

            btnValue.BackColor = Colors.TextColor;
            btnValue.ForeColor = Colors.Grey;

            Size = new Size(128, 32);
        }

        /// <summary>
        /// Handle the click event
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="EventArgs"/></param>
        private void Control_Cick(object sender, EventArgs e)
        {
            // Trigger the value changed event
            Value = !(bool)value;

            UpdatePosition();
        }

        /// <summary>
        /// Update the <see cref="DigitalControl"/> position
        /// </summary>
        private void UpdatePosition()
        {
            Point point;
            string text;

            if ((bool)Value)
            {
                point = new Point(panel.Size.Width - btnValue.Size.Width, -1);
                //text = True;
                panel.BackColor = Colors.Green;
                btnValue.ForeColor = Colors.Green;
            }
            else
            {
                point = new Point(-1, -1);
                //text = False;
                panel.BackColor = Colors.Grey;
                btnValue.ForeColor = Colors.Grey;
            }

            btnValue.Location = point;
            //btnValue.Text = text;
        }

        /// <summary>
        /// Update the <see cref="DigitalControl"/> position
        /// </summary>
        /// <param name="value">The new value</param>
        private void UpdatePosition(bool value)
        {
            Point point;
            string text;

            if (value) // New value is true (from false)
            {
                point = new Point(panel.Size.Width - btnValue.Size.Width, -1);
                //text = True;
                panel.BackColor = Colors.Green;
                btnValue.ForeColor = Colors.Green;
            }
            else // New value is false (from true)
            {
                point = new Point(-1, -1);
                //text = False;
                panel.BackColor = Colors.Grey;
                btnValue.ForeColor = Colors.Grey;
            }

            btnValue.Location = point;
            //btnValue.Text = text;
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

        /// <summary>
        /// Set the <see cref="BoolParameter"/> that will be connected to the <see cref="DigitalControl"/>
        /// </summary>
        /// <param name="parameter">The <see cref="BoolParameter"/> to connect</param>
        public void SetParameter(BoolParameter parameter)
        {
            this.parameter = new BoolParameter($"DigitalControl.BoolParameter.{parameter.Code}", parameter.Value);
            this.parameter.ConnectTo(parameter);

            ValueChanged += DigitalControl_ValueChanged;
            parameter.ValueChanged += Parameter_ValueChanged;
        }

        private void DigitalControl_ValueChanged(object sender, ValueChangedEventArgs e)
            => parameter.Value = e.NewValueAsBool;

        private void Parameter_ValueChanged(object sender, ValueChangedEventArgs e)
            => UpdatePosition(e.NewValueAsBool);

        protected override void OnPaint(PaintEventArgs e)
        {
            panel.Size = Size;
            btnValue.Size = new Size(Size.Width / 2, Size.Height);

            base.OnPaint(e);
        }
    }
}
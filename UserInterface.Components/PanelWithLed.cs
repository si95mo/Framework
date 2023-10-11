using Core.Conditions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    /// <summary>
    /// Define a basic panel with a led
    /// </summary>
    public partial class PanelWithLed : UserControl
    {
        private Color indicatorColor = Colors.Grey;
        private string title = "Title";
        private Color titleForeColor = Colors.TextColor;
        private (ICondition Condition, Color Color)[] ledConfiguration;

        [Category("Properties"), Description("Indicator color"), DefaultValue(typeof(Color), "0xC8C8C8")]
        public Color IndicatorColor
        {
            get { return indicatorColor; }
            set { indicatorColor = value; Invalidate(); }
        }

        [Category("Properties"), Description("Title field"), DefaultValue("Title")]
        public string Title
        {
            get { return title; }
            set { title = value; Invalidate(); }
        }

        /// <summary>
        /// Create a new instance of <see cref="PanelWithLed"/>
        /// </summary>
        public PanelWithLed()
        {
            InitializeComponent();

            BackColor = Color.White;
            Font = new Font("Lucida Sans Unicode", 12, FontStyle.Regular);
            AutoScaleMode = AutoScaleMode.Inherit;

            MaximumSize = new Size(0, 0);
            MinimumSize = new Size(200, 30);
            Size = new Size(120, 120);
        }

        #region Public methods

        /// <summary>
        /// Initialize the led with a set composed of an <see cref="ICondition"/> that indicates the status and the relative <see cref="Color"/>
        /// </summary>
        /// <param name="ledConfiguration">The configuration set of (<see cref="ICondition"/>, <see cref="Color"/>)</param>
        public void InitializeLed(params (ICondition Condition, Color Color)[] ledConfiguration)
        {
            this.ledConfiguration?.Select((x) => x.Condition).ToList().ForEach((x) => x.ValueChanged -= Condition_ValueChanged);
            this.ledConfiguration = ledConfiguration.Where(x => x.Condition != null).ToArray();

            UpdateLed();
            this.ledConfiguration.Select((x) => x.Condition).ToList().ForEach((x) => x.ValueChanged += Condition_ValueChanged);
        }

        #endregion Public methods

        #region Protected methods

        protected override void OnEnabledChanged(EventArgs e)
        {
            if (Enabled)
                titleForeColor = Colors.TextColor;
            else
                titleForeColor = Colors.TextColorLight;

            Invalidate();
            base.OnEnabledChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(ControlPaint.LightLight(Colors.Grey)), 0, 0, Width, 30);
            e.Graphics.FillRectangle(new SolidBrush(indicatorColor), 0, 0, 10, Height);

            Brush bTitle = new SolidBrush(ForeColor);

            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

            e.Graphics.DrawString(Title, base.Font, bTitle, 16, 5);

            base.OnPaint(e);
        }

        public new void Dispose()
        {
            ledConfiguration?.Select((x) => x.Condition).ToList().ForEach((x) => x.ValueChanged -= Condition_ValueChanged);
            base.Dispose(true);
        }

        #endregion Protected methods

        #region Private methods

        /// <summary>
        /// Handle the <see cref="ledConfiguration"/> <see cref="ICondition"/> value changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Condition_ValueChanged(object sender, Core.ValueChangedEventArgs e)
        {
            UpdateLed();
        }

        /// <summary>
        /// Update the led based on the <see cref="ledConfiguration"/> active <see cref="ICondition"/>
        /// </summary>
        private void UpdateLed()
        {
            if (!InvokeRequired)
            {
                IEnumerable<(ICondition Condition, Color Color)> item = ledConfiguration.Where((x) => x.Condition.Value);
                IndicatorColor = item.Any() ? item.First().Color : Colors.Grey;
            }
            else
            {
                BeginInvoke(new Action(() => UpdateLed()));
            }
        }

        #endregion Private methods
    }
}

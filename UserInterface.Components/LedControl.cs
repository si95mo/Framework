using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    /// <summary>
    /// Implement a visual LED user control
    /// </summary>
    public partial class LedControl : Control
    {
        private Color color;
        private bool on = true;
        private Timer timer = new Timer();

        /// <summary>
        /// Gets or Sets the color of the LED light
        /// </summary>
        [DefaultValue(typeof(Color), "0x0, 0x80, 0x0")]
        public Color Color
        {
            get => color;
            set
            {
                color = value;
                Invalidate();  // Redraw the control
            }
        }

        /// <summary>
        /// Gets or Sets whether the light is turned on
        /// </summary>
        public bool On
        {
            get => on;
            set
            {
                on = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Create a new instance of <see cref="LedControl"/>
        /// </summary>
        public LedControl()
        {
            ControlStyles flag = ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor;
            SetStyle(flag, true);

            color = Color.Green;
            timer.Tick += new EventHandler(
                (object sender, EventArgs e) => { On = !On; }
            );
        }

        /// <summary>
        /// Handles the on paint event for the <see cref="LedControl"/>
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Create an offscreen graphics object for double buffering
            Bitmap offScreenBmp = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            using (Graphics g = Graphics.FromImage(offScreenBmp))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                // Draw the control
                DrawControl(g, this.On);
                // Draw the image to the screen
                e.Graphics.DrawImageUnscaled(offScreenBmp, 0, 0);
            }
        }

        /// <summary>
        /// Renders the control to an image
        /// </summary>
        private void DrawControl(Graphics g, bool on)
        {
            // Is the bulb on or off
            Color actualColor = on ? color : Color.Black;

            // Calculate the dimensions of the bulb
            int width = Width - (Padding.Left + Padding.Right);
            int height = Height - (Padding.Top + Padding.Bottom);

            // Diameter is the lesser of width and height
            int diameter = Math.Min(width, height);

            // Subtract 1 pixel so ellipse doesn't get cut off
            diameter = Math.Max(diameter - 1, 1);

            // Draw the background ellipse
            Rectangle rectangle = new Rectangle(Padding.Left, Padding.Top, diameter, diameter);
            g.FillEllipse(new SolidBrush(actualColor), rectangle);

            // Draw the border
            g.SetClip(ClientRectangle);
            if (On)
                g.DrawEllipse(new Pen(Color.FromArgb(85, Color.Black), 1F), rectangle);
            else
                g.DrawEllipse(new Pen(Color.FromArgb(85, Color.White), 1F), rectangle);
        }

        /// <summary>
        /// Causes the <see cref="LedControl"/> to start blinking
        /// </summary>
        /// <param name="interval">The blinking interval (in milliseconds). Set to 0 to stop</param>
        public void Blink(int interval)
        {
            if (interval > 0)
            {
                On = true;
                timer.Interval = interval;
                timer.Enabled = true;
            }
            else
            {
                timer.Enabled = false;
                On = false;
            }
        }

        /// <summary>
        /// Causes the <see cref="LedControl"/> to stop blinking. <br/>
        /// See also <see cref="Blink(int)"/>
        /// </summary>
        public void StopBlink() => Blink(0);
    }
}
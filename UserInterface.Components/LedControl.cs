﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    /// <summary>
    /// The LEDBulb is a .Net control for Windows Forms that emulates an
    /// LED light with two states On and Off.  The purpose of the control is to
    /// provide a sleek looking representation of an LED light that is sizable,
    /// has a transparent background and can be set to different colors.
    /// </summary>
    public partial class LedControl : Control
    {
        private Color color;
        private bool on = true;
        private Color reflectionColor = Color.FromArgb(180, 255, 255, 255);
        private Color[] surroundColor = new Color[] { Color.FromArgb(0, 255, 255, 255) };
        private Timer timer = new Timer();

        /// <summary>
        /// Gets or Sets the color of the LED light
        /// </summary>
        [DefaultValue(typeof(Color), "153, 255, 54")]
        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                DarkColor = ControlPaint.Dark(color);
                DarkDarkColor = ControlPaint.DarkDark(color);
                Invalidate();  // Redraw the control
            }
        }

        /// <summary>
        /// Dark shade of the LED color used for gradient
        /// </summary>
        public Color DarkColor { get; protected set; }

        /// <summary>
        /// Very dark shade of the LED color used for gradient
        /// </summary>
        public Color DarkDarkColor { get; protected set; }

        /// <summary>
        /// Gets or Sets whether the light is turned on
        /// </summary>
        public bool On
        {
            get { return on; }
            set { on = value; Invalidate(); }
        }

        /// <summary>
        /// Crea a new instance of <see cref="LedControl"/>
        /// </summary>
        public LedControl()
        {
            SetStyle(
                ControlStyles.DoubleBuffer
                | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.ResizeRedraw
                | ControlStyles.UserPaint
                | ControlStyles.SupportsTransparentBackColor,
                true
            );

            Color = Color.FromArgb(255, 153, 255, 54);
            timer.Tick += new EventHandler(
                (object sender, EventArgs e) => { On = !On; }
            );
        }

        /// <summary>
        /// Handles the Paint event for this UserControl
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Create an offscreen graphics object for double buffering
            Bitmap offScreenBmp = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            using (Graphics g = Graphics.FromImage(offScreenBmp))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                // Draw the control
                drawControl(g, this.On);
                // Draw the image to the screen
                e.Graphics.DrawImageUnscaled(offScreenBmp, 0, 0);
            }
        }

        /// <summary>
        /// Renders the control to an image
        /// </summary>
        private void drawControl(Graphics g, bool on)
        {
            // Is the bulb on or off
            Color lightColor = on ? Color : Color.FromArgb(150, DarkColor);
            Color darkColor = on ? DarkColor : DarkDarkColor;

            // Calculate the dimensions of the bulb
            int width = Width - (Padding.Left + Padding.Right);
            int height = Height - (Padding.Top + Padding.Bottom);
            // Diameter is the lesser of width and height
            int diameter = Math.Min(width, height);
            // Subtract 1 pixel so ellipse doesn't get cut off
            diameter = Math.Max(diameter - 1, 1);

            // Draw the background ellipse
            var rectangle = new Rectangle(Padding.Left, Padding.Top, diameter, diameter);
            g.FillEllipse(new SolidBrush(darkColor), rectangle);

            // Draw the glow gradient
            var path = new GraphicsPath();
            path.AddEllipse(rectangle);
            var pathBrush = new PathGradientBrush(path)
            {
                CenterColor = lightColor,
                SurroundColors = new Color[] { Color.FromArgb(0, lightColor) }
            };
            g.FillEllipse(pathBrush, rectangle);

            // Draw the white reflection gradient
            var offset = Convert.ToInt32(diameter * .15F);
            var diameter1 = Convert.ToInt32(rectangle.Width * .8F);
            var whiteRect = new Rectangle(rectangle.X - offset, rectangle.Y - offset, diameter1, diameter1);
            var path1 = new GraphicsPath();
            path1.AddEllipse(whiteRect);
            var pathBrush1 = new PathGradientBrush(path)
            {
                CenterColor = reflectionColor,
                SurroundColors = surroundColor
            };
            g.FillEllipse(pathBrush1, whiteRect);

            // Draw the border
            g.SetClip(ClientRectangle);
            if (On)
                g.DrawEllipse(new Pen(Color.FromArgb(85, Color.Black), 1F), rectangle);
        }

        /// <summary>
        /// Causes the Led to start blinking
        /// </summary>
        /// <param name="milliseconds">Number of milliseconds to blink for. 0 stops blinking</param>
        public void Blink(int milliseconds)
        {
            if (milliseconds > 0)
            {
                On = true;
                timer.Interval = milliseconds;
                timer.Enabled = true;
            }
            else
            {
                timer.Enabled = false;
                On = false;
            }
        }
    }
}
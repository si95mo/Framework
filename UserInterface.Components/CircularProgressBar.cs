using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class CircularProgressBar : UserControl
{
    public enum ProgressShape
    {
        Round,
        Flat
    }

    public enum TextMode
    {
        None,
        Value,
        Percentage,
        Custom
    }

    private long value;
    private long maximum = 100;
    private int lineWidth = 1;
    private float barWidth = 14f;

    private Color firstProgressColor = Color.Orange;
    private Color secondProgressColor = Color.Orange;
    private Color lineColor = Color.Silver;
    private LinearGradientMode gradientMode = LinearGradientMode.ForwardDiagonal;
    private ProgressShape progressBarShape;
    private TextMode progressBarTextMode;

    public CircularProgressBar()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        SetStyle(ControlStyles.Opaque, true);
        BackColor = SystemColors.Control;
        ForeColor = Color.DimGray;

        Size = new Size(130, 130);
        Font = new Font("Lucida Sans Unicode", 14);
        MinimumSize = new Size(100, 100);
        DoubleBuffered = true;

        LineWidth = 1;
        LineColor = Color.DimGray;

        Value = 57;
        ProgressBarShape = ProgressShape.Flat;
        ProgressBarTextMode = TextMode.Percentage;
    }

    /// <summary>
    /// Progress value
    /// </summary>
    [Description("Integer value that determines the progress bar fill"), Category("Behavior")]
    public long Value
    {
        get { return value; }
        set
        {
            if (value > maximum)
                value = maximum;

            this.value = value;
            Invalidate();
        }
    }

    [Description("The maximum value of the progress bar"), Category("Behavior")]
    public long Maximum
    {
        get { return maximum; }
        set
        {
            if (value < 1)
                value = 1;

            maximum = value;
            Invalidate();
        }
    }

    [Description("First color of the progress bar"), Category("Appearance")]
    public Color BarColor1
    {
        get { return firstProgressColor; }
        set
        {
            firstProgressColor = value;
            Invalidate();
        }
    }

    [Description("Last color of the progress bar"), Category("Appearance")]
    public Color BarColor2
    {
        get { return secondProgressColor; }
        set
        {
            secondProgressColor = value;
            Invalidate();
        }
    }

    [Description("Progress bar bar width"), Category("Appearance")]
    public float BarWidth
    {
        get { return barWidth; }
        set
        {
            barWidth = value;
            Invalidate();
        }
    }

    [Description("Gradient color mode"), Category("Appearance")]
    public LinearGradientMode GradientMode
    {
        get { return gradientMode; }
        set
        {
            gradientMode = value;
            Invalidate();
        }
    }

    [Description("Intermediate line color"), Category("Appearance")]
    public Color LineColor
    {
        get { return lineColor; }
        set
        {
            lineColor = value;
            Invalidate();
        }
    }

    [Description("Intermendiate line width"), Category("Appearance")]
    public int LineWidth
    {
        get { return lineWidth; }
        set
        {
            lineWidth = value;
            Invalidate();
        }
    }

    [Description("The progress bar shape"), Category("Appearance")]
    public ProgressShape ProgressBarShape
    {
        get { return progressBarShape; }
        set
        {
            progressBarShape = value;
            Invalidate();
        }
    }

    [Description("The progress bar text mode"), Category("Behavior")]
    public TextMode ProgressBarTextMode
    {
        get { return progressBarTextMode; }
        set
        {
            progressBarTextMode = value;
            Invalidate();
        }
    }

    [Description("The progress bar text"), Category("Behavior")]
    public override string Text { get; set; }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        SetStandardSize();
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);
        SetStandardSize();
    }

    protected override void OnPaintBackground(PaintEventArgs p)
    {
        base.OnPaintBackground(p);
    }

    private void SetStandardSize()
    {
        int _Size = Math.Max(Width, Height);
        Size = new Size(_Size, _Size);
    }

    public void Increment(int Val)
    {
        value += Val;
        Invalidate();
    }

    public void Decrement(int Val)
    {
        value -= Val;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        using (Bitmap bitmap = new Bitmap(Width, Height))
        {
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;

                PaintTransparentBackground(this, e);

                // External cirlce
                using (Brush mBackColor = new SolidBrush(BackColor))
                {
                    graphics.FillEllipse(
                        mBackColor,
                        18, 
                        18,
                        (Width - 0x30) + 12,
                        (Height - 0x30) + 12
                    );
                }

                // Internal circle
                using (Pen pen2 = new Pen(LineColor, LineWidth))
                {
                    graphics.DrawEllipse(
                        pen2,
                        18,
                        18,
                        (Width - 0x30) + 12,
                        (Height - 0x30) + 12
                    );
                }

                // Progress bar
                using (LinearGradientBrush brush =  
                    new LinearGradientBrush(ClientRectangle, firstProgressColor, secondProgressColor, GradientMode))
                {
                    using (Pen pen = new Pen(brush, BarWidth))
                    {
                        switch (progressBarShape)
                        {
                            case ProgressShape.Round:
                                pen.StartCap = LineCap.Round;
                                pen.EndCap = LineCap.Round;
                                break;

                            case ProgressShape.Flat:
                                pen.StartCap = LineCap.Flat;
                                pen.EndCap = LineCap.Flat;
                                break;
                        }

                        // Progress bar draw
                        graphics.DrawArc(pen,
                            0x12, 0x12,
                            (Width - 0x23) - 2,
                            (Height - 0x23) - 2,
                            -90,
                            (int)Math.Round((double)((360.0 / maximum) * value)));
                    }
                }

                switch (ProgressBarTextMode)
                {
                    case TextMode.None:
                        Text = string.Empty;
                        break;

                    case TextMode.Value:
                        Text = value.ToString();
                        break;

                    case TextMode.Percentage:
                        Text = Convert.ToString(Convert.ToInt32((100 / maximum) * value)) + " %";
                        break;

                    default:
                        break;
                }

                if (Text != string.Empty)
                {
                    using (Brush FontColor = new SolidBrush(ForeColor))
                    {
                        int ShadowOffset = 2;
                        SizeF MS = graphics.MeasureString(Text, Font);
                        SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(100, ForeColor));

                        // Text effects
                        graphics.DrawString(
                            Text, 
                            Font, 
                            shadowBrush,
                            Convert.ToInt32(Width / 2 - MS.Width / 2) + ShadowOffset,
                            Convert.ToInt32(Height / 2 - MS.Height / 2) + ShadowOffset
                        );

                        // Text
                        graphics.DrawString(
                            Text, 
                            Font, 
                            FontColor,
                            Convert.ToInt32(Width / 2 - MS.Width / 2),
                            Convert.ToInt32(Height / 2 - MS.Height / 2)
                        );
                    }
                }

                // Control draw
                e.Graphics.DrawImage(bitmap, 0, 0);
                graphics.Dispose();
                bitmap.Dispose();
            }
        }
    }

    private static void PaintTransparentBackground(Control c, PaintEventArgs e)
    {
        if (c.Parent == null || !Application.RenderWithVisualStyles)
            return;

        ButtonRenderer.DrawParentBackground(e.Graphics, c.ClientRectangle, c);
    }
}
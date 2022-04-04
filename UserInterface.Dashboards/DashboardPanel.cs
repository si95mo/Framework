using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface.Dashboards
{
    public partial class DashboardPanel : UserControl
    {
        private Graphics graphics;

        protected Color LineColor { get; } = Color.FromArgb(0x28, 0x00, 0x00);
        
        public DashboardPanel()
        {
            InitializeComponent();

            graphics = CreateGraphics();
            DrawLines();
        }

        /// <summary>
        /// Drag the lines grid
        /// </summary>
        public void DrawLines()
        {
            Pen pen = new Pen(LineColor, 0.001f);

            for (int x = 0; x <= Size.Width; x += 16)
            {
                for (int y = 0; y < Size.Height; y += 16)
                {
                    graphics.DrawLine(pen, x, y, Size.Width, y);
                    graphics.DrawLine(pen, x, y, x, Size.Height);
                }
            }
        }

        public void ClearLines()
            => graphics.Clear(SystemColors.Control);
    }
}

using System;
using System.Drawing;
using System.Windows.Forms;

namespace UserInterface.Controls.Tests
{
    public partial class TestForm : Form
    {
        private int counter = 0;
        private Color onColor = Color.Green;
        private Color offColor = Color.Red;

        public TestForm()
        {
            InitializeComponent();

            ledControl.Blink(1000);

            Timer timer = new Timer();
            timer.Interval = 10000;
            timer.Tick += Timer_Tick;

            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ledControl.Invoke(new MethodInvoker(() =>
                    {
                        if (++counter % 2 == 0)
                            ledControl.Color = onColor;
                        else
                            ledControl.Color = offColor;
                    }
                )
            );
        }
    }
}
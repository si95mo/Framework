using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    public partial class ButtonControl : BaseControl
    {
        public override object Value
        {
            get => Text;
            set => Text = (string)value;
        }

        public override string Text
        {
            get => btnControl.Text;
            set => btnControl.Text = value;
        }

        public EventHandler<MouseEventArgs> ButtonClicked;

        public ButtonControl()
        {
            InitializeComponent();
        }
    }
}

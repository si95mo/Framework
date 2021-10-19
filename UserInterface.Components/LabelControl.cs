using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    public class LabelControl : Label
    {
        public LabelControl() : base()
        {
            Font = new Font("Lucida Sans Unicode", 12);
        }
    }
}

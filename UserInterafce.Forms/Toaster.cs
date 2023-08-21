using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface.Forms
{
    /// <summary>
    /// Define the <see cref="Toaster"/> type
    /// </summary>
    public enum ToasterType
    {
        Message = 0,
        Warning = 1,
        Error = 2
    }

    /// <summary>
    /// Define a toaster <see cref="UserControl"/>
    /// </summary>
    internal partial class Toaster : UserControl
    {
        private readonly ToasterType type;
        private readonly CustomForm parent;

        public Toaster()
        {
            InitializeComponent();

            type = ToasterType.Message;
            parent = FindForm() as CustomForm;
        }

        public Toaster(ToasterType type, CustomForm parent) : this()
        {
            this.type = type;
            this.parent = parent;
        }
    }
}

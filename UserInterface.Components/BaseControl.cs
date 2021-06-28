using System.Windows.Forms;

namespace UserInterface.Controls
{
    public partial class BaseControl : UserControl
    {
        protected object value;

        /// <summary>
        /// The <see cref="BaseControl"/> value
        /// </summary>
        public virtual object Value
        {
            get => value;
            set => this.value = value;
        }

        /// <summary>
        /// Initialize the <see cref="BaseControl"/>
        /// with default values
        /// </summary>
        public BaseControl()
        {
            InitializeComponent();
        }
    }
}

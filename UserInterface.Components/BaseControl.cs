using System.Windows.Forms;

namespace UserInterface.Controls
{
    /// <summary>
    /// A base control, used as a base class for create 
    /// more advanced user interface components
    /// </summary>
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

        /// <summary>
        /// Give an alphabetical representation of the
        /// <see cref="BaseControl"/>
        /// </summary>
        /// <returns>The description of the <see cref="BaseControl"/></returns>
        public override string ToString()
        {
            string description = $"({this.GetType().Name}, {value})";
            return description;
        }
    }
}

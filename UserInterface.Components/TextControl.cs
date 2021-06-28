namespace UserInterface.Controls
{
    public partial class TextControl : BaseControl
    {
        public override object Value
        {
            get => txbValue.Text;
            set => txbValue.Text = (string)value;
        }

        public TextControl()
        {
            InitializeComponent();
        }
    }
}

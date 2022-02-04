namespace UserInterface.Controls
{
    /// <summary>
    /// Implement a panel selector control
    /// </summary>
    public partial class PanelSelectorControl : BaseControl
    {
        private string panelName;

        /// <summary>
        /// The <see cref="PanelControl"/> associated to the
        /// <see cref="PanelSelectorControl"/>
        /// </summary>
        public new string Value => panelName;

        /// <summary>
        /// The <see cref="ButtonControl"/> associated to the
        /// <see cref="PanelSelectorControl"/>
        /// </summary>
        public ButtonControl Button => btnSelector;

        /// <summary>
        /// Create a new instance of <see cref="PanelSelectorControl"/>
        /// </summary>
        protected PanelSelectorControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize a new instance of <see cref="PanelSelectorControl"/>
        /// </summary>
        /// <param name="panelName"></param>
        public PanelSelectorControl(string panelName) : this()
        {
            this.panelName = panelName;
            btnSelector.Text = panelName;
        }
    }
}
using Core.DataStructures;
using Hardware;
using System;
using System.Windows.Forms;

namespace UserInterface.Dashboards
{
    /// <summary>
    /// Implement an <see cref="UserControl"/> that will configure an <see cref="IDashboardControl"/>
    /// </summary>
    public partial class ItemConfigurationControl : UserControl
    {
        private IDashboardControl control;
        private AutoCompleteStringCollection source;

        /// <summary>
        /// Create a new instance of <see cref="ItemConfigurationControl"/>
        /// </summary>
        protected ItemConfigurationControl()
        {
            InitializeComponent();

            source = new AutoCompleteStringCollection();
            SetAutoCompleteSource();
        }

        private void SetAutoCompleteSource()
        {
            source.Clear();
            source.AddRange(ServiceBroker.Get<IChannel>().Keys.ToArray());

            txbChannelCode.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txbChannelCode.AutoCompleteMode = AutoCompleteMode.Suggest;
            txbChannelCode.AutoCompleteCustomSource = source;

            txbChannelCode.KeyDown += TxbChannelCode_KeyDown;
            txbDescription.KeyDown += TxbDescription_KeyDown;
        }

        /// <summary>
        /// Create a new instance of <see cref="ItemConfigurationControl"/>
        /// </summary>
        /// <param name="control">The <see cref="IDashboardControl"/> to configure</param>
        internal ItemConfigurationControl(IDashboardControl control) : this()
        {
            this.control = control;
        }

        private void TxbChannelCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string channelCode = txbChannelCode.Text.TrimEnd(Environment.NewLine.ToCharArray());
                control.SetChannel(channelCode);
            }
        }

        private void TxbDescription_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string description = txbDescription.Text.TrimEnd(Environment.NewLine.ToCharArray());

                if (description.CompareTo(string.Empty) != 0)
                    control.Description = txbDescription.Text;
                else
                {
                    if (control.Channel != null)
                        control.Description = control.Channel.Code;
                }
            }
        }
    }
}
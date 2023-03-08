using Core.DataStructures;
using Hardware;
using System.Windows.Forms;

namespace UserInterface.Controls.Tests
{
    public partial class HardwareControlsForm : Form
    {
        public HardwareControlsForm()
        {
            InitializeComponent();

            ResourceControl resourceControl;
            ChannelControl channelControl;
            foreach(IResource resource in ServiceBroker.Get<IResource>())
            {
                resourceControl = new ResourceControl(resource);
                resourceFlowLayout.Controls.Add(resourceControl);

                foreach(IChannel channel in resource.Channels)
                {
                    channelControl = new ChannelControl(channel);
                    channelsLayoutPanel.Controls.Add(channelControl);
                }
            }
        }
    }
}

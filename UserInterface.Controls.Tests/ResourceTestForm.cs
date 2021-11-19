using Hardware.Modbus;
using System;

namespace UserInterface.Controls.Tests
{
    public partial class ResourceTestForm : CustomForm
    {
        public ResourceTestForm()
        {
            InitializeComponent();
        }

        private async void ResourceTestForm_Load(object sender, EventArgs e)
        {
            ModbusResource resource = new ModbusResource("ModbusResource", "123.456.789.000");
            await resource.Start();

            ResourceControl resourceControl = new ResourceControl(resource, layoutPanel);
            layoutPanel.Controls.Add(resourceControl);
        }
    }
}
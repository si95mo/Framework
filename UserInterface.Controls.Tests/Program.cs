using Core.DataStructures;
using Hardware;
using System;
using System.Windows.Forms;
using UserInterface.Forms;

namespace UserInterface.Controls.Tests
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            ServiceBroker.Initialize();

            IResource resource;

            AnalogInput analogInput;
            AnalogOutput analogOutput;
            DigitalInput digitalInput;
            DigitalOutput digitalOutput;
            MultiSampleAnalogInput msAnalogInput;
            Stream stream;

            //for (int i = 1; i <= 10; i++)
            //{
            //    resource = new ResourceSimulator(i.ToString());

            //    for (int j = 1; j <= 2; j++)
            //    {
            //        analogInput = new AnalogInput($"Ai{i}-{j}", resource, "V", "0.000").WithDescription(Guid.NewGuid().ToString()) as AnalogInput;
            //        analogOutput = new AnalogOutput($"Ao{i}-{j}", resource, "V", "0.000").WithDescription(Guid.NewGuid().ToString()) as AnalogOutput;
            //        digitalInput = new DigitalInput($"Di{i}-{j}", resource).WithDescription(Guid.NewGuid().ToString()) as DigitalInput;
            //        digitalOutput = new DigitalOutput($"Do{i}-{j}", resource).WithDescription(Guid.NewGuid().ToString()) as DigitalOutput;
            //        msAnalogInput = new MultiSampleAnalogInput($"Msai{i}-{j}", resource, "V", "0.000").WithDescription(Guid.NewGuid().ToString()) as MultiSampleAnalogInput;
            //        stream = new Stream($"S{i}-{j}", System.Text.Encoding.ASCII, resource).WithDescription(Guid.NewGuid().ToString()) as Stream;
            //        stream.Value = stream.Encoding.GetBytes($"Hello {i}-{j}!");

            //        ServiceBroker.Add<IChannel>(analogInput);
            //        ServiceBroker.Add<IChannel>(analogOutput);
            //        ServiceBroker.Add<IChannel>(digitalInput);
            //        ServiceBroker.Add<IChannel>(digitalOutput);
            //        ServiceBroker.Add<IChannel>(msAnalogInput);
            //        ServiceBroker.Add<IChannel>(stream);
            //    }

            //    resource.Start();
            //    ServiceBroker.Add<IResource>(resource);
            //}

            resource = new SystemInfoResource("SystemInfoResource", 1000);
            resource.Start();
            ServiceBroker.Add<IResource>(resource);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            CustomForm form = new TasksForm();
            Application.Run(form);
            //Application.Run(new TreeViewTestForm());
        }
    }
}
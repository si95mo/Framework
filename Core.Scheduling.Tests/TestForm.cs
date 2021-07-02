using Core.Scheduling.Wrapper;
using IO;
using IO.File;
using LiveCharts;
using LiveCharts.Wpf;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using UserInterface.Controls;

namespace Core.Scheduling.Tests
{
    public partial class TestForm : Form
    {
        private const string jsonPath = @"test/test_program.json";
        private const string binPath = @"test/test_program.bin";

        private SimpleMethodScheduler scheduler;

        private LineSeries series = new LineSeries();

        private bool isOpen = true;

        public TestForm()
        {
            InitializeComponent();

            series.Title = "A series";
            series.Values = new ChartValues<double>();

            TextBoxWriter writer = new TextBoxWriter(txbConsole);
            Console.SetOut(writer);

            scheduler = new SimpleMethodScheduler();
            scheduler.Subscribers.Enqueued += Element_Enqueued;

            TestClass testObject = new TestClass();
            DummyClass dummyObject = new DummyClass();

            var methods = MethodWrapper.Wrap(testObject);
            foreach (var method in methods)
                panelMethods.Controls.Add(new MethodControl(method, scheduler));

            methods = MethodWrapper.Wrap(dummyObject);
            foreach (var method in methods)
                panelMethods.Controls.Add(new MethodControl(method, scheduler));
        }

        private void Element_Enqueued(object sender, EventArgs e)
        {
            lbxInput.Items.Add(
                scheduler.Subscribers.ElementAt(
                    scheduler.Subscribers.Count - 1
                ).ToString()
            );
        }

        private void BtnExecute_Click(object sender, EventArgs e)
        {
            int n = scheduler.Subscribers.Count;
            for (int i = 0; i < n; i++)
            {
                var method = scheduler.Execute();

                lbxOutput.Items.Add(
                    method.ToString()
                );
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            lbxInput.Items.Clear();
            lbxOutput.Items.Clear();
            txbConsole.Text = "";

            scheduler.RemoveAll();
        }

        private void BtnSaveTest_Click(object sender, EventArgs e)
        {
            IOUtility.CreateDirectoryIfNotExists("test");

            JObject json = new JObject();
            int index = 0;
            foreach (var item in lbxInput.Items)
            {
                json.Add(new JProperty((index++).ToString(), item));
            }

            JSON.SaveJSON(json, jsonPath);

            scheduler.SaveExecutionList(binPath);
        }

        private void BtnLoadTest_Click(object sender, EventArgs e)
        {
            lbxInput.Items.Clear();
            lbxOutput.Items.Clear();

            scheduler.RemoveAll();

            JObject json = JSON.ReadJSON(jsonPath);

            foreach (var item in json.Properties())
            {
                string value = item.Value.ToString();
                lbxInput.Items.Add(value);
            }

            scheduler.LoadExecutionList(binPath);
        }

        private void TxbConsole_DoubleClick(object sender, EventArgs e)
        {
            txbConsole.Text = "";
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
            isOpen = false;

            Dispose();
        }

        private void UpdateAction()
        {
            Stopwatch sw = Stopwatch.StartNew();
            while (isOpen)
            {
                series.Values.Add(new Random(sw.Elapsed.Milliseconds).NextDouble());
                Thread.Sleep(100);

                if (series.Values.Count > 10)
                    series.Values.RemoveAt(0);
            }
        }
    }

    /// <summary>
    /// Redirect standard output (i.e. <see cref="Console.WriteLine"/>)
    /// to an UI control (i.e. a <see cref="TextBox"/>).
    /// </summary>
    public class TextBoxWriter : TextWriter
    {
        // The control where we will write text.
        private Control MyControl;

        public TextBoxWriter(Control control)
        {
            MyControl = control;
        }

        public override void Write(char value)
        {
            MyControl.Text += value;
        }

        public override void Write(string value)
        {
            MyControl.Text += value;
        }

        public override Encoding Encoding
        {
            get { return Encoding.Unicode; }
        }
    }
}
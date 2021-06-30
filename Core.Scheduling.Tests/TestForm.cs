using IO;
using IO.File;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Core.Scheduling.Wrapper;
using UserInterface.Controls;

namespace Core.Scheduling.Tests
{
    public partial class TestForm : Form
    {
        private const string jsonPath = @"test/test_program.json";
        private const string binPath = @"test/test_program.bin";

        private SimpleScheduler scheduler;

        public TestForm()
        {
            InitializeComponent();

            TextBoxWriter writer = new TextBoxWriter(txbConsole);
            Console.SetOut(writer);

            scheduler = new SimpleScheduler();
            scheduler.SubscribedMethods.Enqueued += Element_Enqueued;
            scheduler.SubscribedMethods.Dequeued += Element_Dequeued;

            TestClass testObject = new TestClass();
            DummyClass dummyObject = new DummyClass();

            var methods = MethodWrapper.Wrap(testObject);
            foreach (var method in methods)
            {
                panelMethods.Controls.Add(new MethodControl(method, scheduler));
            }

            methods = MethodWrapper.Wrap(dummyObject);
            foreach (var method in methods)
            {
                panelMethods.Controls.Add(new MethodControl(method, scheduler));
            }
        }

        private void Element_Dequeued(object sender, EventArgs e)
        {

        }

        private void Element_Enqueued(object sender, EventArgs e)
        {
            lbxInput.Items.Add(
                scheduler.SubscribedMethods.ElementAt(
                    scheduler.SubscribedMethods.Count - 1
                ).ToString()
            );
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (lbxInput.SelectedItem != null)
                lbxOutput.Items.Add(lbxInput.SelectedItem);
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if (lbxOutput.SelectedItem != null)
                lbxOutput.Items.Remove(lbxOutput.SelectedItem);
        }

        private void BtnExecute_Click(object sender, EventArgs e)
        {
            int n = scheduler.SubscribedMethods.Count;
            for (int i = 0; i < n; i++)
            {
                var method = scheduler.ExecuteAction();

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
            foreach(var item in lbxInput.Items)
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

            foreach(var item in json.Properties())
            {
                string value = item.Value.ToString();
                lbxInput.Items.Add(value);
                lbxOutput.Items.Add(value);
            }

            scheduler.LoadExecutionList(binPath);
        }

        private void TxbConsole_DoubleClick(object sender, EventArgs e)
        {
            txbConsole.Text = "";
        }

        private void lbxOutput_SelectedIndexChanged(object sender, EventArgs e)
        {

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

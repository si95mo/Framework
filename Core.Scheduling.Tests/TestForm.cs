﻿using Benches.Template;
using Core.Scheduling.Wrapper;
using Instructions;
using IO;
using IO.File;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UserInterface.Controls;

namespace Core.Scheduling.Tests
{
    public partial class TestForm : Form
    {
        private const string jsonPath = @"test/test_program.json";
        private const string binPath = @"test/test_program.bin";

        private readonly SimpleInstructionScheduler instructionScheduler;
        private readonly SimpleMethodScheduler methodScheduler;

        public TestForm()
        {
            InitializeComponent();

            TextBoxWriter writer = new TextBoxWriter(txbConsole);
            Console.SetOut(writer);

            instructionScheduler = new SimpleInstructionScheduler();
            instructionScheduler.Subscribers.Enqueued += Instruction_Enqueued;

            methodScheduler = new SimpleMethodScheduler();
            methodScheduler.Instructions.Enqueued += Method_Enqueued;

            TestClass testObject = new TestClass();
            DummyClass dummyObject = new DummyClass();

            NewBench bench = new NewBench("DummyBench");
            foreach (var instruction in bench.Instructions.ToList())
                panelInstructions.Controls.Add(
                    new InstructionControl(
                        instruction as Instruction,
                        instructionScheduler
                    )
                );

            var methods = MethodWrapper.Wrap(testObject);
            foreach (var method in methods)
                panelMethods.Controls.Add(new MethodControl(method, methodScheduler));

            methods = MethodWrapper.Wrap(dummyObject);
            foreach (var method in methods)
                panelMethods.Controls.Add(new MethodControl(method, methodScheduler));
        }

        private void Instruction_Enqueued(object sender, EventArgs e)
        {
            lbxInput.Items.Add(
                instructionScheduler.Subscribers.ElementAt(
                    instructionScheduler.Subscribers.Count - 1
                ).ToString()
            );
        }

        private void Method_Enqueued(object sender, EventArgs e)
        {
            lbxInput.Items.Add(
                methodScheduler.Instructions.ElementAt(
                    methodScheduler.Instructions.Count - 1
                ).ToString()
            );
        }

        private void BtnExecute_Click(object sender, EventArgs e)
        {
            int n = methodScheduler.Instructions.Count;
            for (int i = 0; i < n; i++)
            {
                var method = methodScheduler.Execute();

                lbxOutput.Items.Add(
                    method.ToString()
                );
            }

            n = instructionScheduler.Subscribers.Count;
            for (int i = 0; i < n; i++)
            {
                var instruction = instructionScheduler.Execute();

                lbxOutput.Items.Add(
                    instruction.ToString()
                );
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            lbxInput.Items.Clear();
            lbxOutput.Items.Clear();
            txbConsole.Text = "";

            methodScheduler.RemoveAll();
        }

        private void BtnSaveProgram_Click(object sender, EventArgs e)
        {
            IOUtility.CreateDirectoryIfNotExists("test");

            JObject json = new JObject();
            int index = 0;
            foreach (var item in lbxInput.Items)
            {
                json.Add(new JProperty((index++).ToString(), item));
            }

            JSON.SaveJSON(json, jsonPath);

            methodScheduler.SaveExecutionList(binPath);
            instructionScheduler.SaveExecutionList(binPath);

            MessageBox.Show("Program saved", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnLoadProgram_Click(object sender, EventArgs e)
        {
            lbxInput.Items.Clear();
            lbxOutput.Items.Clear();

            methodScheduler.RemoveAll();
            instructionScheduler.RemoveAll();

            JObject json = JSON.ReadJSON(jsonPath);

            //foreach (var item in json.Properties())
            //{
            //    string value = item.Value.ToString();
            //    lbxInput.Items.Add(value);
            //}

            methodScheduler.LoadExecutionList(binPath);
            instructionScheduler.LoadExecutionList(binPath);

            MessageBox.Show("Program loaded", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void TxbConsole_DoubleClick(object sender, EventArgs e)
        {
            txbConsole.Text = "";
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }

        private void lbxOutput_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void lbxInput_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void txbConsole_TextChanged(object sender, EventArgs e)
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
        private readonly Control control;

        public TextBoxWriter(Control control)
        {
            this.control = control;
        }

        public override void Write(char value)
        {
            control.Text += value;
        }

        public override void Write(string value)
        {
            control.Text += value;
        }

        public override Encoding Encoding
        {
            get { return Encoding.Unicode; }
        }
    }
}
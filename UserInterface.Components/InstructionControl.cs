using Core.Parameters;
using Core.Scheduling;
using Extensions;
using Instructions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    public partial class InstructionControl : UserControl
    {
        private const int controlLocationOffset = 6;
        private const int textBoxSize = 25;

        private Size size;

        private Instruction instruction;
        private InstructionScheduler scheduler;
        private List<BaseControl> values = new List<BaseControl>();

        /// <summary>
        /// Create a new instance of the class <see cref="InstructionControl"/>
        /// </summary>
        /// <param name="instruction">The <see cref="Instruction"/></param>
        /// <param name="scheduler">The <see cref="InstructionScheduler"/></param>
        public InstructionControl(Instruction instruction, InstructionScheduler scheduler)
        {
            InitializeComponent();

            size = Size;
            btnAdd.Image = Properties.Resources.ImageAdd;

            this.instruction = instruction;
            var pars = instruction.InputParameters;
            this.scheduler = scheduler;

            lblInstructionName.Text = instruction.Code;

            int parameterCount = (int)pars?.Count;
            if (parameterCount > 0)
            {
                size.Height += textBoxSize * parameterCount + 8;
                Size = size;
            }

            int index = 1;
            Label lblParam;
            if (pars != null)
            {
                foreach (var p in pars.ToList())
                {
                    lblParam = new Label();
                    lblParam.AutoSize = true;
                    lblParam.Text = p.Code;
                    lblParam.Location = new Point(
                        lblMethodNamePlaceholder.Location.X + 10,
                        lblMethodNamePlaceholder.Location.Y + 10
                            + index++ * (lblMethodNamePlaceholder.Size.Height + controlLocationOffset)
                    );
                    lblParam.Size = lblMethodNamePlaceholder.Size;

                    if (
                        p.GetType() == typeof(NumericParameter) ||
                        p.GetType() == typeof(StringParameter) ||
                        p.GetType() == typeof(TimeSpanParameter)
                    )
                    {
                        var txtCtrl = new TextControl();
                        txtCtrl.Location = new Point(
                            lblInstructionName.Location.X,
                            lblParam.Location.Y - 4
                        );

                        AddElementToCollection(lblParam, txtCtrl);
                    }
                    else
                    {
                        if (p.GetType() == typeof(BooleanParameter))
                        {
                            var diCtrl = new DigitalControl();
                            diCtrl.Location = new Point(
                                lblInstructionName.Location.X,
                                lblParam.Location.Y - 4
                            );

                            AddElementToCollection(lblParam, diCtrl);
                        }
                        else
                        {
                            var cbxCtrl = new ComboControl();
                            cbxCtrl.Location = new Point(
                                lblInstructionName.Location.X,
                                lblParam.Location.Y - 4
                            );

                            BindingSource bs = new BindingSource();
                            bs.DataSource = Enum.GetValues(p.Type);
                            cbxCtrl.DataSource = bs;

                            AddElementToCollection(lblParam, cbxCtrl);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Add an element to the collection
        /// </summary>
        /// <param name="lblParam">The <see cref="Label"/></param>
        /// <param name="control">The <see cref="BaseControl"/></param>
        private void AddElementToCollection(Label lblParam, BaseControl control)
        {
            Controls.Add(lblParam);
            Controls.Add(control);

            values.Add(control);
        }

        /// <summary>
        /// Add a <see cref="Method"/> to the <see cref="MethodScheduler"/>
        /// <see cref="ActionQueue{T}"/>
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e"><The <see cref="EventArgs"/></param>
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            IInstruction deepInstructionCopy = instruction.DeepCopy();
            var parameters = deepInstructionCopy.InputParameters.ToList();

            for (int i = 0; i < parameters.Count; i++)
            {
                var p = parameters.ElementAt(i);
                Type t = p.GetType();

                switch (t)
                {
                    case Type boolType when boolType == typeof(BooleanParameter):
                        (parameters.ElementAt(i) as BooleanParameter).Value = (bool)values.ElementAt(i).Value;
                        break;

                    case Type numType when numType == typeof(NumericParameter):
                        (parameters.ElementAt(i) as NumericParameter).Value = (double)values.ElementAt(i).Value;
                        break;

                    case Type stringType when stringType == typeof(StringParameter):
                        (parameters.ElementAt(i) as StringParameter).Value = (string)values.ElementAt(i).Value;
                        break;

                    case Type timeType when timeType == typeof(TimeSpanParameter):
                        (parameters.ElementAt(i) as TimeSpanParameter).Value = (TimeSpan)values.ElementAt(i).Value;
                        break;

                    default: // EnumParameter<T>
                        if (t.IsGenericType)
                            if (t.GetGenericTypeDefinition() == typeof(EnumParameter<>))
                                parameters.ElementAt(i).ValueAsObject = (Enum)values.ElementAt(i).Value;
                        break;
                }
            }

            scheduler.AddElement(deepInstructionCopy);
        }

        private void InstructionControl_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(
                e.Graphics,
                e.ClipRectangle,
                Colors.BackgroundColor,
                ButtonBorderStyle.Solid
            );
        }
    }
}
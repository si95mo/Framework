using Core.DataStructures;
using Core.Scheduling;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    /// <summary>
    /// An user control to graphically visualize an instance
    /// of <see cref="Method"/> and eventually add it
    /// to a <see cref="MethodScheduler"/>.
    /// See also <see cref="UserControl"/>
    /// </summary>
    public partial class MethodControl : UserControl
    {
        private const int LocationOffset = 6;
        private const int TextBoxSize = 25;

        private Size size;

        private Method method;
        private List<Control> values = new List<Control>();

        /// <summary>
        /// Create a new instance of the class <see cref="MethodControl"/>
        /// </summary>
        /// <param name="method">The <see cref="Method"/></param>
        public MethodControl(Method method)
        {
            InitializeComponent();

            size = Size;

            this.method = method;
            ParameterList<MethodParameter> parameters = method.Parameters;

            lblMethodName.Text = method.Name;

            int parameterCount = (int)parameters?.Count;
            if (parameterCount > 1)
            {
                size.Height += TextBoxSize * parameterCount + 8;
                Size = size;
            }

            int index = 0;
            Label lblParameter;
            if (parameters != null)
            {
                foreach (MethodParameter parameter in parameters)
                {
                    int y;
                    if (++index == 1)
                        y = lblMethodName.Location.Y;
                    else
                        y = index * (lblMethodName.Size.Height + TextBoxSize);

                    lblParameter = new Label
                    {
                        AutoSize = true,
                        Location = new Point(lblMethodName.Size.Width + LocationOffset, y),
                        Size = lblMethodName.Size
                    };
                    lblParameter.Text = parameter.Name;

                    if (parameter.Type != ParameterType.Bool && parameter.Type != ParameterType.Enum)
                    {
                        TextControl txtCtrl = new TextControl
                        {
                            Location = CalculateLocation(lblParameter)
                        };
                        txtCtrl.Size = new Size(txtCtrl.Size.Width + txtCtrl.Size.Width * 20 / 100, txtCtrl.Size.Height);

                        AddElementToCollection(lblParameter, txtCtrl);
                    }
                    else
                    {
                        if (parameter.Type == ParameterType.Bool)
                        {
                            DigitalControl diCtrl = new DigitalControl
                            {
                                Location = CalculateLocation(lblParameter)
                            };

                            AddElementToCollection(lblParameter, diCtrl);
                        }
                        else
                        {
                            ComboControl cbxCtrl = new ComboControl
                            {
                                Location = CalculateLocation(lblParameter)
                            };

                            BindingSource bs = new BindingSource
                            {
                                DataSource = Enum.GetValues(parameter.ExactType)
                            };
                            cbxCtrl.DataSource = bs;

                            AddElementToCollection(lblParameter, cbxCtrl);
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
        private void AddElementToCollection(Label lblParam, Control control)
        {
            Controls.Add(lblParam);
            Controls.Add(control);

            values.Add(control);
        }

        /// <summary>
        /// Calculate the control <see cref="Control.Location"/>
        /// </summary>
        /// <param name="lbl">The reference <see cref="Label"/></param>
        /// <returns>The <see cref="Point"/> with the calculated <see cref="Control.Location"/></returns>
        private Point CalculateLocation(Label lbl)
        {
            Point point = new Point(lbl.Location.X + lbl.Size.Width + LocationOffset, lbl.Location.Y - 4);
            point.X -= point.X * 20 / 100;

            return point;
        }

        /// <summary>
        /// Add a <see cref="Method"/> to the <see cref="MethodScheduler"/>
        /// <see cref="ActionQueue{T}"/>
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="EventArgs"/></param>
        private void BtnInvoke_Click(object sender, EventArgs e)
        {
            string[] parameters = new string[method.ParametersCount];
            for (int i = 0; i < method.ParametersCount; i++)
                parameters[i] = values[i].Text;

            method.Invoke(parameters);
        }
    }
}
using Core.DataStructures;
using Core.Scheduling;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    /// <summary>
    /// An user control to graphically visualize an instance of <see cref="Method"/>.
    /// See also <see cref="UserControl"/>
    /// </summary>
    public partial class MethodControl : UserControl
    {
        private const int controlLocationOffset = 6;
        private const int textBoxSize = 25;

        private Size size;

        private Method method;
        private List<UserControl> values = new List<UserControl>();

        /// <summary>
        /// Create a new instance of the class <see cref="MethodControl"/>
        /// </summary>
        /// <param name="method">The <see cref="Method"/></param>
        public MethodControl(Method method)
        {
            InitializeComponent();

            size = Size;
            btnAdd.Image = Properties.Resources.ImageAdd;

            this.method = method;
            ParameterList<MethodParameter> parameters = method.Parameters;

            lblMethodName.Text = method.Name;

            int parameterCount = (int)parameters?.Count;
            if (parameterCount > 0)
            {
                size.Height += textBoxSize * parameterCount + 8;
                Size = size;
            }

            int index = 1;
            Label lblParameter;
            if (parameters != null)
            {
                foreach (MethodParameter parameter in parameters)
                {
                    lblParameter = new Label();
                    lblParameter.AutoSize = true;
                    lblParameter.Text = parameter.Name;
                    lblParameter.Size = lblMethodName.Size;
                    lblParameter.Location = new Point(
                        lblMethodName.Location.X + 10,
                        lblMethodName.Location.Y + 10 + index++ * (lblMethodName.Size.Height + controlLocationOffset)
                    );

                    if (parameter.Type != ParameterType.Bool && parameter.Type != ParameterType.Enum)
                    {
                        TextControl txtCtrl = new TextControl
                        {
                            Location = new Point(lblMethodName.Location.X, lblParameter.Location.Y - 4)
                        };

                        // AddElementToCollection(lblParameter, txtCtrl);
                    }
                    else
                    {
                        if (parameter.Type == ParameterType.Bool)
                        {
                            DigitalControl diCtrl = new DigitalControl
                            {
                                Location = new Point(lblMethodName.Location.X, lblParameter.Location.Y - 4)
                            };

                            AddElementToCollection(lblParameter, diCtrl);
                        }
                        else
                        {
                            ComboControl cbxCtrl = new ComboControl
                            {
                                Location = new Point(lblMethodName.Location.X, lblParameter.Location.Y - 4)
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
        private void AddElementToCollection(Label lblParam, UserControl control)
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
        /// <param name="e">The <see cref="EventArgs"/></param>
        private void BtnInvoke_Click(object sender, EventArgs e)
        {
            string[] parameters = new string[method.Parameters.Count];
            for (int i = 0; i < parameters.Length; i++)
                parameters[i] = method.Parameters[i].Value.ToString();

            method.Invoke(parameters);
        }

        private void MethodControl_Paint(object sender, PaintEventArgs e)
            => ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, Colors.BackgroundColor, ButtonBorderStyle.Solid);
    }
}

// TODO: Improve UI for MethodControl
/* Add new UserControls in order to have a better UI in respect to the type of the parameter, e.g.
 * - A "MyUserComponent" that inherit from UserComponent and add a Value property
 *   with the actual value
 * All the other new UserComponents will inherit from "MyUserComponent".
 * - A "DigitalButton" for boolean parameters
 * - A "NumericTextBox" for integer and real numbers (the UserControl may have a format
 *   to distinguish between the type of the number?)
 * - A "StringComponent" for string parameters (inherit from TextBox and unify its style?)
 *
 * With all the new UserComponents (with a Value property with the actual value inserted by
 * the user), txbValues should become a list of "MyUserComponent" and then:
 * components?.ElementAt(i)?.Value
 */
using Core.DataStructures;
using Core.Scheduling;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        private const int controlLocationOffset = 6;
        private const int textBoxSize = 25;

        private Size size;

        private Method method;
        private MethodScheduler scheduler;
        private List<BaseControl> values = new List<BaseControl>();

        /// <summary>
        /// Create a new instance of the class <see cref="MethodControl"/>
        /// </summary>
        /// <param name="method">The <see cref="Method"/></param>
        /// <param name="scheduler">The <see cref="MethodScheduler"/></param>
        public MethodControl(Method method, MethodScheduler scheduler)
        {
            InitializeComponent();

            size = Size;
            btnAdd.Image = Properties.Resources.ImageAdd;

            this.method = method;
            var pars = method.Parameters;
            this.scheduler = scheduler;

            lblMethodName.Text = method.Name;

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
                foreach (var p in pars)
                {
                    lblParam = new Label();
                    lblParam.AutoSize = true;
                    lblParam.Text = p.Name;
                    lblParam.Location = new Point(
                        lblMethodNamePlaceholder.Location.X + 10,
                        lblMethodNamePlaceholder.Location.Y + 10
                            + index++ * (lblMethodNamePlaceholder.Size.Height + controlLocationOffset)
                    );
                    lblParam.Size = lblMethodNamePlaceholder.Size;

                    if (p.Type != ParameterType.Bool)
                    {
                        var txtCtrl = new TextControl();
                        txtCtrl.Location = new Point(
                            lblMethodName.Location.X,
                            lblParam.Location.Y - 4
                        );

                        AddElementToCollection(lblParam, txtCtrl);
                    }
                    else
                    {
                        var diCtrl = new DigitalControl();
                        diCtrl.Location = new Point(
                            lblMethodName.Location.X,
                            lblParam.Location.Y - 4
                        );

                        AddElementToCollection(lblParam, diCtrl);
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
            for (int i = 0; i < method.ParametersCount; i++)
                method.Parameters.ElementAt(i).Value = values.ElementAt(i).Value;

            scheduler.AddElement(method);
        }
    }
}

// TODO
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
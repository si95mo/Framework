using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace UserInterface.Controls.Panels
{
    /// <summary>
    /// Define a panel that can be used to build a custom navbar
    /// </summary>
    public partial class PanelNavbar : UserControl
    {
        private const int LabelSpacing = 8;

        /// <summary>
        /// The set of all the user controls
        /// </summary>
        public Dictionary<string, Control> UserControls { get; }

        /// <summary>
        /// The collection of <see cref="LabelControl"/>
        /// </summary>
        public List<LabelControl> Labels { get; protected set; }

        /// <summary>
        /// The <see cref="PanelNavbar"/> container <see cref="Form"/>
        /// </summary>
        public Form ContainerForm { get; set; }

        /// <summary>
        /// The <see cref="Panels.ActionPanel"/>
        /// </summary>
        public ActionPanel ActionPanel { get; set; }

        /// <summary>
        /// The layout panel <see cref="Size"/>
        /// </summary>
        [Description("Layout panel size"), Category("Data")]
        public Size LayoutSize { get => LayoutPanel.Size; set => LayoutPanel.Size = value; }

        /// <summary>
        /// The <see cref="Control.ControlCollection"/> with all the displayabale <see cref="UserControl"/>
        /// </summary>
        public ControlCollection LayoutControls => LayoutPanel.Controls;

        /// <summary>
        /// The version <see cref="LabelControl"/>. Can be used to add info the default assembly version
        /// </summary>
        public LabelControl VersionLabel => LblVersion;

        /// <summary>
        /// The actual user control
        /// </summary>
        public Control ActualUserControl
        {
            get => actualUserControl;
            set
            {
                if (value != actualUserControl)
                {
                    actualUserControl = value;
                    OnActualUserControlChanged();
                }
            }
        }

        /// <summary>
        /// Fired when the <see cref="ActualUserControl"/> changes
        /// </summary>
        public event ActualUserControlHandler ActualUserControlChanged;

        public delegate void ActualUserControlHandler(object source, ActualUserControlEventArgs e);

        protected virtual void OnActualUserControlChanged()
        {
            ActualUserControlChanged?.Invoke(this, new ActualUserControlEventArgs(ActualUserControl));
        }

        private Control actualUserControl;

        /// <summary>
        /// Create a new instance of <see cref="PanelNavbar"/>
        /// </summary>
        public PanelNavbar()
        {
            InitializeComponent();

            AutoScaleMode = AutoScaleMode.Inherit;
            LayoutPanel.BackColor = Colors.Grey;

            VersionLabel.Text = Assembly.GetEntryAssembly().GetName().Version.ToString();
            if (!VersionLabel.Text.StartsWith("v"))
                VersionLabel.Text = $"v{VersionLabel.Text}";

            UserControls = new Dictionary<string, Control>();
            Labels = new List<LabelControl>();
            ActualUserControl = new Panel()
            {
                BackColor = Color.Red,
                Size = new Size(100, 100),
                Location = new Point(100, 100)
            };

            ActionPanel = new ActionPanel();
        }

        /// <summary>
        /// Add a new <see cref="Control"/> to the navbar
        /// </summary>
        /// <param name="text">The text to display in the navbar</param>
        /// <param name="control">The relative <see cref="Control"/></param>
        public void Add(string text, Control control)
        {
            if (UserControls.ContainsKey(text))
            {
                UserControls[text] = control;
            }
            else
            {
                UserControls.Add(text, control);

                LabelControl labelControl = new LabelControl()
                {
                    Text = text,
                    AutoSize = false,
                    Anchor = AnchorStyles.None,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size = new Size(128, LayoutPanel.Size.Height - 8),
                    BackColor = Colors.Transparent
                };
                labelControl.Click += LabelControl_Click;

                Labels.Add(labelControl);
                LayoutPanel.Controls.Add(labelControl);

                // Calculate the total width of the labels
                int totalLabelsWidth = LayoutPanel.Controls.Cast<Control>().Sum((x) => x.Width) + (LayoutPanel.Controls.Count - 1) * LabelSpacing;

                // Calculate the X position to center the set of labels within the panel
                int labelsX = (LayoutPanel.Width - totalLabelsWidth) / 2;

                // Set the location of each label within the panel
                int labelY = (LayoutPanel.Height - labelControl.Height) / 2;
                foreach (LabelControl label in LayoutPanel.Controls)
                {
                    label.Location = new Point(labelsX, labelY);
                    labelsX += label.Width + LabelSpacing;
                }

                UpdateLabels(labelControl);
            }

            if (UserControls.Count == 1) // Only one control at the moment
            {
                Size actualSize = control.Size;

                ActualUserControl = control; // So set this as the actual one also
                ActualUserControl.Size = actualSize;
            }
        }

        /// <summary>
        /// Set the actual <see cref="LabelControl"/> for the <see cref="ActualUserControl"/>
        /// </summary>
        /// <param name="index">The index of the label to set as default"/></param>
        public void SetActualLabel(int index = 0)
        {
            LabelControl label = Labels[index];
            UpdateLabels(label);
        }

        #region Event handlers

        private void LabelControl_Click(object sender, EventArgs e)
        {
            UpdateLabels(sender as LabelControl); // Bold style on the selected labels
        }

        #region PbxAction

        private void PbxAction_Click(object sender, EventArgs e)
        {

        }

        private void PbxAction_MouseDown(object sender, MouseEventArgs e)
        {
            SetColor(PbxAction, ControlPaint.Light(Colors.Grey)); // Change back color for the close control
        }

        private void PbxAction_MouseUp(object sender, MouseEventArgs e)
        {
            SetColor(PbxAction, Colors.Grey); // Reset back color for the close control
        }

        #endregion PbxAction

        #region PbxClose

        private void PbxClose_Click(object sender, EventArgs e)
        {
            ContainerForm?.Close(); // Close the container form
        }

        private void PbxClose_MouseDown(object sender, MouseEventArgs e)
        {
            SetColor(PbxClose, ControlPaint.Light(Colors.Grey)); // Change back color for the close control
        }

        private void PbxClose_MouseUp(object sender, MouseEventArgs e)
        {
            SetColor(PbxClose, Colors.Grey); // Reset back color for the close control
        }

        #endregion PbxClose

        #endregion Event handlers

        /// <summary>
        /// Update the labels (set the correct <see cref="ActualUserControl"/> and the bold style in the navbar)
        /// </summary>
        /// <param name="sender">The sender <see cref="Label"/> (<see cref="LabelControl"/> at runtime)</param>
        private void UpdateLabels(Label sender)
        {
            foreach (LabelControl label in LayoutPanel.Controls)
            {
                label.Font = new Font(label.Font, FontStyle.Regular);
                label.BackColor = Color.Transparent;
                label.BorderStyle = BorderStyle.None;
            }

            LabelControl castedSender = sender as LabelControl;
            castedSender.Font = new Font(castedSender.Font, FontStyle.Bold);
            castedSender.BackColor = ControlPaint.Light(BackColor);
            castedSender.BorderStyle = BorderStyle.FixedSingle;

            ActualUserControl = UserControls[castedSender.Text];
        }

        private void SetColor(PictureBox pictureBox, Color color)
        {
            pictureBox.BackColor = color;
        }
    }

    /// <summary>
    /// Define the event arguments for the actual control changed
    /// </summary>
    public class ActualUserControlEventArgs : EventArgs
    {
        /// <summary>
        /// The control involved
        /// </summary>
        public Control Control { get; set; }

        /// <summary>
        /// Create a new instance of <see cref="ActualUserControlEventArgs"/>
        /// </summary>
        /// <param name="control">The <see cref="Control"/> involved</param>
        public ActualUserControlEventArgs(Control control)
        {
            Control = control;
        }
    }
}
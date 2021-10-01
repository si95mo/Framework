using System;
using System.Drawing;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    /// <summary>
    /// Define the type of the <see cref="Image"/>
    /// </summary>
    public enum AutomaticActionImage
    {
        /// <summary>
        /// Execute image
        /// </summary>
        Execute = 0,

        /// <summary>
        /// Save image
        /// </summary>
        Save = 1,

        /// <summary>
        /// Load image
        /// </summary>
        Load = 2,

        /// <summary>
        /// Clear image
        /// </summary>
        Clear = 3
    }

    public partial class BaseAutomaticButton : Button
    {
        private Image image;
        private AutomaticActionImage buttonImage;

        /// <summary>
        /// The click <see cref="EventHandler"/>
        /// </summary>
        public new event EventHandler Click
        {
            add
            {
                base.Click += value;
                foreach (Control control in Controls)
                    control.Click += value;
            }
            remove
            {
                base.Click -= value;
                foreach (Control control in Controls)
                    control.Click -= value;
            }
        }

        /// <summary>
        /// The <see cref="AutomaticActionImage"/>
        /// </summary>
        public AutomaticActionImage ButtonImage
        {
            get => buttonImage;
            set
            {
                buttonImage = value;
                switch (buttonImage)
                {
                    case AutomaticActionImage.Execute:
                        image = Properties.Resources.ImageExecute;
                        break;

                    case AutomaticActionImage.Save:
                        image = Properties.Resources.ImageSave;
                        break;

                    case AutomaticActionImage.Load:
                        image = Properties.Resources.ImageLoad;
                        break;

                    case AutomaticActionImage.Clear:
                        image = Properties.Resources.ImageClear;
                        break;

                    default:
                        // Nothing to do
                        break;
                }

                btnAction.Image = image;
            }
        }

        /// <summary>
        /// Create a new instance of <see cref="BaseAutomaticButton"/>
        /// </summary>
        public BaseAutomaticButton()
        {
            InitializeComponent();

            btnAction.BackColor = Colors.BackgroundColor;
            btnAction.Size = new Size(100, 100);

            btnAction.FlatStyle = FlatStyle.Flat;
            btnAction.FlatAppearance.BorderColor = Colors.Black;

            buttonImage = AutomaticActionImage.Execute;
            image = Properties.Resources.ImageExecute;
            btnAction.Image = image;
        }
    }
}
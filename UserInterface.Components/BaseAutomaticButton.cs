using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    public enum AutomaticActionImage
    {
        Execute = 0,
        Save = 1,
        Load = 2,
        Clear = 3
    }

    public partial class BaseAutomaticButton : Button
    {
        private Image image;
        private AutomaticActionImage buttonImage;

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

using Core;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    public class LabelControl : Label
    {
        public LabelControl() : base()
        {
            Font = new Font("Lucida Sans Unicode", 12);
        }

        /// <summary>
        /// Set the actual <see cref="IProperty{T}"/> that will be connected to <see langword="this"/> <see cref="LabelControl"/>
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IProperty{T}"/> to connect</typeparam>
        /// <param name="property">The <see cref="IProperty{T}"/> to connect</param>
        public void SetProperty<T>(IProperty<T> property)
        {
            Text = property.ToString();
            property.ValueChanged += (s, e) =>
            {
                if (!InvokeRequired)
                {
                    Text = property.ToString();
                }
                else
                {
                    BeginInvoke(new Action(() => Text = property.ToString()));
                }
            };
        }
    }
}
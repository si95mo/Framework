using System;
using System.Drawing;

namespace UserInterface.Controls
{
    public partial class PanelControl : BaseControl
    {
        protected string code;

        /// <summary>
        /// The <see cref="PanelControl"/> code
        /// </summary>
        public string Code => code;

        public PanelControl() : this(Guid.NewGuid().ToString())
        { }

        public PanelControl(string code)
        {
            this.code = code;

            Location = new Point(0, 0);
            Visible = false;
            Enabled = false;

            SendToBack();
        }
    }
}
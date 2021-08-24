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

        /// <summary>
        /// Create a new instance of <see cref="PanelControl"/>
        /// </summary>
        public PanelControl() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Create a new instance of <see cref="PanelControl"/>
        /// </summary>
        /// <param name="code">The code</param>
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
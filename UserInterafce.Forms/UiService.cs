using Core;
using Core.DataStructures;

namespace UserInterface.Forms
{
    /// <summary>
    /// Define the <see cref="Toaster"/> type
    /// </summary>
    public enum ToasterType
    {
        Message = 0,
        Warning = 1,
        Error = 2
    }

    /// <summary>
    /// Define a <see cref="Service{T}"/> for UI
    /// </summary>
    public class UiService : Service<IProperty>
    {
        /// <summary>
        /// The parent <see cref="CustomForm"/>
        /// </summary>
        public CustomForm Form { get; private set; }

        /// <summary>
        /// Create a new instance of <see cref="UiService"/>
        /// </summary>
        /// <param name="form">The parent <see cref="CustomForm"/></param>
        public UiService(CustomForm form) : base()
        {
            Form = form;
        }

        /// <summary>
        /// Create a new instance of <see cref="CustomForm"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="form">The parenct <see cref="CustomForm"/></param>
        public UiService(string code, CustomForm form) : base(code)
        {
            Form = form;
        }

        /// <summary>
        /// Show a <see cref="Toaster"/> to screen
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="toasterType">The <see cref="ToasterType"/></param>
        public void ShowToaster(string message, ToasterType toasterType)
        {
            Toaster toaster = new Toaster();
            Toaster.ShowToaster(toaster, message, toasterType, Form);
        }
    }
}

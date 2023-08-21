using Core;
using Core.DataStructures;

namespace UserInterface.Forms
{
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
    }
}

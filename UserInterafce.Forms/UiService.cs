using Core;
using Core.DataStructures;
using System;
using System.Collections.Generic;

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
        /// The actual number of <see cref="Toaster"/> shown
        /// </summary>
        internal static int ToasterCounter { get; private set; } = 0;

        private static readonly object sync = new object();
        private static readonly List<Toaster> activeToasters = new List<Toaster>();

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
        /// <param name="form">The parent <see cref="CustomForm"/></param>
        public UiService(string code, CustomForm form) : base(code)
        {
            Form = form;
        }

        /// <summary>
        /// Show a <see cref="Toaster"/> to screen
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="toasterType">The <see cref="ToasterType"/></param>
        /// <param name="displayDurationInMilliseconds">The <see cref="Toaster"/> display duration, in milliseconds</param>
        public void ShowToaster(string message, ToasterType toasterType, int displayDurationInMilliseconds = 10000)
        {
            Toaster toaster;
            lock (sync)
            {
                toaster = new Toaster();
                activeToasters.Add(toaster);

                ToasterCounter++;
            }

            toaster.FormClosed += (s, e) =>
            {
                lock(sync)
                {
                    Toaster toasterToRemove = s as Toaster;
                    activeToasters.Remove(toasterToRemove);

                    ToasterCounter--;

                    int toasterCounter = 1;
                    foreach(Toaster activeToaster in activeToasters)
                    {
                        activeToaster.MoveToLocation(Form, toasterCounter++);
                    }
                }
            };

            Toaster.ShowToaster(toaster, message, toasterType, Form, displayDurationInMilliseconds);
        }

        /// <summary>
        /// Show a <see cref="Toaster"/> to screen
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="toasterType">The <see cref="ToasterType"/></param>
        /// <param name="displayDuration">The <see cref="Toaster"/> display duration <see cref="TimeSpan"/></param>
        public void ShowToaster(string message, ToasterType toasterType, TimeSpan displayDuration)
            => ShowToaster(message, toasterType, (int)displayDuration.TotalMilliseconds);
    }
}
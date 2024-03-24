﻿using Core;
using Core.DataStructures;
using Diagnostic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UserInterface.Controls.Panels;

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

        /// <summary>
        /// Define if the main <see cref="PanelNavbar"/>
        /// </summary>
        internal static PanelNavbar Navbar { get; private set; } = null;

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
        public UiService(CustomForm form) : this(Guid.NewGuid().ToString(), form)
        { }

        /// <summary>
        /// Create a new instance of <see cref="CustomForm"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="form">The parent <see cref="CustomForm"/></param>
        public UiService(string code, CustomForm form) : base(code)
        {
            Form = form;
            Form.FormClosed += Form_FormClosed;
        }

        #region Toasters

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

            toaster.FormClosed += (s, e) => RemoveToaster(s as Toaster);

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

        /// <summary>
        /// Remove a <see cref="Toaster"/> from the active ones
        /// </summary>
        /// <param name="toaster">The <see cref="Toaster"/> to remove</param>
        private void RemoveToaster(Toaster toaster)
        {
            lock (sync)
            {
                activeToasters.Remove(toaster);

                ToasterCounter--;

                int toasterCounter = 1;
                foreach (Toaster activeToaster in activeToasters)
                {
                    activeToaster.MoveToLocation(Form, toasterCounter++);
                }
            }
        }

        #endregion Toasters

        #region Panels

        /// <summary>
        /// Add the navbar to the main <see cref="Form"/>
        /// </summary>
        public void AddNavbar()
        {
            if(Navbar == null)
            {
                Navbar = new PanelNavbar();
                Navbar.ContainerForm = Form;
                Navbar.Location = new Point(0, Form.Height - Navbar.Height);

                if(Form.Width >= Navbar.Width)
                {
                    Form.Controls.Add(Navbar);
                }
                else
                {
                    Logger.Info($"Unable to add the navbar to the main form, incompatible size detected. Main form size is ({Form.Height}, {Form.Width}) and its width must be >= {Navbar.Width}");
                }
            }
            else
            {
                Logger.Info("Navbar alredy added to the main form, unable to add another one");
            }
        }

        /// <summary>
        /// Add a new <see cref="PanelControl"/> to the main <see cref="PanelNavbar"/>
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="panel">The <see cref="PanelControl"/> to add</param>
        public void AddPanel(string text, PanelControl panel)
        {
            Navbar.Add(text, panel);
        }

        #endregion Panels

        #region Event handlers

        private void Form_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            IEnumerable<IService> services = ServiceBroker.GetServices();
            int count = services.Count();

            int counter = 0;
            foreach(IService service in services)
            {
                Logger.Info($"Disposing service ({++counter}/{count}) of type \"{service.GetType().Name}\"");
                service.Dispose();
            }

            Logger.Warn("All services disposed");
            Logger.Warn("Main form is closed");

            Form.FormClosed -= Form_FormClosed;
        }

        #endregion Event handlers
    }
}
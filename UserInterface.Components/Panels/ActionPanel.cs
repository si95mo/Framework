using Core.Extensions;
using System;
using System.Windows.Forms;

namespace UserInterface.Controls.Panels
{
    public partial class ActionPanel : UserControl
    {
        public ActionPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Add a new action to the <see cref="ActionPanel"/>
        /// </summary>
        /// <param name="control">The <see cref="Control"/> to add and display</param>
        /// <param name="action">The <see cref="Action"/> invoke on <paramref name="control"/> <see cref="Control.Click"/> event</param>
        public void AddAction(Control control, Action action)
        {
            layoutPanel.Controls.Add(control);
            control.Click += delegate
            {
                action();
            };
        }

        /// <summary>
        /// Clear all the <see cref="Control"/> associated with the <see cref="ActionPanel"/>
        /// </summary>
        /// <remarks>Also clear all the <see cref="Control.Click"/> events!</remarks>
        public void Clear()
        {
            foreach (Control control in layoutPanel.Controls)
            {
                control.ClearEventInvocations(nameof(Click));
            }
            layoutPanel.Controls.Clear();
        }

        /// <summary>
        /// Remove the specified <paramref name="control"/> from the <see cref="ActionPanel"/>
        /// </summary>
        /// <remarks>Also clear all the <see cref="Control.Click"/> events!</remarks>
        /// <param name="control">The <see cref="Control"/> to remove</param>
        public void Remove(Control control)
        {
            if (layoutPanel.Controls.Contains(control))
            {
                layoutPanel.Controls.Remove(control);
                control.ClearEventInvocations(nameof(Click));
            }
        }
    }
}

﻿using Extensions;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace UserInterface.Dashboards
{
    /// <summary>
    /// Implement an <see cref="IDashboardControl"/> item creation <see cref="UserControl"/>
    /// </summary>
    public partial class DashboardItemControl : UserControl
    {
        private Panel dashboard, configPanel;

        /// <summary>
        /// Create a new instance of <see cref="DashboardItemControl"/>
        /// </summary>
        /// <param name="dashboard"></param>
        /// <param name="configPanel"></param>
        public DashboardItemControl(Panel dashboard, Panel configPanel)
        {
            InitializeComponent();

            this.dashboard = dashboard;
            this.configPanel = configPanel;

            AnalogReadControl analogReadControl = new AnalogReadControl();
            analogReadControl.DoubleClick += AnalogReadControl_DoubleClick;
            analogReadControl.SetDraggable(false);
            layoutPanel.Controls.Add(analogReadControl);

            AnalogWriteControl analogWriteControl = new AnalogWriteControl();
            analogWriteControl.DoubleClick += AnalogWriteControl_DoubleClick;
            analogWriteControl.SetDraggable(false);
            layoutPanel.Controls.Add(analogWriteControl);

            DigitalReadControl digitalReadControl = new DigitalReadControl();
            digitalReadControl.DoubleClick += DigitalReadControl_DoubleClick;
            digitalReadControl.SetDraggable(false);
            layoutPanel.AddControl(digitalReadControl);

            DigitalWriteControl digitalWriteControl = new DigitalWriteControl();
            digitalWriteControl.DoubleClick += DigitalWriteControl_DoubleClick;
            digitalWriteControl.SetDraggable(false);
            layoutPanel.AddControl(digitalWriteControl);

            MultiSampleAnalogReadControl multiSampleReadControl = new MultiSampleAnalogReadControl();
            multiSampleReadControl.DoubleClick += MultiSampleReadControl_DoubleClick;
            multiSampleReadControl.SetDraggable(false);
            layoutPanel.AddControl(multiSampleReadControl);
        }

        private void AnalogReadControl_DoubleClick(object sender, EventArgs e)
        {
            AnalogReadControl control = new AnalogReadControl();
            HandleNewControl(control);
        }

        private void AnalogWriteControl_DoubleClick(object sender, EventArgs e)
        {
            AnalogWriteControl control = new AnalogWriteControl();
            HandleNewControl(control);
        }

        private void DigitalReadControl_DoubleClick(object sender, EventArgs e)
        {
            DigitalReadControl control = new DigitalReadControl();
            HandleNewControl(control);
        }

        private void DigitalWriteControl_DoubleClick(object sender, EventArgs e)
        {
            DigitalWriteControl control = new DigitalWriteControl();
            HandleNewControl(control);
        }

        private void MultiSampleReadControl_DoubleClick(object sender, EventArgs e)
        {
            MultiSampleAnalogReadControl control = new MultiSampleAnalogReadControl();
            HandleNewControl(control);
        }

        /// <summary>
        /// Handle the new created <see cref="IDashboardControl"/>
        /// </summary>
        /// <param name="control">The <see cref="IDashboardControl"/> to handle</param>
        private void HandleNewControl(IDashboardControl control)
        {
            (control as DraggableControl).SetDraggable(true);
            (control as DraggableControl).Click += (_, __) =>
            {
                configPanel.Controls.Clear();
                configPanel.Controls.Add(new ItemConfigurationControl(control));
            };

            dashboard.Controls.Add(control as DraggableControl);
        }
    }
}
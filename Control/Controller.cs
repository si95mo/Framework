﻿using Hardware;
using System;

namespace Control
{
    public abstract class Controller : IController
    {
        protected string code;
        protected Channel<double> rk;
        protected AnalogOutput uk;
        protected double setPoint;

        /// <summary>
        /// The code
        /// </summary>
        public string Code => code;

        /// <summary>
        /// The value as object
        /// </summary>
        public object ValueAsObject
        {
            get => this;
            set => _ = value;
        }

        /// <summary>
        /// The <see cref="System.Type"/>
        /// </summary>
        public Type Type => typeof(Controller);

        /// <summary>
        /// The actual set point
        /// </summary>
        public double SetPoint
        {
            get => setPoint;
            set => setPoint = value;
        }

        /// <summary>
        /// Create a new instance of <see cref="Controller"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="u">The controlled variable</param>
        /// <param name="setPoint">The desired set point</param>
        protected Controller(string code, Channel<double> u, double setPoint)
        {
            this.code = code;
            uk = new AnalogOutput($"Y.{code}", measureUnit: u.MeasureUnit, format: u.Format);
            this.rk = u;
            this.setPoint = setPoint;
        }

        /// <summary>
        /// Start the control algorithm
        /// </summary>
        public abstract void Start();
    }
}
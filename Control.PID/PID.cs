﻿using Core;
using Core.Threading;
using Hardware;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Control.PID
{
    /// <summary>
    /// Implement a (P)roportional (I)ntegrative (D)erivative controller
    /// </summary>
    public class PID : Controller
    {
        private int n;
        private double kp, ki, kd;
        private double upperLimit, lowerLimit;
        private double cycleTime;

        private TimeSpan timeSinceLastUpdate;

        private double integralTerm;
        private double proportionalTerm;
        private double derivativeTerm;

        private double lastControlledValue;

        private Task controlTask;

        /// <summary>
        /// The filter coefficient
        /// </summary>
        public int N { get => n; set => n = value; }

        /// <summary>
        /// The proportional gain
        /// </summary>
        public double Kp { get => kp; set => kp = value; }

        /// <summary>
        /// The integral gain
        /// </summary>
        public double Ki { get => ki; set => ki = value; }

        /// <summary>
        /// The derivative gain
        /// </summary>
        public double Kd { get => kd; set => kd = value; }

        /// <summary>
        /// The clamping upper output limit of the controller
        /// </summary>
        public double UpperLimit { get => upperLimit; set => upperLimit = value; }

        /// <summary>
        /// The clamping lower output limit of the controller
        /// </summary>
        public double LowerLimit { get => lowerLimit; set => lowerLimit = value; }

        /// <summary>
        /// The controller output (as an <see cref="AnalogOutput"/>
        /// </summary>
        public AnalogOutput Output => output;

        /// <summary>
        /// The controlled variable (as a generic <see cref="Channel{T}"/>
        /// </summary>
        public Channel<double> U => u;

        /// <summary>
        /// The cycle time of the controller (in milliseconds)
        /// </summary>
        public double CycleTime
        {
            get => cycleTime;
            set => cycleTime = value;
        }

        /// <summary>
        /// Create a new instance of <see cref="PID"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="u">The controlled variable</param>
        /// <param name="kp">The proportional gain</param>
        /// <param name="ki">The integral gain</param>
        /// <param name="kd">The derivative gain</param>
        /// <param name="upperLimit">The upper limit (for clamping)</param>
        /// <param name="lowerLimit">The lower limit (for clamping)</param>
        /// <param name="setPoint">The desired set point</param>
        public PID(string code, Channel<double> u, int n, double kp, double ki, double kd,
            double upperLimit, double lowerLimit, double setPoint) : base(code, u, setPoint)
        {
            this.n = n;
            this.kp = kp;
            this.ki = ki;
            this.kd = kd;
            this.upperLimit = upperLimit;
            this.lowerLimit = lowerLimit;

            controlTask = null;

            this.u.ValueChanged += ControlledVariable_ValueChanged;
        }

        private void ControlledVariable_ValueChanged(object sender, ValueChangedEventArgs e)
            => lastControlledValue = (double)e.OldValue;

        /// <summary>
        /// Create a new controlling <see cref="Task"/>
        /// </summary>
        /// <returns>The controlling <see cref="Task"/></returns>
        private Task CreateControlTask() => new Task(async () =>
                {
                    Stopwatch sw;
                    int timeToWait;

                    timeSinceLastUpdate = new TimeSpan(0);
                    while (true)
                    {
                        sw = Stopwatch.StartNew();

                        Iterate();

                        timeToWait = (int)(cycleTime - sw.Elapsed.TotalMilliseconds);

                        if (timeToWait > 0)
                            await Tasks.NoOperation(timeToWait, 1);

                        timeSinceLastUpdate = new TimeSpan(sw.Elapsed.Ticks).Subtract(timeSinceLastUpdate);
                    }
                }
            );

        /// <summary>
        /// Start the PID controller (or restart it if already started)
        /// </summary>
        public override void Start()
        {
            if (controlTask == null)
            {
                controlTask = CreateControlTask();
                controlTask.Start();
            }
            else
            {
                controlTask.Wait((int)cycleTime);
                controlTask.Dispose();

                controlTask = CreateControlTask();
                controlTask.Start();
            }
        }

        /// <summary>
        /// Perform an iteration of the control algorithm
        /// </summary>
        private void Iterate()
        {
            double error = setPoint - u.Value;

            // Integral term
            integralTerm += (ki * error * timeSinceLastUpdate.TotalSeconds);
            integralTerm = Clamp(integralTerm);

            // Derivative term
            // double dInput = u.Value - lastControlledValue;
            derivativeTerm = kd * (n / (1 + n * integralTerm));

            // Proportional term
            proportionalTerm = kp * error;

            // Output update
            output.Value = proportionalTerm + integralTerm - derivativeTerm;
            output.Value = Clamp(output.Value);
        }

        /// <summary>
        /// Clamp a variable based on <see cref="UpperLimit"/>
        /// and <see cref="LowerLimit"/>
        /// </summary>
        /// <param name="valueToClamp">The value to clamp</param>
        /// <returns>The clamped value</returns>
        private double Clamp(double valueToClamp)
        {
            double result = valueToClamp;

            if (valueToClamp <= lowerLimit)
                result = lowerLimit;
            if (valueToClamp >= upperLimit)
                result = upperLimit;

            return result;
        }
    }
}
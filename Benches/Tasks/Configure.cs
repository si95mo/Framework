﻿using Core.Parameters;
using Devices;
using Instructions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Benches.Tasks
{
    public class Configure<TDevice, TParameter, TInstruction>
    {
        private readonly List<IDevice> devices;
        private readonly List<IParameter> parameters;
        private readonly List<IInstruction> instructions;

        private readonly Bench<TDevice, TParameter, TInstruction> bench;

        /// <summary>
        /// Create a new instance of <see cref="Tasks.Configure{TDevice, TParameter, TInstruction}"/>
        /// </summary>
        /// <param name="bench">The <see cref="Bench{TDevice, TParameter, TInstruction}"/> to configure</param>
        public Configure(Bench<TDevice, TParameter, TInstruction> bench)
        {
            this.bench = bench;
            Type deviceType = typeof(TDevice);
            Type parameterType = typeof(TParameter);
            Type instructionType = typeof(TInstruction);

            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

            var ctors = deviceType.GetConstructors(flags);
            var channel = ctors[0].Invoke(new object[] { });

            //devices = deviceType.GetProperties(flags)
            //    .Select(
            //        p => (IDevice)p.GetValue(channel, null)
            //    ).ToList();

            //ctors = parameterType.GetConstructors(flags);
            //var parameter = ctors[0].Invoke(new object[] { });

            //parameters = parameterType.GetProperties(flags)
            //    .Select(
            //        p => (IParameter)p.GetValue(parameter, null)
            //    ).ToList();
        }

        /// <summary>
        /// Execute the configuration of the <see cref="Bench{TDevice, TParameter, TInstruction}"/>
        /// </summary>
        public void Execute()
        {
            //foreach (IChannel channel in devices)
            //    bench.Devices.Add(channel);

            //foreach (IParameter parameter in parameters)
            //    bench.Parameters.Add(parameter);
        }
    }
}
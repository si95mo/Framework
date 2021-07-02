﻿using Core;
using Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Benches.Tasks
{
    internal class Configure<TDevice, TParameter>
    {
        private List<IDevice> devices;
        private List<IParameter> parameters;

        private Bench<TDevice, TParameter> bench;

        /// <summary>
        /// Create a new instance of <see cref="Configure"/>
        /// </summary>
        /// <param name="bench">The <see cref="Bench{TDevice, TParameter}"/> to configure</param>
        public Configure(Bench<TDevice, TParameter> bench)
        {
            this.bench = bench;
            Type deviceType = typeof(TDevice);
            Type parameterType = typeof(TParameter);

            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

            var ctors = deviceType.GetConstructors(flags);
            var channel = ctors[0].Invoke(new object[] { });

            devices = deviceType.GetProperties(flags)
                .Select(
                    p => (IDevice)p.GetValue(channel, null)
                ).ToList();

            ctors = parameterType.GetConstructors(flags);
            var parameter = ctors[0].Invoke(new object[] { });

            parameters = parameterType.GetProperties(flags)
                .Select(
                    p => (IParameter)p.GetValue(parameter, null)
                ).ToList();
        }

        /// <summary>
        /// Execute the configuration of the <see cref="Device"/>
        /// </summary>
        public void Execute()
        {
            foreach (IChannel channel in devices)
                bench.Devices.Add(channel);

            foreach (IParameter parameter in parameters)
                bench.Parameters.Add(parameter);
        }
    }
}
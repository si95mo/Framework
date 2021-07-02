using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Devices.Tasks
{
    internal class Configure<TChannel, TParameter>
    {
        private List<IChannel> channels;
        private List<IParameter> parameters;

        private Device<TChannel, TParameter> device;

        /// <summary>
        /// Create a new instance of <see cref="Configure"/>
        /// </summary>
        /// <param name="device">The <see cref="Device{TChannel, TParameter}"/> to configure</param>
        public Configure(Device<TChannel, TParameter> device)
        {
            this.device = device;
            Type channelType = typeof(TChannel);
            Type parameterType = typeof(TParameter);

            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

            var ctors = channelType.GetConstructors(flags);
            var channel = ctors[0].Invoke(new object[] { });

            channels = channelType.GetProperties(flags)
                .Select(
                    p => (IChannel)p.GetValue(channel, null)
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
            foreach (IChannel channel in channels)
                device.Channels.Add(channel);

            foreach (IParameter parameter in parameters)
                device.Parameters.Add(parameter);
        }
    }
}
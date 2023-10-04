using Core.Conditions;
using Core.DataStructures;
using Hardware;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Diagnostic.Messages
{
    /// <summary>
    /// Define the type of compare to use when firing an alarm
    /// </summary>
    public enum ChannelCompareType
    {
        /// <summary>
        /// The comparison will be with "<c>=</c>"
        /// </summary>
        IsEqual = 0,

        /// <summary>
        /// The comparison will be with "<c>!=</c>"
        /// </summary>
        IsNotEqual = 1
    }

    /// <summary>
    /// Define an <see cref="Alarm"/> monitor for hardware related properties
    /// </summary>
    public static class HardwareAlarmMonitor
    {
        /// <summary>
        /// The collection of the <see cref="IDiagnosticMessage"/> (i.e. <see cref="Alarm"/> contained in the <see cref="HardwareAlarmMonitor"/>
        /// </summary>
        public static IEnumerable<IDiagnosticMessage> Alarms => service.GetAll().Cast<IDiagnosticMessage>();

        private static DiagnosticMessagesService service;
        private static Alarm resourcesWithSameCode = null;

        /// <summary>
        /// Initialize the <see cref="HardwareAlarmMonitor"/>
        /// </summary>
        public static void Initialize()
        {
            if (ServiceBroker.CanProvide<DiagnosticMessagesService>())
            {
                service = ServiceBroker.GetService<DiagnosticMessagesService>();
            }
            else
            {
                service = new DiagnosticMessagesService();
                Logger.Warn($"{nameof(DiagnosticMessagesService)} not provided by the {nameof(ServiceBroker)}. {nameof(HardwareAlarmMonitor)} will use its own");
            }
        }

        /// <summary>
        /// Add a new <see cref="Alarm"/> for a collection of <see cref="IResource"/> as an <see langword="array"/> of <see cref="IResource"/>
        /// (based on <see cref="IResource.Status"/> equal to <see cref="ResourceStatus.Failure"/>)
        /// </summary>
        /// <param name="resources">The <see cref="IResource"/> collection</param>
        public static void AddAlarm(params IResource[] resources)
        {
            ICondition condition;
            Alarm alarm;
            foreach (IResource resource in resources)
            {
                condition = resource.Status.IsEqualTo(ResourceStatus.Failure);

                alarm = Alarm.New($"{resource.Code}.{nameof(Alarm)}", $"Resource {resource.Code} in failure state", condition, string.Empty);
                service.Add(alarm);
            }
        }

        /// <summary>
        /// Add a new <see cref="Alarm"/> for a collection of <see cref="IResource"/> as an <see cref="IEnumerable{T}"/>
        /// (based on <see cref="IResource.Status"/> equal to <see cref="ResourceStatus.Failure"/>). See also <see cref="AddAlarm(IResource[])"/>
        /// </summary>
        /// <param name="resources">The <see cref="IResource"/> collection</param>
        public static void AddAlarm(IEnumerable<IResource> resources)
            => AddAlarm(resources.ToArray());

        /// <summary>
        /// Add a new <see cref="Alarm"/> for all the <see cref="IResource"/> contained in the <see cref="ResourcesService"/>
        /// (based on <see cref="IResource.Status"/> equal to <see cref="ResourceStatus.Failure"/>)
        /// </summary>
        /// <remarks>
        /// <see cref="ResourcesService"/> should be provided first!
        /// </remarks>
        public static void AddAlarm()
        {
            if(ServiceBroker.CanProvide<ResourcesService>())
            {
                ResourcesService resourcesService = ServiceBroker.GetService<ResourcesService>();
                AddAlarm(resourcesService.GetAll().Cast<IResource>());
            }
            else
            {
                Alarm alarm = Alarm.New($"{nameof(ResourcesService)}.NotProvided", "Resources service doesn't exist", sourceCode: string.Empty);
                service.Add(alarm);

                alarm.Fire("Resources service must be provided first");
            }
        }

        /// <summary>
        /// Add a new <see cref="Alarm"/> for an <see cref="IDigitalChannel"/> that will fire based on <paramref name="compare"/> and <paramref name="alarmValue"/>
        /// </summary>
        /// <remarks>
        /// The <see cref="Alarm"/> message will be "<c><paramref name="channel"/>.Description (<paramref name="channel"/>.Symbolic) is 'ON/OFF'</c>"
        /// </remarks>
        /// <typeparam name="T">The type of the <paramref name="channel"/> and <paramref name="alarmValue"/></typeparam>
        /// <param name="channel">The <see cref="IChannel{T}"/> to monitor</param>
        /// <param name="compare">The <see cref="ChannelCompareType"/></param>
        /// <param name="alarmValue">The target value that will fire the <see cref="Alarm"/></param>
        public static void AddAlarm(IDigitalChannel channel, ChannelCompareType compare, bool alarmValue)
        {
            ICondition condition = compare == ChannelCompareType.IsEqual ? channel.IsEqualTo(alarmValue) : channel.IsNotEqualTo(alarmValue);
            string message = $"{channel.Description} ({channel.Symbolic}) is '{(channel.Value ? "ON" : "OFF")}'";

            Alarm alarm = Alarm.New($"{channel.Code}.{compare}", message, condition, string.Empty);
            service.Add(alarm);
        }

        /// <summary>
        /// Add a new <see cref="Alarm"/> and <see cref="Warn"/> for an <see cref="IAnalogChannel"/> that will fire based on 
        /// <paramref name="minValue"/> and <paramref name="maxValue"/> for the <see cref="Alarm"/> and <paramref name="preMinValue"/> and <paramref name="preMaxValue"/>
        /// for the <see cref="Warn"/>. See also <see cref="AddAlarm(IAnalogChannel, double, double, double, double, double)"/>
        /// </summary>
        /// <remarks>
        /// The <see cref="Alarm"/> and <see cref="Warn"/> messages will be 
        /// "<c><paramref name="channel"/>.Description (<paramref name="channel"/>.Symbolic) value is <paramref name="channel"/>.Value, outside of (lowLim, highLim)</c>"
        /// </remarks>
        /// <typeparam name="T">The type of the <paramref name="channel"/> and <paramref name="alarmValue"/></typeparam>
        /// <param name="channel">The <see cref="IChannel{T}"/> to monitor</param>
        /// <param name="minValue">The min value for the <see cref="Alarm"/></param>
        /// <param name="maxValue">The max value for the <see cref="Alarm"/></param>
        /// <param name="preMinValue">The pre min value for the <see cref="Warn"/></param>
        /// <param name="preMaxValue">The pre max value for the <see cref="Warn"/></param>
        /// <param name="stabilizationTime">The stabilization time of the firing conditions</param>
        public static void AddAlarm(IAnalogChannel channel, double maxValue, double minValue, double preMaxValue, double preMinValue, TimeSpan stabilizationTime)
        {
            ICondition alarmCondition = channel.IsNotInRange(minValue, maxValue).IsStableFor(stabilizationTime);
            string alarmMessage = $"{channel.Description} ({channel.Symbolic}) value is {channel}, outside of ({minValue}, {maxValue})";

            ICondition warnCondition = channel.IsNotInRange(preMinValue, preMaxValue).IsStableFor(stabilizationTime);
            string warnMessage = $"{channel.Description} ({channel.Symbolic}) value is {channel}, outside of ({preMinValue}, {preMaxValue})";

            Alarm alarm = Alarm.New($"{channel.Code}.IsInRange.Alarm", alarmMessage, alarmCondition, string.Empty);
            service.Add(alarm);

            Warn warn = Warn.New($"{channel.Code}.IsInRange.Warn", warnMessage, warnCondition, string.Empty);
            service.Add(warn);
        }

        /// <summary>
        /// Add a new <see cref="Alarm"/> and <see cref="Warn"/> for an <see cref="IAnalogChannel"/> that will fire based on 
        /// <paramref name="minValue"/> and <paramref name="maxValue"/> for the <see cref="Alarm"/> and <paramref name="preMinValue"/> and <paramref name="preMaxValue"/>
        /// for the <see cref="Warn"/>. See also <see cref="AddAlarm(IAnalogChannel, double, double, double, double, TimeSpan)"/>
        /// </summary>
        /// <remarks>
        /// The <see cref="Alarm"/> and <see cref="Warn"/> messages will be 
        /// "<c><paramref name="channel"/>.Description (<paramref name="channel"/>.Symbolic) value is <paramref name="channel"/>.Value, outside of (lowLim, highLim)</c>"
        /// </remarks>
        /// <typeparam name="T">The type of the <paramref name="channel"/> and <paramref name="alarmValue"/></typeparam>
        /// <param name="channel">The <see cref="IChannel{T}"/> to monitor</param>
        /// <param name="minValue">The min value for the <see cref="Alarm"/></param>
        /// <param name="maxValue">The max value for the <see cref="Alarm"/></param>
        /// <param name="preMinValue">The pre min value for the <see cref="Warn"/></param>
        /// <param name="preMaxValue">The pre max value for the <see cref="Warn"/></param>
        /// <param name="stabilizationTimeInMilliseconds">The stabilization time in milliseconds of the firing conditions</param>
        public static void AddAlarm(IAnalogChannel channel, double maxValue, double minValue, double preMaxValue, double preMinValue, double stabilizationTimeInMilliseconds)
            => AddAlarm(channel, maxValue, minValue, preMaxValue, preMinValue, TimeSpan.FromMilliseconds(stabilizationTimeInMilliseconds));
    }
}

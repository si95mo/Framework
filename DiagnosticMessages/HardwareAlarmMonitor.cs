using Core.Conditions;
using Core.DataStructures;
using Hardware;
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
            service = ServiceBroker.CanProvide<DiagnosticMessagesService>() ? ServiceBroker.GetService<DiagnosticMessagesService>() : new DiagnosticMessagesService();
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
        /// Add a new <see cref="Alarm"/> for a <see cref="IChannel{T}"/> that will fire based on <paramref name="compare"/> and <paramref name="alarmValue"/>
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="channel"/> and <paramref name="alarmValue"/></typeparam>
        /// <param name="channel">The <see cref="IChannel{T}"/> to monitor</param>
        /// <param name="compare">The <see cref="ChannelCompareType"/></param>
        /// <param name="alarmValue">The target value that will fire the <see cref="Alarm"/></param>
        public static void AddAlarm<T>(IChannel<T> channel, ChannelCompareType compare, T alarmValue)
        {
            ICondition condition = compare == ChannelCompareType.IsEqual ? channel.IsEqualTo(alarmValue) : channel.IsNotEqualTo(alarmValue);
            string message = $"Channel {channel.Code} is {(compare == ChannelCompareType.IsEqual ? "equal" : "not equal")} to {alarmValue}";

            Alarm alarm = Alarm.New($"{channel.Code}.{compare}", message, condition, string.Empty);
            service.Add(alarm);
        }
    }
}

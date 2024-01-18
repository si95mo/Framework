using Core.DataStructures;
using Diagnostic;
using Diagnostic.Messages;
using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasks
{
    /// <summary>
    /// Define a class that will monitor all the <see cref="Alarm"/> contained in the <see cref="DiagnosticMessagesService"/> and if one fires then will
    /// stop all the <see cref="IAwaitable"/> tasks in the <see cref="TasksService"/> not marked with the <see cref="DontStopInAlarm"/> <see cref="Attribute"/>
    /// </summary>
    public static class AlarmMonitor
    {
        private static DiagnosticMessagesService alarmsService;
        private static TasksService tasksService;

        /// <summary>
        /// Initialize the <see cref="AlarmMonitor"/>
        /// </summary>
        public static void Initialize()
        {
            // Retrieve the services needed if possible, otherwise use new services but log an error, the latter is not the way this class is intended to work
            // 1) Retrieve the diagnostic messages service
            if (ServiceBroker.CanProvide<DiagnosticMessagesService>())
            {
                alarmsService = ServiceBroker.GetService<DiagnosticMessagesService>();
            }
            else
            {
                alarmsService = new DiagnosticMessagesService();
                Logger.Error($"{nameof(DiagnosticMessagesService)} not provided by the {nameof(ServiceBroker)}. {nameof(AlarmMonitor)} will use its own");
            }

            // 2) Retrieve the tasks service
            if (ServiceBroker.CanProvide<TasksService>())
            {
                tasksService = ServiceBroker.GetService<TasksService>();
            }
            else
            {
                tasksService = new TasksService();
                Logger.Error($"{nameof(TasksService)} not provided by the {nameof(ServiceBroker)}. {nameof(AlarmMonitor)} will use its own");
            }

            // The bind all the possible alarms already present in the service
            foreach (IDiagnosticMessage message in alarmsService.GetAll().Cast<IDiagnosticMessage>())
            {
                Bind(message);
            }

            // And prepare to bind the ones that will be added later
            alarmsService.Subscribers.Added += DiagnosticMessage_Added;
        }

        private static void DiagnosticMessage_Added(object sender, BagChangedEventArgs<Core.IProperty> e)
        {
            Bind(e.Item as IDiagnosticMessage);
        }

        /// <summary>
        /// Bind an <see cref="Alarm"/> to the <see cref="Alarm_FiredAsync(object, FiredEventArgs)"/> event handler
        /// </summary>
        /// <param name="message">The <see cref="IDiagnosticMessage"/> to bind (only if it's an <see cref="Alarm"/>)</param>
        private static void Bind(IDiagnosticMessage message)
        {
            if (message != null && message is Alarm alarm)
            {
                alarm.Fired += Alarm_FiredAsync;
            }
        }

        private static async void Alarm_FiredAsync(object sender, FiredEventArgs e)
        {
            // Get all the tasks that must be stopped (the ones not marked with the DontStopInAlarm attribute)
            IEnumerable<IAwaitable> tasksToStop = tasksService
                .GetAll()
                .OfType<IAwaitable>()
                .Where((x) => !Attribute.IsDefined(x.GetType(), typeof(DontStopInAlarm)));

            // Then try to stop all the tasks in parallel, if possible
            await tasksToStop.ParallelForeachAsync(async (x) => await Task.Run(() => x.Fail($"Alarm '{e.DiagnosticMessage.Code}' fired")));
        }
    }
}
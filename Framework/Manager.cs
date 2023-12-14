﻿using Configurations;
using Core.Conditions;
using Core.DataStructures;
using Core.Scripting;
using Diagnostic;
using Diagnostic.Messages;
using Hardware;
using System.Threading.Tasks;
using Tasks;
using UserInterface.Forms;

namespace Framework
{
    /// <summary>
    /// Provide basic framework features
    /// </summary>
    public static class Manager
    {
        /// <summary>
        /// Initialize the framework
        /// </summary>
        /// <remarks>The order is <see cref="Logger"/> → <see cref="ServiceBroker"/> → all <see cref="Service{T}"/>, except the <see cref="UiService"/></remarks>
        /// <param name="logsPath">The log files path</param>
        /// <param name="daysOfLogsToKeepSaved">The number of days of logs to keep saved (-1 means keep everything)</param>
        /// <param name="logExternalExceptions">The option to log external exceptions</param>
        /// <param name="scriptsPath">The scripts path (<see langword="null"/> to skip)</param>
        /// <param name="maxDegreesOfParallelism">The maximum number of tasks that the scheduler car run in parallel</param>
        /// <param name="configPath">The configuration file path</param>
        /// <returns>The (async) <see cref="Task{TResult}"/> with the read <see cref="Configuration"/></returns>
        public static async Task<Configuration> InitializeAsync(string logsPath = "logs\\", int daysOfLogsToKeepSaved = -1, bool logExternalExceptions = false, 
            string scriptsPath = null, int maxDegreesOfParallelism = 100, string configPath = "config\\config.json")
        {
            await InitializeFrameworkAsync(logsPath, daysOfLogsToKeepSaved, logExternalExceptions);
            await InitializeServicesAsync(scriptsPath, maxDegreesOfParallelism);
            Configuration configuration = await ReadConfigurationAsync(configPath);

            return configuration;
        }

        /// <summary>
        /// Initialize the framework and the <see cref="UiService"/>
        /// </summary>
        /// <remarks>
        /// The order is <see cref="Logger"/> → <see cref="ServiceBroker"/> → all <see cref="Service{T}"/>, <see cref="UiService"/> included (as the last).<br/>
        /// This method requires that the main <see cref="CustomForm"/> is already created, so up to the creation part no framework utilities will be available!
        /// See <see cref="InitializeAsync(string, int, bool, string, int, string)"/> and then <see cref="InitializeUiAsync(CustomForm)"/> for alternatives
        /// </remarks>
        /// <param name="form">The main <see cref="CustomForm"/></param>
        /// <param name="logsPath">The log files path</param>
        /// <param name="daysOfLogsToKeepSaved">The number of days of logs to keep saved (-1 means keep everything)</param>
        /// <param name="logExternalExceptions">The option to log external exceptions</param>
        /// <param name="scriptsPath">The scripts path (<see langword="null"/> to skip)</param>
        /// <param name="maxDegreesOfParallelism">The maximum number of tasks that the scheduler car run in parallel</param>
        /// <param name="configPath">The configuration file path</param>
        /// <returns>The (async) <see cref="Task{TResult}"/> with the read <see cref="Configuration"/></returns>
        public static async Task<Configuration> InitializeAsync(CustomForm form, string logsPath = "logs\\", int daysOfLogsToKeepSaved = -1, bool logExternalExceptions = false,
            string scriptsPath = null, int maxDegreesOfParallelism = 100, string configPath = "config\\config.json")
        {
            await InitializeFrameworkAsync(logsPath, daysOfLogsToKeepSaved, logExternalExceptions);
            await InitializeServicesAsync(scriptsPath, maxDegreesOfParallelism);
            Configuration configuration = await ReadConfigurationAsync(configPath);
            await InitializeUiAsync(form);

            return configuration;
        }

        /// <summary>
        /// Initialize the <see cref="UiService"/>
        /// </summary>
        /// <param name="form">The main <see cref="CustomForm"/></param>
        /// <returns>The (async) <see cref="Task"/></returns>
        public static async Task InitializeUiAsync(CustomForm form)
        {
            UiService ui = new UiService(form);
            ServiceBroker.Provide(ui);
            await Logger.InfoAsync($"{nameof(UiService)} created");
        }

        #region Helper methods

        /// <summary>
        /// Initialize the basic framework functionalities
        /// </summary>
        /// <param name="logsPath">The log files path</param>
        /// <param name="daysOfLogsToKeepSaved">The number of days of logs to keep saved (-1 means keep everything)</param>
        /// <param name="logExternalExceptions">The option to log external exceptions</param>
        /// <returns>The (async) <see cref="Task"/></returns>
        private static async Task InitializeFrameworkAsync(string logsPath = "logs\\", int daysOfLogsToKeepSaved = -1, bool logExternalExceptions = false)
        {
            Logger.Initialize(logsPath, daysOfLogsToKeepSaved, logExternalExceptions);
            await Logger.InfoAsync($"{nameof(Logger)} initialized");

            ServiceBroker.Initialize();
            await Logger.InfoAsync($"{nameof(ServiceBroker)} initialized");
        }

        /// <summary>
        /// Initialize all the needed services
        /// </summary>
        /// <param name="scriptsPath">The scripts path (<see langword="null"/> to skip)</param>
        /// <param name="maxDegreesOfParallelism">The maximum number of tasks that the scheduler car run in parallel</param>
        /// <returns>The (async) <see cref="Task"/></returns>
        private static async Task InitializeServicesAsync(string scriptsPath = null, int maxDegreesOfParallelism = 100)
        {
            ConfigurationsService configurations = new ConfigurationsService();
            ServiceBroker.Provide(configurations);
            await Logger.InfoAsync($"{nameof(ConfigurationsService)} created");

            ResourcesService resources = new ResourcesService();
            ServiceBroker.Provide(resources);
            await Logger.InfoAsync($"{nameof(ResourcesService)} created");

            ChannelsService channels = new ChannelsService();
            ServiceBroker.Provide(channels);
            await Logger.InfoAsync($"{nameof(ChannelsService)} created");

            ConditionsService conditions = new ConditionsService();
            ServiceBroker.Provide(conditions);
            await Logger.InfoAsync($"{nameof(ConditionsService)} created");

            DiagnosticMessagesService messages = new DiagnosticMessagesService();
            ServiceBroker.Provide(messages);
            await Logger.InfoAsync($"{nameof(DiagnosticMessagesService)} created");

            TasksService tasks = new TasksService();
            ServiceBroker.Provide(tasks);
            await Logger.InfoAsync($"{nameof(TasksService)} created");

            SchedulersService schedulers = new SchedulersService(maxDegreesOfParallelism);
            ServiceBroker.Provide(schedulers);
            await Logger.InfoAsync($"{nameof(SchedulersService)} created");

            ScriptsService scripts = new ScriptsService(scriptsPath);
            ServiceBroker.Provide(scripts);
            await Logger.InfoAsync($"{nameof(ScriptsService)} created");
        }

        /// <summary>
        /// Read the configuration file
        /// </summary>
        /// <param name="configPath">The configuration file path</param>
        /// <returns>The (async) <see cref="Task{TResult}"/> with the read <see cref="Configuration"/></returns>
        private static async Task<Configuration> ReadConfigurationAsync(string configPath = "config\\config.json")
        {
            Configuration configuration = new Configuration(path: configPath);
            ServiceBroker.GetService<ConfigurationsService>().Add(configuration);

            await Logger.InfoAsync("Configuration file processed");

            return configuration;
        }

        #endregion Helper methods
    }
}

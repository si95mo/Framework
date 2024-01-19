using Core.DataStructures;
using Core.Parameters;
using Diagnostic;
using Extensions;
using IO;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Tasks.Configurations
{
    public class ConfigurationsService : Service<IConfigurationTask>
    {
        public ConfigurationsService() : this(Guid.NewGuid().ToString())
        { }

        public ConfigurationsService(string code) : base(code)
        { }

        /// <summary>
        /// Configure all the <see cref="IConfigurationTask"/> subscribed to the <see cref="ConfigurationsService"/> asynchronously
        /// </summary>
        /// <returns>The (async) <see cref="Task"/></returns>
        public async Task ConfigureAllAsync()
        {
            await Logger.InfoAsync("Async configuration started");

            int counter = 0;
            int n = Subscribers.Count;
            await Subscribers.Values.ParallelForeachAsync(async (x) => 
                { 
                    await Logger.DebugAsync($"Starting configuration task {x.Code} ({++counter} / {n})");
                    await x.Start(); 
                }
            );

            await Logger.InfoAsync("Async configuration done");
        }

        /// <summary>
        /// Configure all the <see cref="IConfigurationTask"/> subscribed to the <see cref="ConfigurationsService"/>
        /// </summary>
        public void ConfigureAll()
        {
            Logger.Info("Configuration started");

            int counter = 0;
            int n = Subscribers.Count;
            foreach (IConfigurationTask task in Subscribers.Values)
            {
                Logger.Debug($"Starting configuration task {task.Code} ({++counter} / {n})");
                Task.Run(async () => await task.Start());
            }

            Logger.Info("Configuration done");
        }

        /// <summary>
        /// Connect the jsons to the <see cref="IConfigurationTask.Settings"/>
        /// </summary>
        /// <param name="task">The <see cref="IConfigurationTask"/> to connect</param>
        internal void ConnectFromSystemParameters(IConfigurationTask task)
        {
            if (Directory.Exists(Paths.Parameters))
            {
                string path = Path.Combine(Paths.Parameters, task.Code);
                if (Directory.Exists(path))
                {
                    foreach (IParameter parameter in task.Settings)
                    {
                        path = Path.Combine(path, $"{parameter.Code}.json");

                        string json = File.ReadAllText(path);
                        IParameter copy = (IParameter)JsonConvert.DeserializeObject(json, parameter.GetType());
                        task.Settings.Get(parameter.Code).ValueAsObject = copy.ValueAsObject;
                    }
                }
                else
                {
                    Logger.Info($"Configuration files for task with code \"{task.Code}\" not found @ \"{path}\". This may be the first execution of the code with the task");
                }
            }
        }

        public override void Dispose()
        {
            if (!Directory.Exists(Paths.Parameters))
            {
                Directory.CreateDirectory(Paths.Parameters);
            }

            foreach (IConfigurationTask task in Subscribers)
            {
                string path = Path.Combine(Paths.Parameters, task.Code);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                foreach (IParameter parameter in task.Settings)
                {
                    string json = JsonConvert.SerializeObject(parameter, Formatting.Indented);

                    path = Path.Combine(path, $"{parameter.Code}.json");
                    File.WriteAllText(path, json);
                }
            }

            base.Dispose();
        }
    }
}

using Core.DataStructures;
using Diagnostic;
using IO;
using Newtonsoft.Json;
using Security.Users;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Security
{
    #region Event args

    /// <summary>
    /// The <see cref="EventArgs"/> for log in/log out events
    /// </summary>
    public class LogEventArgs : EventArgs
    {
        /// <summary>
        /// The <see cref="IUser"/> involved
        /// </summary>
        public IUser User { get; private set; }

        /// <summary>
        /// Create a new instance of <see cref="LogEventArgs"
        /// </summary>
        /// <param name="user">The <see cref="IUser"/> involved</param>
        public LogEventArgs(IUser user)
        {
            User = user;
        }
    }

    #endregion Event args

    /// <summary>
    /// Define a <see cref="Service{T}"/> for <see cref="IUser"/>
    /// </summary>
    public class SecuritiesService : Service<IUser>
    {
        /// <summary>
        /// The actual <see cref="IUser"/> logged on
        /// </summary>
        /// <remarks><see langword="null"/> if no user is actually logged in</remarks>
        public static IUser ActualUser;

        /// <summary>
        /// The log in <see cref="EventHandler{TEventArgs}"/>
        /// </summary>
        public static event EventHandler<LogEventArgs> LoggedIn;

        /// <summary>
        /// The log out <see cref="EventHandler{TEventArgs}"/>
        /// </summary>
        public static event EventHandler<LogEventArgs> LoggedOut;

        private JsonSerializerSettings settings;

        /// <summary>
        /// Create a new instance of <see cref="SecuritiesService"/>
        /// </summary>
        public SecuritiesService() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Create a new instance of <see cref="SecuritiesService"/>
        /// </summary>
        /// <param name="code">The code</param>
        public SecuritiesService(string code) : base(code)
        {
            settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore
            };

            if (!Directory.Exists(Paths.Users))
            {
                Directory.CreateDirectory(Paths.Users);
            }

            string[] directories = Directory.GetDirectories(Paths.Users);
            foreach (string directory in directories)
            {
                string path = Path.Combine(directory, "User.json");
                if(File.Exists(path))
                {
                    string json = File.ReadAllText(path);

                    IUser user = JsonConvert.DeserializeObject<User>(json, settings);
                    Add(user);
                }
            }

            IUser userToLogin = Subscribers.Values.Where((x) => x.Name == "admin").FirstOrDefault();
            if (userToLogin != default)
            {
                Logger.Info("Logging in with default user");
                LogIn("admin", "admin");
            }
            else
            {
                Logger.Warn("Default user \"admin\" not found, adding it");

                userToLogin = new User("admin", "admin");
                Add(userToLogin);

                LogIn("admin", "admin");
            }
        }

        #region Log in/out

        /// <summary>
        /// Log in a new <see cref="IUser"/>, and do a <see cref="LogOut"/> before, if needed
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="password">The password</param>
        /// <returns><see langword="true"/> if the login succeeded, <see langword="false"/> otherwise</returns>
        public bool LogIn(string name, string password)
        {
            bool loggedIn = false;

            if (ActualUser != null)
            {
                LogOut();
            }

            if (Subscribers.ContainsKey(name))
            {
                IUser user = Subscribers.Get(name);
                if (user.Match(password))
                {
                    ActualUser = user;

                    loggedIn = true;

                    LoggedIn?.Invoke(this, new LogEventArgs(ActualUser));
                    Logger.Info($"User \"{name}\" logged in");
                }
                else
                {
                    Logger.Info($"Attempted to login failed. Incorrect password for user \"{name}\"");
                }
            }
            else
            {
                Logger.Info($"Attempted to login failed. User with name \"{name}\" does not exists");
            }

            return loggedIn;
        }

        /// <summary>
        /// Log out the actual <see cref="IUser"/>
        /// </summary>
        public void LogOut()
        {
            if (ActualUser != null)
            {
                IUser user = ActualUser;
                ActualUser = null;

                LoggedOut?.Invoke(this, new LogEventArgs(user));
                Logger.Info($"User \"{user.Name}\" logged out");
            }
        }

        #endregion Log in/out

        public override void Dispose()
        {
            // Delete all the directories that do not point to a serialized user anymore (i.e. deleted)
            IEnumerable<string> directories = Directory
                .GetDirectories(Paths.Users)
                .Except(Subscribers.Values.OfType<IUser>().Select((x) => Path.Combine(Paths.Users, x.Name)));
            foreach (string directory in directories)
            {
                Directory.Delete(directory, true);
                Logger.Debug($"Directory @ \"{directory}\" for user \"{Path.GetDirectoryName(directory)}\" deleted");
            }

            foreach (IUser user in Subscribers.Values)
            {
                if (!Directory.Exists(Paths.Users))
                {
                    Directory.CreateDirectory(Paths.Users);
                }

                string json = JsonConvert.SerializeObject(user, Formatting.Indented, settings);
                string path = Path.Combine(Paths.Users, user.Name);

                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                path = Path.Combine(path, "User.json");
                File.WriteAllText(path, json);
            }

            base.Dispose();
        }
    }
}

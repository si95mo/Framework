using Core.DataStructures;
using Diagnostic;
using Security.Users;
using System;

namespace Security
{
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

        /// <summary>
        /// Create a new instance of <see cref="SecuritiesService"/>
        /// </summary>
        public SecuritiesService() : base()
        { }

        /// <summary>
        /// Create a new instance of <see cref="SecuritiesService"/>
        /// </summary>
        /// <param name="code">The code</param>
        public SecuritiesService(string code) : base(code)
        { }

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
    }
}

using Core;
using Security.Passwords;

namespace Security.Users
{
    /// <summary>
    /// Define a basic prototype for users
    /// </summary>
    public interface IUser : IProperty
    {
        /// <summary>
        /// The user name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The <see cref="Passwords.Password"/>
        /// </summary>
        IPassword Password { get; }

        /// <summary>
        /// Define if the user <see cref="Password"/> matches with the one provided
        /// </summary>
        /// <param name="password">The password to check</param>
        /// <returns><see langword="true"/> if <paramref name="password"/> matches, <see langword="false"/> otherwise</returns>
        bool Match(string password);
    }
}

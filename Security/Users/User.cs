using Newtonsoft.Json;
using Security.Passwords;
using System;

namespace Security.Users
{
    /// <summary>
    /// Define an <see cref="IUser"/>
    /// </summary>
    public class User : IUser
    {
        #region IUser implementation

        public string Name { get; protected set; }

        public IPassword Password { get; protected set; }

        #endregion IUser implementation

        #region IProperty implementation

        [JsonIgnore]
        public string Code => Name;

        [JsonIgnore]
        public object ValueAsObject { get => Name; set => _ = value; }

        [JsonIgnore]
        public Type Type => GetType();

        #endregion IProperty implementation

        /// <summary>
        /// Create a new instance of <see cref="User"/> with default <see cref="Name"/> and <see cref="Password"/>
        /// </summary>
        public User() : this("admin", "admin")
        { }

        /// <summary>
        /// Create a new instance of <see cref="User"
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="password">The password in clear</param>
        public User(string name, string password) 
        {
            Name = name;

            DefaultEncryptor encryptor = new DefaultEncryptor();
            string encryptedPassword = encryptor.Encrypt(password);
            Password = new Password(encryptedPassword);
        }

        /// <summary>
        /// Create a new instance of <see cref="User"/>
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="password">The <see cref="IPassword"/></param>
        public User(string name, IPassword password)
        {
            Name = name;
            Password = password;
        }

        public bool Match(string password)
            => Password.Match(password);
    }
}

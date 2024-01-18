using Newtonsoft.Json;

namespace Security.Passwords
{
    /// <summary>
    /// Define a password
    /// </summary>
    public class Password : IPassword
    {
        [JsonIgnore]
        public IEncryptor Encryptor { get; private set; }

        /// <summary>
        /// The encrypted password
        /// </summary>
        public string EncryptedPassword { get; private set; }

        /// <summary>
        /// Create a new instance of <see cref="Password"/>
        /// </summary>
        /// <param name="encryptedPassword">The encrypted password</param>
        /// <param name="encryptor">The <see cref="IEncryptor"/></param>
        public Password(string encryptedPassword, IEncryptor encryptor)
        {
            Encryptor = encryptor;
            EncryptedPassword = encryptedPassword;
        }

        /// <summary>
        /// Create a new instance of <see cref="Password"/>, with <see cref="DefaultEncryptor"/> as the crypto algorithm
        /// </summary>
        /// <param name="encryptedPassword">The <see cref="EncryptedPassword"/></param>
        public Password(string encryptedPassword) : this(encryptedPassword, new DefaultEncryptor())
        { }

        public virtual void Encrypt(string password)
        {
            if(Encryptor == null)
            {
                Encryptor = new DefaultEncryptor();
            }

            EncryptedPassword = Encryptor.Encrypt(password);
        }

        public bool Match(string password)
        {
            if (Encryptor == null)
            {
                Encryptor = new DefaultEncryptor();
            }

            string encryptedPassword = Encryptor.Encrypt(password);
            bool match = EncryptedPassword == encryptedPassword;

            return match;
        }
    }
}

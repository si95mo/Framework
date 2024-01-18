namespace Security.Passwords
{
    /// <summary>
    /// Define a password
    /// </summary>
    public class Password : IPassword
    {
        /// <summary>
        /// The <see cref="Password"/> <see cref="IEncryptor"/>
        /// </summary>
        protected IEncryptor Encryptor;

        /// <summary>
        /// The encrypted password
        /// </summary>
        protected string EncryptedPassword {  get; set; }

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
            EncryptedPassword = Encryptor.Encrypt(password);
        }

        public bool Match(string password)
        {
            string encryptedPassword = Encryptor.Encrypt(password);
            bool match = EncryptedPassword == encryptedPassword;

            return match;
        }
    }
}

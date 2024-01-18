using System.Text;

namespace Security.Passwords
{
    /// <summary>
    /// Define a default <see cref="IEncryptor"/> with SHA256 crypto algorithm
    /// </summary>
    public class DefaultEncryptor : IEncryptor
    {
        /// <summary>
        /// The <see cref="System.Text.Encoding"/> to use
        /// </summary>
        public Encoding Encoding { get; internal set; }

        /// <summary>
        /// Create a new instance of <see cref="DefaultEncryptor"/>
        /// </summary>
        /// <param name="encoding">The <see cref="System.Text.Encoding"/> to use</param>
        public DefaultEncryptor(Encoding encoding)
        {
            Encoding = encoding;
        }

        /// <summary>
        /// Create a new instance of <see cref="DefaultEncryptor"/> with <see cref="Encoding.UTF8"/> as the encoding
        /// </summary>
        public DefaultEncryptor() : this(Encoding.UTF8)
        { }

        public string Encrypt(string text)
        {
            byte[] data = Encoding.GetBytes(text);

            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            string hash = Encoding.GetString(data);

            return hash;
        }
    }
}

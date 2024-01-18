namespace Security.Passwords
{
    /// <summary>
    /// Define a basic encryptor prototype
    /// </summary>
    public interface IEncryptor
    {
        /// <summary>
        /// Encrypt a <see cref="string"/>
        /// </summary>
        /// <param name="text">The <see cref="string"/> to encrypt</param>
        /// <returns>The encrypted <see cref="string"/></returns>
        string Encrypt(string text);
    }
}

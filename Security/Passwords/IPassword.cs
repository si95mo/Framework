namespace Security.Passwords
{
    /// <summary>
    /// Define a basic interface for passwords
    /// </summary>
    public interface IPassword
    {
        /// <summary>
        /// Encrypt a clear <paramref name="password"/>
        /// </summary>
        /// <param name="password">The password to encrypt</param>
        /// <returns>The encrypted password</returns>
        void Encrypt(string password);

        /// <summary>
        /// Define if the <see cref="IPassword"/> matches with the one provided
        /// </summary>
        /// <param name="password">The clear password to check</param>
        /// <returns><see langword="true"/> if <paramref name="password"/> matches, <see langword="false"/> otherwise</returns>
        bool Match(string password);
    }
}

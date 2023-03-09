namespace Hardware
{
    /// <summary>
    /// Provide hardware-related extension methods
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Set the <see cref="IChannel.Description"/>
        /// </summary>
        /// <param name="source">The source <see cref="IChannel"/></param>
        /// <param name="description">The description</param>
        public static IChannel WithDescription(this IChannel source, string description)
        {
            source.Description = description;
            return source;
        }
    }
}
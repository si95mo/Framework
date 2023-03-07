using Core.DataStructures;

namespace Hardware
{
    /// <summary>
    /// Define a <see cref="Service{T}"/> for <see cref="IResource"/>
    /// </summary>
    public class ResourcesService : Service<IResource>
    {
        /// <summary>
        /// Create a new instance of <see cref="ResourcesService"/>
        /// </summary>
        public ResourcesService() : base()
        { }

        /// <summary>
        /// Create a new instance of <see cref="ResourcesService"/>
        /// </summary>
        /// <param name="code">The code</param>
        public ResourcesService(string code) : base(code)
        { }
    }
}

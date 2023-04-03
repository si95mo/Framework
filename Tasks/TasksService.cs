using Core.DataStructures;

namespace Tasks
{
    /// <summary>
    /// Define a <see cref="Service{T}"/> for <see cref="IAwaitable"/> tasks
    /// </summary>
    public class TasksService : Service<IAwaitable>
    {
        /// <summary>
        /// Create a new instance of <see cref="TasksService"/>
        /// </summary>
        public TasksService() : base()
        { }

        /// <summary>
        /// Create a new instance of <see cref="TasksService"/>
        /// </summary>
        /// <param name="code">The code</param>
        public TasksService(string code) : base(code)
        { }
    }
}
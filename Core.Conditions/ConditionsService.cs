using Core.DataStructures;

namespace Core.Conditions
{
    /// <summary>
    /// Define a <see cref="Service{T}"/> for <see cref="ICondition"/>
    /// </summary>
    public class ConditionsService : Service<ICondition>
    {
        /// <summary>
        /// Create a new instance of <see cref="ConditionsService"/>
        /// </summary>
        public ConditionsService() : base()
        { }

        /// <summary>
        /// Create a new instance of <see cref="ConditionsService"/>
        /// </summary>
        /// <param name="code">The code</param>
        public ConditionsService(string code) : base(code)
        { }
    }
}

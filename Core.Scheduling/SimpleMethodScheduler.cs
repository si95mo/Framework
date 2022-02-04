using Core.DataStructures;
using System;

namespace Core.Scheduling
{
    /// <summary>
    /// Class that implement a simple scheduler that executes the methods
    /// of the subscribed <see cref="object"/>.
    ///  <b>Note</b> that each class that will have methods subscribed to the <see cref="SimpleMethodScheduler"/>
    /// <b>must</b> be serializable in order to perform a deep copy of the objects and
    /// have the <see cref="SimpleMethodScheduler"/> work!
    /// See <see cref="Extensions.SystemExtension.DeepCopy{T}(T)"/>, <see cref="SerializableAttribute"/> and
    /// also <see cref="SerializableAttribute.SerializableAttribute"/>
    /// </summary>
    [Serializable]
    public class SimpleMethodScheduler : MethodScheduler
    {
        /// <summary>
        /// Creates a new instance of the <see cref="SimpleMethodScheduler"/>
        /// </summary>
        public SimpleMethodScheduler() : base()
        { }

        /// <summary>
        /// Executes the <see cref="Action"/> associated with the <see cref="Method"/>
        /// stored in the subscribed ones,
        /// and remove it from the <see cref="ActionQueue{T}"/>
        /// </summary>
        /// <returns>The <see cref="Method"/> executed</returns>
        public override Method Execute()
        {
            Method method = subscribedMethods.Dequeue();
            Action action = method.UnwrapAction();

            action?.Invoke();

            return method;
        }
    }
}
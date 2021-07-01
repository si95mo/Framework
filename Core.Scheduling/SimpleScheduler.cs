using Core.DataStructures;
using System;

namespace Core.Scheduling
{
    /// <summary>
    /// Class that implement a simple scheduler that executes the methods
    /// of the subscribed <see cref="object"/>.
    ///  <b>Note</b> that each class that will have methods subscribed to the <see cref="SimpleScheduler"/>
    /// <b>must</b> be serializable in order to perform a deep copy of the objects and
    /// have the <see cref="SimpleScheduler"/> work!
    /// See <see cref="Core.Extensions.SystemExtension.Clone{T}(T)"/>, <see cref="SerializableAttribute"/> and
    /// also <see cref="SerializableAttribute.SerializableAttribute"/>
    /// </summary>
    [Serializable]
    public class SimpleScheduler : Scheduler
    {
        /// <summary>
        /// Creates a new instance of the <see cref="SimpleScheduler"/>
        /// </summary>
        public SimpleScheduler() : base()
        { }

        /// <summary>
        /// Executes the <see cref="Action"/> associated with the <see cref="Method"/>
        /// stored in the <see cref="SubscribedMethods"/>,
        /// and remove it from the <see cref="MethodQueue{T}"/>
        /// </summary>
        /// <returns>The <see cref="Method"/> executed</returns>
        public override Method ExecuteAction()
        {
            Method method = subscribedMethods.Dequeue();
            Action action = method.UnwrapAction();

            action?.Invoke();

            return method;
        }
    }
}
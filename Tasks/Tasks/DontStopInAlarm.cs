using System;

namespace Tasks
{
    /// <summary>
    /// Define an <see cref="Attribute"/> that will mark an <see cref="IAwaitable"/> do not be stopped in case of <see cref="Diagnostic.Messages.Alarm"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DontStopInAlarm : Attribute
    { }
}
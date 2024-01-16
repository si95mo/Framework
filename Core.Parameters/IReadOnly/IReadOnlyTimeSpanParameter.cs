using System;

namespace Core.Parameters.IReadOnly
{
    /// <summary>
    /// Define an <see cref="IReadOnlyParameter{T}"/> for <see cref="TimeSpan"/>
    /// </summary>
    public interface IReadOnlyTimeSpanParameter : IReadOnlyParameter<TimeSpan>
    { }
}

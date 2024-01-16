using System;

namespace Core.Parameters.IReadOnly
{
    /// <summary>
    /// Define an <see cref="IReadOnlyParameter{T}"/> for <see cref="Enum"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadOnlyEnumParameter<T> : IReadOnlyParameter<T> where T : Enum
    { }
}

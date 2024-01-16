namespace Core.Parameters.IReadOnly
{
    /// <summary>
    /// Define a readonly <see cref="IParameter"/>
    /// </summary>
    public interface IReadOnlyParameter : IReadOnlyProperty, IParameter
    { }

    /// <summary>
    /// Define a readonly <see cref="IParameter"/>
    /// </summary>
    /// <typeparam name="T">The generics type</typeparam>
    public interface IReadOnlyParameter<T> : IReadOnlyProperty<T>, IReadOnlyParameter
    { }
}

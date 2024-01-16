namespace Core
{
    /// <summary>
    /// Define an <see cref="IProperty"/> that's only readonly
    /// </summary>
    public interface IReadOnlyProperty : IProperty
    { }

    /// <summary>
    /// Define an <see cref="IProperty"/> whose value is only readonly
    /// </summary>
    public interface IReadOnlyProperty<T> : IReadOnlyProperty
    {
        T Value { get; }
    }
}

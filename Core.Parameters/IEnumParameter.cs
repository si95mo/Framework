using System;

namespace Core.Parameters
{
    public interface IEnumParameter
    {
    }

    /// <summary>
    /// Describe a generic enum parameter
    /// </summary>
    /// <typeparam name="T">The <see cref="Enum"/> type</typeparam>
    public interface IEnumParameter<T> : IParameter<T>
    {
    }
}
using Newtonsoft.Json;

namespace Core.Parameters
{
    /// <summary>
    /// Describe a generic numeric parameter
    /// </summary>
    public interface INumericParameter : IParameter<double>
    {
        /// <summary>
        /// The <see cref="IParameter{T}.Value"/> as <see cref="int"/>
        /// </summary>
        [JsonIgnore]
        int ValueAsInt { get; }

        /// <summary>
        /// The <see cref="IParameter{T}.Value"/> as <see cref="float"/>
        /// </summary>
        [JsonIgnore]
        float ValueAsFloat { get; }

        /// <summary>
        /// The <see cref="IParameter{T}.Value"/> as <see cref="byte"/>
        /// </summary>
        [JsonIgnore]
        byte ValueAsByte { get; }

        /// <summary>
        /// The <see cref="IParameter{T}.Value"/> as a <see cref="byte"/> array
        /// </summary>
        [JsonIgnore]
        byte[] ValueAsByteArray { get; }
    }
}
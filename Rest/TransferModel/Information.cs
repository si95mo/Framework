using Newtonsoft.Json;

namespace Rest.TransferModel
{
    /// <summary>
    /// Define a generic information response for REST modules
    /// </summary>
    /// <remarks>The only thing that this class defines is helper methods for serialization, in which the return value will be the serialized object</remarks>
    public abstract class Information
    {
        /// <summary>
        /// Serialize the instance
        /// </summary>
        /// <param name="formatting">The <see cref="Formatting"/> to use</param>
        /// <returns>The serialization <see cref="string"/></returns>
        internal string Serialize(Formatting formatting = Formatting.Indented)
        {
            string serialized = JsonConvert.SerializeObject(this, formatting);
            return serialized;
        }

        /// <summary>
        /// Define a custom <c>ToString()</c> in which the return value will be the serialization <see cref="string"/> of the instance
        /// </summary>
        /// <remarks>See <see cref="Serialize(Formatting)"/>; this method will use <see cref="Formatting.Indented"/></remarks>
        /// <returns>The serialized instance</returns>
        public string SerializedToString()
        {
            string description = Serialize();
            return description;
        }
    }
}

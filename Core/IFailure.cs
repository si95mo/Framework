using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// Interface that describe a generic failure.
    /// </summary>
    public interface IFailure
    {
        /// <summary>
        /// The <see cref="IFailure"/> description
        /// </summary>
        string Description
        { get; set; }

        /// <summary>
        /// The <see cref="IFailure"/> timestamp;
        /// </summary>
        DateTime Timestamp
        { get; set; }

        /// <summary>
        /// Clear the <see cref="IFailure"/>, 
        /// resetting it to default values
        /// </summary>
        void Clear();
    }
}

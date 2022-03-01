﻿namespace Hardware.Twincat
{
    public class TwincatDigitalInput : TwincatChannel<bool>, ITwincatChannel
    {
        /// <summary>
        /// Create a new instance of <see cref="TwincatDigitalInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="variableName">The variable name</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        protected TwincatDigitalInput(string code, string variableName, IResource resource)
            : base(code, variableName, resource, measureUnit: "", format: "0")
        { }
    }
}
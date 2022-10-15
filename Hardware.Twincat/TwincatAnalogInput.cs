using Diagnostic;
using Extensions;
using System;
using TwinCAT.Ads.TypeSystem;
using TwinCAT.TypeSystem;

namespace Hardware.Twincat
{
    /// <summary>
    /// Implement a Twincat analog input
    /// </summary>
    public class TwincatAnalogInput : TwincatChannel<double>
    {
        /// <summary>
        /// Create a new instance of <see cref="TwincatAnalogInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="variableName">The variable name in Twincat</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        public TwincatAnalogInput(string code, string variableName, IResource resource, string measureUnit = "", string format = "0.000")
            : base(code, variableNamein, resource, measureUnit, format)
        {
            if (resource.Status.Value == ResourceStatus.Executing)
                Attach();
        }

        public override void Attach()
        {
            if ((Resource as TwincatResource).TryGetInstance(this, out ISymbol symbol))
            {
                Symbol = symbol as Symbol;
                ManagedType = (Symbol.DataType as DataType).ManagedType;

                if (ManagedType.IsNumeric())
                {
                    if (Symbol?.Connection?.IsConnected == true)
                    {
                        lock (LockObject)
                        {
                            try
                            {
                                Value = Convert.ToDouble(Symbol.ReadAnyValue(ManagedType));
                            }
                            catch (Exception ex)
                            {
                                Logger.Error($"{ex.Message} occurred when reading {Code}");
                            }
                        }
                    }

                    Symbol.ValueChanged += Symbol_ValueChanged;
                }
            }
        }

        private void Symbol_ValueChanged(object sender, ValueChangedArgs e)
        {
            lock (LockObject)
            {
                try
                {
                    Value = Convert.ToDouble(e.Value);
                }
                catch (Exception ex)
                {
                    Logger.Error($"{ex.Message} occurred when reading {Code}");
                }
            }
        }
    }
}
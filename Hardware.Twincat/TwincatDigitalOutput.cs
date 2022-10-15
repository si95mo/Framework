using Diagnostic;
using System;
using TwinCAT.Ads.TypeSystem;
using TwinCAT.TypeSystem;

namespace Hardware.Twincat
{
    /// <summary>
    /// Implement a Twincat digital output
    /// </summary>
    public class TwincatDigitalOutput : TwincatChannel<bool>, ITwincatChannel
    {
        /// <summary>
        /// Create a new instance of <see cref="TwincatDigitalOutput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="variableName">The variable name in Twincat</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        public TwincatDigitalOutput(string code, string variableName, IResource resource)
            : base(code, variableName, resource, measureUnit: "", format: "0")
        {
            ValueChanged += TwincatAnalogOutput_ValueChanged;
        }

        public override void Attach()
        {
            if ((Resource as TwincatResource).TryGetInstance(this, out ISymbol symbol))
            {
                Symbol = symbol as Symbol;
                ManagedType = (Symbol.DataType as DataType).ManagedType;

                if (typeof(bool).IsAssignableFrom(ManagedType))
                {
                    if (Symbol?.Connection?.IsConnected == true)
                    {
                        lock (LockObject)
                        {
                            try
                            {
                                Value = Convert.ToBoolean(Symbol.ReadAnyValue(ManagedType));
                            }
                            catch (Exception ex)
                            {
                                Logger.Error($"{ex.Message} occurred when reading {Code}");
                            }
                        }
                    }
                }
            }
        }

        private void TwincatAnalogOutput_ValueChanged(object sender, Core.ValueChangedEventArgs e)
        {
            lock (LockObject)
            {
                if (Symbol?.Connection?.IsConnected == true)
                {
                    try
                    {
                        Symbol.WriteValue(e.NewValueAsBool);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"{ex.Message} occurred when writing {Code}");
                    }
                }
            }
        }
    }
}
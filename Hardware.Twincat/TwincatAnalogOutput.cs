using Diagnostic;
using Extensions;
using System;
using TwinCAT.Ads.TypeSystem;
using TwinCAT.TypeSystem;

namespace Hardware.Twincat
{
    /// <summary>
    /// Implement a Twincat analog output
    /// </summary>
    public class TwincatAnalogOutput : TwincatChannel<double>, ITwincatChannel
    {
        /// <summary>
        /// Create a new instance of <see cref="TwincatAnalogOutput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="variableName">The variable name in Twincat</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        public TwincatAnalogOutput(string code, string variableName, IResource resource, string measureUnit = "",
            string format = "0.000") : base(code, variableName, resource, measureUnit, format)
        {
            if (resource.Status.Value == ResourceStatus.Executing)
                Attach();

            ValueChanged += TwincatAnalogOutput_ValueChanged;
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
                }
            }
        }

        private void TwincatAnalogOutput_ValueChanged(object sender, Core.ValueChangedEventArgs e)
        {
            lock(LockObject)
            {
                if(Symbol?.Connection?.IsConnected == true)
                {
                    try
                    {
                        Symbol.WriteValue(Convert.ChangeType(e.NewValueAsDouble, ManagedType));
                    }
                    catch(Exception ex)
                    {
                        Logger.Error($"{ex.Message} occurred when writing {Code}");
                    }
                }
            }
        }
    }
}
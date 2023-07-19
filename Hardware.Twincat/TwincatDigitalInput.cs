using Diagnostic;
using System;
using TwinCAT.Ads.TypeSystem;
using TwinCAT.TypeSystem;

namespace Hardware.Twincat
{
    /// <summary>
    /// Implement a Twincat digital input
    /// </summary>
    public class TwincatDigitalInput : TwincatChannel<bool>, ITwincatChannel
    {
        /// <summary>
        /// Create a new instance of <see cref="TwincatDigitalInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="variableName">The variable name</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        public TwincatDigitalInput(string code, string variableName, IResource resource)
            : base(code, variableName, resource, measureUnit: "", format: "0", ChannelType.DigitalInput)
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
                    Value = Convert.ToBoolean(e.Value);
                }
                catch (Exception ex)
                {
                    Logger.Error($"{ex.Message} occurred when reading {Code}");
                }
            }
        }
    }
}
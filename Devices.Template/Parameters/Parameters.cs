using Core.Parameters;

namespace Devices.Template.Parameters
{
    /// <summary>
    /// Contains all of the <see cref="Device{TChannel, TParameter}"/> parameters.
    /// So, in <see cref="Device{TChannel, TParameter}"/> <see cref="Parameters"/> is
    /// the actual type of the generics <see cref="TParameter"/>.
    /// </summary>
    public class Parameters
    {
        private NumericParameter dummy = new NumericParameter("DummyNumericParameter");

        // Fore each parameter, there must be a public property with the getter
        public NumericParameter Dummy => dummy;
    }
}
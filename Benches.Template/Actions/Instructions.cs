namespace Benches.Template.Actions
{
    public class Instructions
    {
        private static DummyInstruction dummy;
        private static SecondDummyInstruction secondDummy;

        // Fore each channel, there must be a public property with the getter
        public static DummyInstruction Dummy
        {
            get => dummy;
            set => dummy = value;
        }

        public static SecondDummyInstruction SecondDummy
        {
            get => secondDummy;
            set => secondDummy = value;
        }
    }
}
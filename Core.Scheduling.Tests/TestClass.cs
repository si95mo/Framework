using System;
using System.Threading.Tasks;

namespace Core.Scheduling.Tests
{
    [Serializable]
    public class TestClass
    {
        public TestClass()
        { }

        public void WriteOneLine() => Console.WriteLine("One line!");

        public void WriteTwoLine()
        {
            Console.WriteLine("First line!");
            Console.WriteLine("Second line!");
        }

        public void WriteMessage(string message) => Console.WriteLine(message);

        public void WriteTwoMessages(string firstMessage, string secondMessage)
        {
            Console.WriteLine(firstMessage);
            Console.WriteLine(secondMessage);
        }

        public string ConcatMessages(string firstMessage, string secondMessage, string thirdMessage)
            => firstMessage + secondMessage + thirdMessage;

        public double Add(double x, double y)
            => x + y;

        public double Mul(double x, double y)
            => x * y;

        public bool Negate(bool value)
            => !value;
    }

    [Serializable]
    public class DummyClass
    {
        public DummyClass()
        { }

        /// <summary>
        /// A method that perform a delay
        /// </summary>
        /// <param name="delay">The delay in ms</param>
        public async void WaitForShort(int delay)
        {
            Console.WriteLine($">> DummyClass start (short) :: {DateTime.Now:hh:mm:ss:ffff}");
            await Task.Delay(delay);
            Console.WriteLine($">> DummyClass stop (short) :: {DateTime.Now:hh:mm:ss:ffff}");
        }

        /// <summary>
        /// A method that perform a delay
        /// </summary>
        /// <param name="delay">The delay in ms</param>
        public async void WaitForLong(int delay)
        {
            Console.WriteLine($">> DummyClass start (long) :: {DateTime.Now:hh:mm:ss:ffff}");
            await Task.Delay(2 * delay);
            Console.WriteLine($">> DummyClass stop (long) :: {DateTime.Now:hh:mm:ss:ffff}");
        }
    }
}
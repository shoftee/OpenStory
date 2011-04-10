using System.Threading;

namespace OpenStory.Server.Emulation
{
    internal class Program
    {
        private static void Main()
        {
            var emulator = new Emulator();
            emulator.Start();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
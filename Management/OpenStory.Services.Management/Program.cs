using System;
using System.Threading;
using OpenStory.Services.Registry;

namespace OpenStory.Services.Management
{
    internal class Program
    {
        private static void Main()
        {
            // TODO: Add Management/Diagnostic hooks into all Services. Preferably easily customizable ones. Dunno about dynamic.

            var uri = new Uri("net.tcp://localhost/OpenStory/Registry");
            IRegistryService client = new RegistryServiceClient(uri);
            var result = client.GetRegistrations();

            Console.WriteLine(result);

            Thread.Sleep(Timeout.Infinite);
        }
    }
}

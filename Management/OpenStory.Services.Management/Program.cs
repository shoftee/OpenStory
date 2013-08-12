using System;
using System.Threading;
using OpenStory.Services.Registry;

namespace OpenStory.Services.Management
{
    internal static class Program
    {
        private static void Main()
        {
            // TODO: Add Management/Diagnostic hooks into all Services. Preferably easily customizable ones. Dunno about dynamic.
            
            var client = new RegistryServiceClient();
            var result = client.GetRegistrations();

            var registrations = result.GetResult(false);
            if (registrations != null)
            {
                Console.WriteLine("Registrations: [{0}]({1})", string.Join(", ", registrations), registrations.Length);
            }

            Thread.Sleep(Timeout.Infinite);
        }
    }
}

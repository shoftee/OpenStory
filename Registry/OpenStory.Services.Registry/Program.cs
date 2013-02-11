using System;
using System.ServiceModel;
using System.Threading;

namespace OpenStory.Services.Registry
{
    internal static class Program
    {
        private static void Main()
        {
            Console.Title = @"OpenStory - Registry Service";

            var service = new RegistryService();
            var uri = new Uri("net.tcp://localhost/OpenStory/Registry");
            using (var host = new ServiceHost(service, uri))
            {
                host.Open();

                Console.WriteLine("Registry hosted at: {0}", uri);

                Thread.Sleep(Timeout.Infinite);
            }
        }
    }
}

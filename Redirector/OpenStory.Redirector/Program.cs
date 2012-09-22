using System;
using System.Net;
using System.Threading;
using OpenStory.Common.Tools;

namespace OpenStory.Redirector
{
    internal static class Program
    {
        private static void Main()
        {
            string error;
            var parameters = ParameterList.FromEnvironment(out error);
            if (error != null)
            {
                ShowHelpMessage();
                Console.WriteLine(error);
                return;
            }

            var info = ParseCommandLine(parameters);

            if (info == null)
            {
                ShowHelpMessage();
                return;
            }

            Console.Title = "OpenStory.Redirector - " + info.Port;
            using (var redirector = new Redirector(info))
            {
                redirector.Bind();

                Thread.Sleep(Timeout.Infinite);
            }
        }

        private static IPEndPoint ParseCommandLine(ParameterList parameters)
        {
            string hostString = parameters["host"];
            string portString = parameters["port"];

            IPAddress ipAddress;
            if (String.IsNullOrEmpty(hostString) || !IPAddress.TryParse(hostString, out ipAddress))
            {
                return null;
            }

            int port;
            if (String.IsNullOrEmpty(portString) || !int.TryParse(portString, out port))
            {
                return null;
            }

            return new IPEndPoint(ipAddress, port);
        }

        private static void ShowHelpMessage()
        {
            Console.WriteLine("Please run the program with both of the arguments below.");
            Console.WriteLine("With the quotes.");
            Console.WriteLine();
            Console.WriteLine("--host=\"IP.Address.Like.This\"");
            Console.WriteLine("--port=\"port\"");
            Console.ReadKey();
        }
    }
}

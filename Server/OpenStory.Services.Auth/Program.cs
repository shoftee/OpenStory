using System;
using System.Threading;
using Ninject;
using OpenStory.Server;
using OpenStory.Server.Auth;

namespace OpenStory.Services.Auth
{
    internal static class Program
    {
        private static void Main()
        {
            var bootstrapper = Initialize();
            bootstrapper.Start();
        }

        private static Bootstrapper Initialize()
        {
            var kernel = new StandardKernel();
            kernel.Load(new AuthServerModule());
            return kernel.Get<Bootstrapper>();
        }
    }
}

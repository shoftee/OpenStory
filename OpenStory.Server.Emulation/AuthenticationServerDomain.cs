using System;
using OpenStory.Server.Emulation.Authentication;
using OpenStory.Server.Emulation.Helpers;

namespace OpenStory.Server.Emulation
{
    class AuthenticationServerDomain
    {
        private AppDomain domain;

        public AuthenticationServerDomain()
        {
            domain = AppDomainHelpers.GetNewDomain("OpenStory-Login");
        }

        public void StartAuthenticationServer()
        {
            domain.DoCallBack(() =>
            {
                AuthServer authServer = new AuthServer();
                authServer.Start();
            });
        }
    }
}

using System;
using OpenStory.Server.Emulation.Helpers;
using OpenStory.Server.Emulation.Login;

namespace OpenStory.Server.Emulation
{
    class LoginDomain
    {
        private AppDomain domain;

        public LoginDomain()
        {
            domain = AppDomainHelpers.GetNewDomain("OpenStory-Login");
        }

        public void StartLoginServer()
        {
            domain.DoCallBack(() =>
            {
                LoginServer loginServer = new LoginServer();
                loginServer.Start();
            });
        }
    }
}

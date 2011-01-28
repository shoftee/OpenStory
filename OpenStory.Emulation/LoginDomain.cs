using System;
using OpenStory.Emulation.Helpers;
using OpenStory.Emulation.Login;

namespace OpenStory.Emulation
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

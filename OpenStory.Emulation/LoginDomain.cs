using System;
using OpenStory.Emulation.Helpers;
using OpenStory.Emulation.Login;

namespace OpenStory.Emulation
{
    class LoginDomain
    {
        private AppDomain domain;

        private LoginDomain()
        {
            domain = AppDomainHelpers.GetNewDomain("OpenStory-Login");
            domain.DoCallBack(() =>
            {
                LoginServer loginServer = new LoginServer();
                loginServer.Start();
            });
        }
    }
}

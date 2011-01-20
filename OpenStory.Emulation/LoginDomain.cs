using System;
using OpenStory.Emulation.Helpers;
using OpenStory.Emulation.Login;

namespace OpenStory.Emulation
{
    class LoginDomain
    {
        private AppDomain loginDomain;

        public LoginDomain()
        {
            this.loginDomain = AppDomainHelpers.GetNewDomain("OpenStory-Login");
            this.loginDomain.DoCallBack(() =>
                                        {
                                            LoginServer loginServer = new LoginServer();
                                            loginServer.Start();
                                        });
        }
    }
}

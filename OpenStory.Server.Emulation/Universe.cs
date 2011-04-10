using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using OpenStory.Server.AccountService;
using OpenStory.Server.Communication;

namespace OpenStory.Server.Emulation
{
    /// <summary>
    /// Singleton class which provides cross-process access to game services.
    /// </summary>
    class Universe
    {
        private static readonly Universe Instance = new Universe();
        private Universe()
        {
            const string AccountServiceUrl = "ipc://OpenStory/AccountService.rem";
            RemotingHelpers.RegisterClientType<IAccountService>("AccountService", AccountServiceUrl);
            this.accountService = RemotingHelpers.GetRemoteObject<IAccountService>(AccountServiceUrl);
        }

        private IAccountService accountService;

        public static IAccountService Accounts
        {
            get { return Instance.accountService; }
        }
    }
}
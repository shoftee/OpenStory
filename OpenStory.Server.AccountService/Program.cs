using System;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using OpenStory.Server.Communication;

namespace OpenStory.Server.AccountService
{
    class Program
    {
        static void Main(string[] args)
        {
            IpcChannel channel = new IpcChannel("OpenStory");
            ChannelServices.RegisterChannel(channel, false);

            RemotingHelpers.RegisterServiceType<AccountService>("OpenStory");

            Console.WriteLine("AccountService registered.");
            Console.ReadLine();
        }
    }
}

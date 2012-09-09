using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using OpenStory.Services.Clients;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Management
{
    internal class Launcher
    {
    }

    internal sealed class OsServiceReference
    {
        public Uri ServiceUri { get; set; }

        public OsServiceReference()
        {
        }
    }
}

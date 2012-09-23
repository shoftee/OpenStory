using System;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Auth
{
    internal sealed class AuthNexusFragment : NexusServiceFragmentBase
    {
        public AuthNexusFragment(Uri uri) : base(uri) { }

        public override ServiceState TryGetServiceUri(Guid accessToken, out Uri uri)
        {
            return base.Service.TryGetAuthServiceUri(accessToken, out uri);
        }
    }
}
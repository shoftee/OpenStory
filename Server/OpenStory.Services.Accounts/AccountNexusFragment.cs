using System;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Account
{
    internal sealed class AccountNexusFragment : NexusServiceFragmentBase
    {
        public AccountNexusFragment(Uri uri) : base(uri) { }

        public override ServiceOperationResult TryGetServiceUri(Guid accessToken, out Uri uri)
        {
            return base.Service.TryGetAccountServiceUri(accessToken, out uri);
        }
    }
}
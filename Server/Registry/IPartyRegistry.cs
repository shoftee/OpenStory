namespace OpenMaple.Server.Registry
{
    /// <summary>
    /// Provides methods for creation and access of parties.
    /// </summary>
    public interface IPartyRegistry
    {
        IParty CreateParty(IPlayer leader);
        IParty GetPartyById(int partyId);
    }
}
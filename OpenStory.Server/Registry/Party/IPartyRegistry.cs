namespace OpenStory.Server.Registry.Party
{
    /// <summary>
    /// Provides methods for creation and access of parties.
    /// </summary>
    public interface IPartyRegistry
    {
        /// <summary>
        /// Creats a new party.
        /// </summary>
        /// <param name="leader">The leader of the party.</param>
        /// <returns>An <see cref="IParty"/> object representing the new party.</returns>
        IParty CreateParty(IPlayer leader);

        /// <summary>
        /// Gets a party by its ID.
        /// </summary>
        /// <param name="partyId">The ID of the party to query.</param>
        /// <returns>An <see cref="IParty"/> object representing the party if it was found.</returns>
        IParty GetPartyById(int partyId);
    }
}
using System.Data;
using OpenStory.Server.Data;

namespace OpenStory.Server.Auth.Data
{
    /// <summary>
    /// Represents a data object for a character.
    /// </summary>
    public sealed class AuthCharacter : Character
    {
        internal AuthCharacter(IDataRecord record) : base(record)
        {
        }
    }
}
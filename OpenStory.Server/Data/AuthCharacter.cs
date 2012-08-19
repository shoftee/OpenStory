using System.Data;
using OpenStory.Common.Game;

namespace OpenStory.Server.Data
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
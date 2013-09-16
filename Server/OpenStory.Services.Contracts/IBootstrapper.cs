using System;
using Ninject;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Represents a thingie which starts the rest of the thingies.
    /// </summary>
    public interface IBootstrapper
    {
        /// <summary>
        /// Starts ALL the things!
        /// </summary>
        void Start();
    }
}
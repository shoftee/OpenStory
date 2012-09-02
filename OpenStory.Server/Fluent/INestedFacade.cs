﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenStory.Server.Fluent
{
    /// <summary>
    /// Provides a method for returning to the parent facade object.
    /// </summary>
    /// <typeparam name="TParent">The type of the parent facade.</typeparam>
    public interface INestedFacade<TParent>
    {
        /// <summary>
        /// Returns to the parent facade.
        /// </summary>
        /// <returns>the parent facade.</returns>
        TParent Done();
    }
}
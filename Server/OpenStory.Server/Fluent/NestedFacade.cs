namespace OpenStory.Server.Fluent
{
    /// <summary>
    /// A base class for nested facades.
    /// </summary>
    /// <typeparam name="TParent">The type of the parent facade.</typeparam>
    internal abstract class NestedFacade<TParent> : INestedFacade<TParent>
    {
        /// <summary>
        /// Gets the parent facade.
        /// </summary>
        protected TParent Parent { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NestedFacade{TParent}"/> class.
        /// </summary>
        /// <param name="parent">The parent facade object.</param>
        protected NestedFacade(TParent parent)
        {
            this.Parent = parent;
        }

        #region Implementation of INestedFacade<TParent>

        /// <inheritdoc />
        public virtual TParent Done()
        {
            return this.Parent;
        }

        #endregion
    }
}
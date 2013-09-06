using System;
using System.Net.Sockets;

namespace OpenStory.Networking
{
    /// <summary>
    /// Represents an abstract asynchronous network operation buffer.
    /// </summary>
    public abstract class DescriptorBase : IDisposable
    {
        private bool isDisposed;

        /// <summary>
        /// Occurs when a connection error occurs.
        /// </summary>
        public event EventHandler<SocketErrorEventArgs> Error;

        /// <summary>
        /// Gets the <see cref="IDescriptorContainer">Container</see> of this descriptor.
        /// </summary>
        protected IDescriptorContainer Container { get; private set; }

        /// <summary>
        /// Gets the <see cref="SocketAsyncEventArgs"/> object for this descriptor.
        /// </summary>
        protected SocketAsyncEventArgs SocketArgs { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptorBase"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor initializes the 
        /// <see cref="Container"/> and <see cref="SocketArgs"/> properties.
        /// </remarks>
        /// <param name="container">The container of this descriptor.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="container" /> is <see langword="null"/>.
        /// </exception>
        protected DescriptorBase(IDescriptorContainer container)
        {
            Guard.NotNull(() => container, container);

            this.isDisposed = false;

            this.SocketArgs = new SocketAsyncEventArgs();
            this.Container = container;
        }

        /// <summary>
        /// Raises the <see cref="Error"/> event and closes the connection.
        /// </summary>
        /// <remarks>
        /// The event will be raised only if it has subscribers and the 
        /// <see cref="SocketAsyncEventArgs.SocketError"/> property of 
        /// <paramref name="args"/> is not <see cref="SocketError.Success"/>.
        /// </remarks>
        /// <param name="args">
        /// A <see cref="SocketAsyncEventArgs"/> object for the operation that caused the error.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="args" /> is <see langword="null"/>.
        /// </exception>
        protected void OnError(SocketAsyncEventArgs args)
        {
            Guard.NotNull(() => args, args);

            if (args.SocketError != SocketError.Success && this.Error != null)
            {
                this.Error(this, new SocketErrorEventArgs(args.SocketError));
            }

            if (args.SocketError != SocketError.OperationAborted)
            {
                this.Container.Close();
            }
        }

        /// <summary>
        /// Closes the <see cref="DescriptorBase"/> instance.
        /// </summary>
        public void Close()
        {
            this.Error = null;
            this.OnClosed();
        }

        /// <summary>
        /// A hook to the end of the publicly exposed <see cref="Close()"/> method.
        /// </summary>
        protected abstract void OnClosed();

        #region Implementation of IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Called when the instance is being released.
        /// </summary>
        /// <param name="disposing">Whether we are disposing or finalizing the instance.</param>
        protected virtual void Dispose(bool disposing)
        {
            this.Close();

            if (disposing && !this.isDisposed)
            {
                var args = this.SocketArgs;
                if (args != null)
                {
                    args.Dispose();
                    this.SocketArgs = null;
                }

                this.isDisposed = true;
            }
        }

        #endregion
    }
}

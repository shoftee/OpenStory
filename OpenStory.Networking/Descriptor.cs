using System;
using System.Net.Sockets;

namespace OpenStory.Networking
{
    abstract class Descriptor
    {
        /// <summary>
        /// The event is raised when there is a connection error.
        /// </summary>
        public event EventHandler<SocketErrorEventArgs> OnError;

        /// <summary>
        /// Gets the <see cref="IDescriptorContainer">Container</see> of this Descriptor.
        /// </summary>
        protected IDescriptorContainer Container { get; private set; }

        /// <summary>
        /// Gets the <see cref="SocketAsyncEventArgs"/> object for this Descriptor.
        /// </summary>
        protected SocketAsyncEventArgs SocketArgs { get; private set; }

        /// <summary>
        /// Initializes a new Descriptor.
        /// </summary>
        /// <remarks>
        /// This constructor initializes the 
        /// <see cref="Container"/> and <see cref="SocketArgs"/> properties.
        /// </remarks>
        /// <param name="container">The container of this descriptor.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="container" /> is <c>null</c>.
        /// </exception>
        protected Descriptor(IDescriptorContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            this.SocketArgs = new SocketAsyncEventArgs();
            this.Container = container;
        }

        /// <summary>
        /// Raises the <see cref="OnError"/> event and closes the connection.
        /// </summary>
        /// <remarks>
        /// The event will be raised only if it has subscribers and the 
        /// <see cref="SocketAsyncEventArgs.SocketError"/> 
        /// property is different than <see cref="SocketError.Success"/>.
        /// </remarks>
        /// <param name="args">
        /// A <see cref="SocketAsyncEventArgs"/> object 
        /// for the operation that caused the error.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="args" /> is <c>null</c>.
        /// </exception>
        protected void HandleError(SocketAsyncEventArgs args)
        {
            if (args == null) throw new ArgumentNullException("args");

            if (args.SocketError != SocketError.Success && this.OnError != null)
            {
                this.OnError(this, new SocketErrorEventArgs(args.SocketError));
            }
            this.Container.Close();
        }

        /// <summary>
        /// A hook to the end of the publicly 
        /// exposed <see cref="Close()"/> method.
        /// </summary>
        protected abstract void CloseImpl();

        public void Close()
        {
            this.OnError = null;
            this.CloseImpl();
        }
    }
}
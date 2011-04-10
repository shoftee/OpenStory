using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;

namespace OpenStory.Server.Communication
{
    /// <summary>
    /// Provides helper methods for remoting operations.
    /// </summary>
    public static class RemotingHelpers
    {
        /// <summary>
        /// Registers a type on the service end as a well-known singleton.
        /// </summary>
        /// <param name="ipcChannelName">The name of the IpcChannel to use.</param>
        public static void RegisterServiceType<T>(string ipcChannelName) where T : MarshalByRefObject
        {
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(T),
                GetObjectUrlForType(typeof(T)),
                WellKnownObjectMode.Singleton);
        }

        /// <summary>
        /// Gets a URI for a remote type.
        /// </summary>
        /// <param name="pipeName">The name of the IPC channel.</param>
        /// <param name="remoteType">The remote type.</param>
        /// <returns>A URI in string form.</returns>
        public static string GetUriForServiceType(string pipeName, Type remoteType)
        {
            return String.Format("ipc://{0}/{1}",
                                 pipeName, GetObjectUrlForType(remoteType));
        }

        private static string GetObjectUrlForType(Type remoteType)
        {
            return String.Format("{0}.rem", remoteType.Name);
        }

        /// <summary>
        /// Registers a channel for the given service and then registers a type as well-known.
        /// </summary>
        /// <remarks>
        /// This method is only for the use of client processes.
        /// </remarks>
        /// <typeparam name="T">The type to register.</typeparam>
        /// <param name="serviceName">The name of the service.</param>
        /// <param name="objectUrl">The object URL for the type.</param>
        public static void RegisterClientType<T>(string serviceName, string objectUrl)
        {
            IpcChannel clientChannel = new IpcChannel(serviceName);
            ChannelServices.RegisterChannel(clientChannel, false);

            RemotingConfiguration.RegisterWellKnownClientType(typeof(T), objectUrl);
        }

        /// <summary>
        /// Gets a remote instance of a type.
        /// </summary>
        /// <remarks>
        /// This method call is equivalent to the expression 
        /// <c>(T) <see cref="Activator.GetObject(Type,string)">Activator.GetObject</see>(typeof(T), objectUrl)</c>.
        /// </remarks>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <param name="objectUrl">The object URL for the type.</param>
        /// <returns>The object casted to <typeparamref name="T"/>.</returns>
        public static T GetRemoteObject<T>(string objectUrl)
        {
            return (T) Activator.GetObject(typeof(T), objectUrl);           
        }
    }
}
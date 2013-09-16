using System;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Provides methods for creating client channels to services.
    /// </summary>
    /// <typeparam name="TChannel">The type of the service.</typeparam>
    public interface IServiceClientProvider<out TChannel>
        where TChannel : class
    {
        /// <summary>
        /// Gets a service channel using discovery.
        /// </summary>
        TChannel CreateChannel();
    }

    /// <summary>
    /// Extensions for service client objects?
    /// </summary>
    public static class ServiceClientExtensions
    {
        public static void Call<TChannel>(this IServiceClientProvider<TChannel> provider, Action<TChannel> action) 
            where TChannel : class
        {
            var channel = provider.CreateChannel();
            using (channel.AsDisposable())
            {
                action(channel);
            }
        }

        public static TResult Call<TChannel, TResult>(this IServiceClientProvider<TChannel> provider, Func<TChannel, TResult> func)
            where TChannel : class
        {
            var channel = provider.CreateChannel();
            using (channel.AsDisposable())
            {
                return func(channel);
            }
        }
    }
}
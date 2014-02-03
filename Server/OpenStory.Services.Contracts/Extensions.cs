using System;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Provides extension methods. Which are useful. For stuff.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Returns the provided object as an instance of <see cref="IDisposable"/>.
        /// </summary>
        public static IDisposable AsDisposable(this object obj)
        {
            return obj as IDisposable;
        }

        /// <summary>
        /// Calls the specified client action by creating a client instance using the specified service client provider.
        /// </summary>
        public static void Call<TChannel>(this IServiceClientProvider<TChannel> provider, Action<TChannel> action)
            where TChannel : class
        {
            var channel = provider.CreateChannel();
            using (channel.AsDisposable())
            {
                action(channel);
            }
        }

        /// <summary>
        /// Calls the specified client function by creating a client instance using the specified service client provider.
        /// </summary>
        public static TResult Call<TChannel, TResult>(this IServiceClientProvider<TChannel> provider, Func<TChannel, TResult> func)
            where TChannel : class
        {
            var channel = provider.CreateChannel();
            using (channel.AsDisposable())
            {
                var result = func(channel);
                return result;
            }
        }
    }
}

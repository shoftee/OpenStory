using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Provides extension methods. Which are useful. For stuff.
    /// </summary>
    public static class Extensions
    {
        public static IDisposable AsDisposable(this object obj)
        {
            return obj as IDisposable;
        }

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
                var result = func(channel);
                return result;
            }
        }
    }
}

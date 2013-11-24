using System;
using FluentAssertions.Specialized;

namespace OpenStory.Tests.Helpers
{
    public static class FluentAssertionsExtensions
    {
        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <remarks>
        /// This method is used to ignore the return value of an expression easily.
        /// </remarks>
        /// <param name="object">Some object.</param>
        public static void Whatever(this object @object)
        {
        }

        public static ExceptionAssertions<TException> WithMessagePrefix<TException>(
            this ExceptionAssertions<TException> assertions,
            string substring,
            string reason = "",
            params object[] reasonArgs) where TException : Exception
        {
            return assertions.WithMessage(substring + "*", reason, reasonArgs);
        }

        public static ExceptionAssertions<TException> WithMessageSuffix<TException>(
            this ExceptionAssertions<TException> assertions,
            string substring,
            string reason = "",
            params object[] reasonArgs) where TException : Exception
        {
            return assertions.WithMessage("*" + substring, reason, reasonArgs);
        }

        public static ExceptionAssertions<TException> WithMessageSubstring<TException>(
            this ExceptionAssertions<TException> assertions, 
            string substring, 
            string reason = "", 
            params object[] reasonArgs) where TException : Exception
        {
            return assertions.WithMessage("*" + substring + "*", reason, reasonArgs);
        }

        public static ExceptionAssertions<TException> WithMessageFormat<TException>(
            this ExceptionAssertions<TException> assertions, 
            string format, 
            params object[] formatArgs) where TException : Exception
        {
            return assertions.WithMessage(string.Format(format, formatArgs));
        }
    }
}

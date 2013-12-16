using System;
using FluentAssertions;
using FluentAssertions.Primitives;
using FluentAssertions.Specialized;
using OpenStory.Common.Game;

namespace OpenStory.Tests.Helpers
{
    public class PointSAssertions : ReferenceTypeAssertions<PointS, PointSAssertions>
    {
        protected override string Context
        {
            get { return "PointS"; }
        }

        public PointSAssertions(PointS subject)
        {
            this.Subject = subject;
        }

        public AndConstraint<PointSAssertions> HaveComponents(short x, short y)
        {
            this.Subject.X.Should().Be(x);
            this.Subject.Y.Should().Be(y);

            return new AndConstraint<PointSAssertions>(this);
        }
    }

    public static class FluentAssertionsExtensions
    {
        public static PointSAssertions Should(this PointS subject)
        {
            return new PointSAssertions(subject);
        }

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

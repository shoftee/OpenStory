using System;
using System.Reflection;

namespace OpenStory.Server.Emulation.Helpers
{
    /// <summary>
    /// Contains helper methods for Reflection routines.
    /// </summary>
    public static class ReflectionHelpers
    {
        private static readonly object[] EmptyObjectArray = new object[0];

        /// <summary>
        /// Inspects a <see cref="MemberInfo"/> object for an attribute of a specific type.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute to look for.</typeparam>
        /// <param name="memberInfo">The <see cref="MemberInfo"/> to inspect.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="memberInfo" /> is <c>null</c>.</exception>
        /// <returns>The attribute if there is one, otherwise null.</returns>
        public static TAttribute GetAttribute<TAttribute>(MemberInfo memberInfo)
            where TAttribute : Attribute
        {
            if (memberInfo == null) throw new ArgumentNullException("memberInfo");
            return Attribute.GetCustomAttribute(memberInfo, typeof (TAttribute)) as TAttribute;
        }

        /// <summary>
        /// Determines whether a <see cref="MemberInfo"/> has an attribute of a specific type.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute to look for.</typeparam>
        /// <param name="memberInfo">The <see cref="MemberInfo"/> to inspect.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="memberInfo" /> is <c>null</c>.
        /// </exception>
        /// <returns>true if <paramref name="memberInfo"/> has the attribute; otherwise, false.</returns>
        public static bool HasAttribute<TAttribute>(MemberInfo memberInfo)
        {
            if (memberInfo == null) throw new ArgumentNullException("memberInfo");
            return memberInfo.IsDefined(typeof (TAttribute), false);
        }

        /// <summary>
        /// Executes the method reflected in the given <see cref="MethodBase"/> as a Func(T) delegate.
        /// This method is equivalent to the expression <c>(T) <paramref name="method"/>.Invoke(null, new object[0])</c>.
        /// </summary>
        /// <typeparam name="T">The return type of the reflected method.</typeparam>
        /// <param name="method">The <see cref="MethodBase"/> to invoke.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="method" /> is <c>null</c>.
        /// </exception>
        /// <returns>The result from the invocation of <paramref name="method"/>.</returns>
        public static T InvokeStaticFunc<T>(MethodBase method)
        {
            if (method == null) throw new ArgumentNullException("method");
            return (T) method.Invoke(null, EmptyObjectArray);
        }
    }
}
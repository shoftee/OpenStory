using System;
using System.Reflection;

namespace OpenStory.Common.Tools
{
    public class ReflectionUtils
    {
        private static readonly object[] EmptyObjectArray = new object[0];

        /// <summary>
        /// Inspects <paramref name="memberInfo"/> for an attribute of type <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute to look for.</typeparam>
        /// <param name="memberInfo">The <see cref="T:System.Reflection.MemberInfo"/> to inspect.</param>
        /// <returns>The attribute if there is one, otherwise null.</returns>
        public static TAttribute GetAttribute<TAttribute>(MemberInfo memberInfo)
            where TAttribute : Attribute
        {
            return Attribute.GetCustomAttribute(memberInfo, typeof (TAttribute)) as TAttribute;
        }

        /// <summary>
        /// Determines whether <paramref name="memberInfo"/> has an attribute of type <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute to look for.</typeparam>
        /// <param name="memberInfo">The <see cref="T:System.Reflection.MemberInfo"/> to inspect.</param>
        /// <returns>true if <paramref name="memberInfo"/> has the attribute; otherwise, false.</returns>
        public static bool HasAttribute<TAttribute>(MemberInfo memberInfo)
        {
            return memberInfo.IsDefined(typeof (TAttribute), false);
        }

        /// <summary>
        /// Executes the method reflected in the given <see cref="T:System.Reflection.MemberInfo"/> as a Func(T) delegate.
        /// This method is equivalent to the expression (T) method.Invoke(null, new object[0]).
        /// </summary>
        /// <typeparam name="T">The return type of the reflected method.</typeparam>
        /// <param name="method">The <see cref="T:System.Reflection.MemberInfo"/> to invoke.</param>
        /// <returns>The result from the invocation of <paramref name="method"/>.</returns>
        public static T InvokeFunc<T>(MethodInfo method)
        {
            return (T) method.Invoke(null, EmptyObjectArray);
        }
    }

    /// <summary>
    /// Represents a {<see cref="T:System.Reflection.MemberInfo">MemberInfo</see>, <see cref="T:System.Attribute">Attribute</see>} pair.
    /// </summary>
    /// <typeparam name="TMemberInfo">The type of the member in the pair.</typeparam>
    /// <typeparam name="TAttribute">The type of the attribute in the pair.</typeparam>
    public class MetadataPair<TMemberInfo, TAttribute>
        where TMemberInfo : MemberInfo
        where TAttribute : Attribute
    {
        public MetadataPair(TMemberInfo memberInfo, TAttribute attribute)
        {
            this.MemberInfo = memberInfo;
            this.Attribute = attribute;
        }

        public TMemberInfo MemberInfo { get; private set; }
        public TAttribute Attribute { get; private set; }
    }
}
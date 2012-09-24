using System.Reflection;
using System;

namespace OpenStory.Services.Management.Helpers
{
    /// <summary>
    /// Represents a {<see cref="MemberInfo"/>, <see cref="Attribute"/>} pair.
    /// </summary>
    /// <typeparam name="TMemberInfo">The type of the member in the pair.</typeparam>
    /// <typeparam name="TAttribute">The type of the attribute in the pair.</typeparam>
    public sealed class MetadataPair<TMemberInfo, TAttribute>
        where TMemberInfo : MemberInfo
        where TAttribute : Attribute
    {
        /// <summary>
        /// Gets the <see cref="MemberInfo"/> object of this pair.
        /// </summary>
        public TMemberInfo MemberInfo { get; private set; }

        /// <summary>
        /// Gets the <see cref="Attribute"/> object of this pair.
        /// </summary>
        public TAttribute Attribute { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="MetadataPair{TMemberInfo,TAttribute}"/>.
        /// </summary>
        /// <param name="memberInfo">The <see cref="MemberInfo"/> object for this pair.</param>
        /// <param name="attribute">The <see cref="Attribute"/> object for this pair.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="memberInfo"/> 
        /// or <paramref name="attribute"/> are null.
        /// </exception>
        public MetadataPair(TMemberInfo memberInfo, TAttribute attribute)
        {
            if (memberInfo == null)
            {
                throw new ArgumentNullException("memberInfo");
            }
            if (attribute == null)
            {
                throw new ArgumentNullException("attribute");
            }

            this.MemberInfo = memberInfo;
            this.Attribute = attribute;
        }
    }
}

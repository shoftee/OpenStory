using System;

namespace OpenStory.Server.Emulation
{
    /// <summary>
    /// Indicates that the method is an initialization method 
    /// and will be called by the initialization routines.
    /// </summary>
    /// <remarks>
    /// This attribute is to be used in conjunction with the
    /// <see cref="ServerModuleAttribute"/>.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class InitializationMethodAttribute : Attribute
    {
    }
}

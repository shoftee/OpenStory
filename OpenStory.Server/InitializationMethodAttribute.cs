using System;

namespace OpenMaple
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class InitializationMethodAttribute : Attribute {}
}
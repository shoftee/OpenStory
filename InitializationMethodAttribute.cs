using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMaple
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class InitializationMethodAttribute : Attribute { }
}

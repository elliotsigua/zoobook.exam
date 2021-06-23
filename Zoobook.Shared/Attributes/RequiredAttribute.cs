using System;

namespace Zoobook.Shared
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class RequiredAttribute : Attribute { }
}

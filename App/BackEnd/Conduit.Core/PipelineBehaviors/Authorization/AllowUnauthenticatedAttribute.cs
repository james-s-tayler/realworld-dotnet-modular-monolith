using System;

namespace Conduit.Core.PipelineBehaviors.Authorization
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AllowUnauthenticatedAttribute : Attribute
    {
        
    }
}
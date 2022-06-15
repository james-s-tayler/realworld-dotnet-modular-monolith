using System;

namespace Application.Core.PipelineBehaviors.Authorization
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AllowUnauthenticatedAttribute : Attribute
    {
        
    }
}
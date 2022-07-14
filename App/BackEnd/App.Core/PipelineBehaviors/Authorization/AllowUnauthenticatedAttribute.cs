using System;

namespace App.Core.PipelineBehaviors.Authorization
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AllowUnauthenticatedAttribute : Attribute
    {
        
    }
}
using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentValidation;

namespace App.Core.PipelineBehaviors.Validation;

public static class FluentValidationCustomization
{
    public static void ConfigureFluentValidation()
    {
        ValidatorOptions.Global.PropertyNameResolver = LowerCasePropertyNameResolver;
    }
    
    public static readonly Func<Type, MemberInfo, LambdaExpression, string> LowerCasePropertyNameResolver =
        (type, memberInfo, expression) =>
        {
            return memberInfo != null ? memberInfo.Name.ToLower() : null;
        };
}
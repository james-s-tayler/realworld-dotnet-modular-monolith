using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentValidation;
using FluentValidation.Internal;

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
            if ( memberInfo != null && !string.IsNullOrEmpty(memberInfo.Name))
            {
                return memberInfo.Name.ToLower();
            }
            
            if (expression != null) {
                var chain = PropertyChain.FromExpression(expression);
                if (chain.Count > 0) return chain.ToString();
            }
            
            return null;
        };
}
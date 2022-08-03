using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Conduit.API.OpenApi
{
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        private readonly string _scheme = "Token";

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if ( !context
                    .MethodInfo
                    .GetCustomAttributes(true)
                    .OfType<AllowAnonymousAttribute>()
                    .Any() )
            {
                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                BearerFormat = _scheme,
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = _scheme
                                }
                            },
                            new List<string>()
                        }
                    }
                };
            }
        }
    }
}
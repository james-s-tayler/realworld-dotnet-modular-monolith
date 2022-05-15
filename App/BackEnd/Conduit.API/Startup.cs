using System;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Conduit.API.Filters;
using Conduit.API.OpenApi;
using Conduit.API.Formatters;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Conduit.API
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// The application configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JwtSettings:ValidAudience"],
                    ValidIssuer = Configuration["JwtSettings:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSettings:Secret"]))
                };
            });
            services.AddAuthorization();

            // Add framework services.
            services
                // Don't need the full MVC stack for an API, see https://andrewlock.net/comparing-startup-between-the-asp-net-core-3-templates/
                .AddControllers(options =>
                {
                    options.InputFormatters.Insert(0, new InputFormatterStream());
                });
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("1.0.0", new OpenApiInfo
                    {
                        Title = "Conduit API",
                        Description = "Conduit API (ASP.NET Core 5.0)",
                        TermsOfService = new Uri("https://github.com/openapitools/openapi-generator"),
                        Contact = new OpenApiContact
                        {
                            Name = "RealWorld",
                            Url = new Uri("https://realworld.io"),
                            Email = ""
                        },
                        License = new OpenApiLicense
                        {
                            Name = "NoLicense",
                            Url = new Uri("https://opensource.org/licenses/MIT")
                        },
                        Version = "1.0.0",
                    });
                    c.CustomSchemaIds(type => type.FriendlyId(true));
                    c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{Assembly.GetEntryAssembly().GetName().Name}.xml");

                    var scheme = JwtBearerDefaults.AuthenticationScheme;
                    c.AddSecurityDefinition(scheme, new OpenApiSecurityScheme
                    {
                        Description = $"JWT Authorization header using the {scheme} scheme.",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Scheme = scheme,
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "Bearer {token}"
                    });
                    c.OperationFilter<SecurityRequirementsOperationFilter>();
                    // Include DataAnnotation attributes on Controller Action parameters as OpenAPI validation rules (e.g required, pattern, ..)
                    // Use [ValidateModelState] on Actions to actually validate it in C# as well!
                    c.OperationFilter<GeneratePathParamsValidationFilter>();
                });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsProduction())
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }
            app.UseSwagger(c =>
                    {
                        c.RouteTemplate = "openapi/{documentName}/openapi.json";
                    })
                    .UseSwaggerUI(c =>
                    {
                        // set route prefix to openapi, e.g. http://localhost:8080/openapi/index.html
                        c.RoutePrefix = "openapi";
                        //TODO: Either use the SwaggerGen generated OpenAPI contract (generated from C# classes)
                        c.SwaggerEndpoint("/openapi/1.0.0/openapi.json", "Conduit API");
                    });
            
            app.UseDefaultFiles();
            app.UseStaticFiles();
            
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                    .RequireAuthorization();
            });
        }
    }
}

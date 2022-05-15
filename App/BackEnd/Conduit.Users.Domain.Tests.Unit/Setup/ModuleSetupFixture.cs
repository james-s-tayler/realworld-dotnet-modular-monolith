using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Conduit.Core.Context;
using Conduit.Identity.Domain.Configuration;
using Conduit.Identity.Domain.Entities;
using Conduit.Identity.Domain.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using ScottBrady91.AspNetCore.Identity;

namespace Conduit.Identity.Domain.Tests.Unit.Setup
{
    public class ModuleSetupFixture : IDisposable
    {
        public Fixture AutoFixture { get; } = new ();
        public string Token { get; }= "jwt";
        public string PlainTextPassword { get; } = "soloyolo";
        public static BCryptPasswordHasher<User> PasswordHasher = new ();
        public User User { get; set; }
        public IMediator Mediator { get; set; }
        internal IdentityModule Module { get; }
        public IServiceCollection Services { get; }
        public ConfigurationBuilder Configuration { get; }
        public Mock<IUserContext> UserContext { get; } = new ();
        public Mock<IUserRepository> UserRepo { get; }

        public ModuleSetupFixture()
        {
            User = new User
            {
                Id = 1,
                Email = "solo@yolo.com",
                Username = "soloyolo",
                Password = PasswordHasher.HashPassword(null, PlainTextPassword)
            };
            
            UserRepo = new Mock<IUserRepository>();
            UserRepo.Setup(repository => repository.Exists(It.Is<int>(id => id == User.Id))).Returns(Task.FromResult(true));
            UserRepo.Setup(repository => repository.GetById(It.Is<int>(id => id == User.Id))).Returns(Task.FromResult(User));
            UserRepo.Setup(repository => repository.ExistsByEmail(It.Is<string>(email => email.Equals(User.Email)))).Returns(Task.FromResult(true));
            UserRepo.Setup(repository => repository.GetByEmail(It.Is<string>(email => email.Equals(User.Email)))).Returns(Task.FromResult(User));
            
            Module = new IdentityModule();
            Services = new ServiceCollection();
            Configuration = new ConfigurationBuilder();
            Configuration.AddInMemoryCollection(new Dictionary<string, string>
            {
                {$"{nameof(JwtSettings)}:{nameof(JwtSettings.Secret)}", "secretsecretsecretsecretsecretsecret"},
                {$"{nameof(JwtSettings)}:{nameof(JwtSettings.ValidIssuer)}", "issuer"},
                {$"{nameof(JwtSettings)}:{nameof(JwtSettings.ValidAudience)}", "audience"},
            });
            
            WithDefaultUserContext();
            
            Services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
            Module.AddServices(Configuration.Build(), Services);
            Module.ReplaceSingleton(UserRepo.Object);
            Module.ReplaceScoped(UserContext.Object);
            Module.ReplaceSingleton<IPasswordHasher<User>, BCryptPasswordHasher<User>>(PasswordHasher);

            var provider = Services.BuildServiceProvider();
            Mediator = provider.GetRequiredService<IMediator>();
        }
        
        // ReSharper disable once InconsistentNaming
        public void BuildDIContainer()
        {
            
        }

        public void WithUnauthenticatedUserContext()
        {
            UserContext.Reset();
            UserContext.SetupGet(context => context.IsAuthenticated).Returns(false);
        }
        
        public void WithDefaultUserContext()
        {
            WithUserContextReturning(User, Token);
        }

        public void WithRandomUserContext()
        {
            var user = AutoFixture.Create<User>();
            var token = AutoFixture.Create<string>();
            WithUserContextReturning(user, token);
        }
        
        public void WithUserContextReturning(User user, string token)
        {
            UserContext.Reset();
            UserContext.SetupGet(context => context.IsAuthenticated).Returns(true);
            UserContext.SetupGet(context => context.UserId).Returns(user.Id);
            UserContext.SetupGet(context => context.Username).Returns(user.Username);
            UserContext.SetupGet(context => context.Email).Returns(user.Email);
            UserContext.SetupGet(context => context.Token).Returns(token);
        }

        public void Dispose() {}
    }
}
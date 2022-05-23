using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Conduit.Core.Context;
using Conduit.Users.Domain.Configuration;
using Conduit.Users.Domain.Entities;
using Conduit.Users.Domain.Infrastructure.Repositories;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using ScottBrady91.AspNetCore.Identity;
using Serilog;

namespace Conduit.Users.Domain.Tests.Unit.Setup
{
    public class UsersModuleSetupFixture : IDisposable
    {
        public Fixture AutoFixture { get; } = new ();
        public string Token { get; } = "jwt";
        public string PlainTextPassword { get; } = "soloyolo";
        public BCryptPasswordHasher<User> PasswordHasher = new ();
        public User ExistingUser { get; }
        public User ExistingUser2 { get; }
        public IMediator Mediator { get; set; }
        internal UsersModule Module { get; }
        public IServiceCollection Services { get; }
        public ConfigurationBuilder Configuration { get; }
        public Mock<IUserContext> UserContext { get; } = new ();
        public Mock<IUserRepository> UserRepo { get; }

        public UsersModuleSetupFixture()
        {
            
            ExistingUser = new User
            {
                Id = 1,
                Email = "solo@yolo.com",
                Username = "soloyolo",
                Password = PasswordHasher.HashPassword(null, PlainTextPassword)
            };
            
            ExistingUser2 = new User
            {
                Id = 2,
                Email = "solo2@yolo2.com",
                Username = "soloyolo2",
                Password = PasswordHasher.HashPassword(null, PlainTextPassword)
            };
            
            UserRepo = new Mock<IUserRepository>();
            WithUserRepoContainingDefaultUsers();

            Module = new UsersModule();
            Services = new ServiceCollection();
            Configuration = new ConfigurationBuilder();
            Configuration.AddInMemoryCollection(new Dictionary<string, string>
            {
                {$"{nameof(JwtSettings)}:{nameof(JwtSettings.Secret)}", "secretsecretsecretsecretsecretsecret"},
                {$"{nameof(JwtSettings)}:{nameof(JwtSettings.ValidIssuer)}", "issuer"},
                {$"{nameof(JwtSettings)}:{nameof(JwtSettings.ValidAudience)}", "audience"},
            });
            
            WithAuthenticatedUserContext();

            Services.AddLogging(builder =>
            {
                builder.AddSerilog();
                builder.SetMinimumLevel(LogLevel.Debug);
            });
            Module.AddServices(Configuration.Build(), Services);
            Module.ReplaceSingleton(UserRepo.Object);
            Module.ReplaceScoped(UserContext.Object);
            Module.ReplaceSingleton<IPasswordHasher<User>, BCryptPasswordHasher<User>>(PasswordHasher);

            var provider = Services.BuildServiceProvider();
            Mediator = provider.GetRequiredService<IMediator>();
        }


        public void WithUserRepoContainingDefaultUsers()
        {
            WithUserRepoContainingUsers(ExistingUser, ExistingUser2);
        }
        public void WithUserRepoContainingUsers(params User[] users)
        {
            UserRepo.Reset();
            foreach (var user in users)
            {
                AddUserToUserRepo(user);
            }
        }

        public void AddUserToUserRepo([NotNull] User user)
        {
            UserRepo.Setup(repository => repository.Exists(It.Is<int>(id => id == user.Id))).Returns(Task.FromResult(true));
            UserRepo.Setup(repository => repository.GetById(It.Is<int>(id => id == user.Id))).Returns(Task.FromResult(user));
            UserRepo.Setup(repository => repository.ExistsByEmail(It.Is<string>(email => email.Equals(user.Email)))).Returns(Task.FromResult(true));
            UserRepo.Setup(repository => repository.GetByEmail(It.Is<string>(email => email.Equals(user.Email)))).Returns(Task.FromResult(user));
            UserRepo.Setup(repository => repository.ExistsByUsername(It.Is<string>(username => username.Equals(user.Username)))).Returns(Task.FromResult(true));
        }

        public void WithUnauthenticatedUserContext()
        {
            UserContext.Reset();
            UserContext.SetupGet(context => context.IsAuthenticated).Returns(false);
        }
        
        public void WithAuthenticatedUserContext()
        {
            WithUserContextReturning(ExistingUser, Token);
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
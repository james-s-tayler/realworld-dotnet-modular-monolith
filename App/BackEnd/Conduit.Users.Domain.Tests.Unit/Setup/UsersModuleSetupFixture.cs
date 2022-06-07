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
using Microsoft.Extensions.Hosting;
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
        public string PlainTextPassword { get; } = "soloyolo99";
        public BCryptPasswordHasher<User> PasswordHasher = new ();
        public User ExistingUser { get; }
        public User ExistingUser2 { get; }
        public IMediator Mediator { get; set; }
        internal UsersModule Module { get; }
        public IServiceCollection Services { get; }
        public ConfigurationBuilder Configuration { get; }
        private Mock<IHostEnvironment> _hostEnvironment;

        internal IUserRepository UserRepository { get; }
        public Mock<IUserContext> UserContext { get; } = new ();
        
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
            
            Module = new UsersModule();
            _hostEnvironment = new Mock<IHostEnvironment>();
            _hostEnvironment.Setup(environment => environment.EnvironmentName).Returns("Test");
            Services = new ServiceCollection();
            Configuration = new ConfigurationBuilder();
            Configuration.AddInMemoryCollection(new Dictionary<string, string>
            {
                {$"{nameof(JwtSettings)}:{nameof(JwtSettings.Secret)}", "secretsecretsecretsecretsecretsecret"},
                {$"{nameof(JwtSettings)}:{nameof(JwtSettings.ValidIssuer)}", "issuer"},
                {$"{nameof(JwtSettings)}:{nameof(JwtSettings.ValidAudience)}", "audience"},
                {$"DatabaseConfig:{UsersDomain.Assembly.GetName().Name}:DatabaseName", "users"}
            });
            
            WithAuthenticatedUserContext();

            Services.AddLogging(builder =>
            {
                builder.AddSerilog();
                builder.SetMinimumLevel(LogLevel.Debug);
            });
            
            Module.InitializeModule(Configuration.Build(), _hostEnvironment.Object, Services);
            Module.ReplaceTransient(UserContext.Object);
            Module.ReplaceTransient<IPasswordHasher<User>, BCryptPasswordHasher<User>>(PasswordHasher);

            var provider = Services.BuildServiceProvider();
            Mediator = provider.GetRequiredService<IMediator>();
            UserRepository = provider.GetService<IUserRepository>();
            WithUserRepoContainingDefaultUsers().GetAwaiter().GetResult();
        }

        public async Task WithUserRepoContainingDefaultUsers()
        {
            await WithUserRepoContainingUsers(ExistingUser, ExistingUser2);
        }
        public async Task WithUserRepoContainingUsers(params User[] users)
        {
            await UserRepository.DeleteAll();
            foreach (var user in users)
            {
                await AddUserToUserRepo(user);
            }
        }

        public async Task AddUserToUserRepo([NotNull] User user)
        {
            await UserRepository.Create(user);
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
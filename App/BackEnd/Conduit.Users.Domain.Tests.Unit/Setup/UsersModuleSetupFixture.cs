using System.Collections.Generic;
using System.Threading.Tasks;
using Conduit.Core.Modules;
using Conduit.Core.Testing;
using Conduit.Users.Domain.Configuration;
using Conduit.Users.Domain.Entities;
using Conduit.Users.Domain.Infrastructure.Repositories;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ScottBrady91.AspNetCore.Identity;

namespace Conduit.Users.Domain.Tests.Unit.Setup
{
    public class UsersModuleSetupFixture : AbstractModuleSetupFixture
    {
        public string PlainTextPassword { get; } = "soloyolo99";
        internal BCryptPasswordHasher<User> PasswordHasher = new ();
        internal User ExistingUser { get; private set; }
        internal User ExistingUser2 { get; private set; }
        
        internal IUserRepository UserRepository { get; private set; }
        
        protected override void AddConfiguration(IDictionary<string, string> configuration)
        {
            configuration.Add($"{nameof(JwtSettings)}:{nameof(JwtSettings.Secret)}", "secretsecretsecretsecretsecretsecret");
            configuration.Add($"{nameof(JwtSettings)}:{nameof(JwtSettings.ValidIssuer)}", "issuer");
            configuration.Add($"{nameof(JwtSettings)}:{nameof(JwtSettings.ValidAudience)}", "audience");
        }
        
        protected override void ReplaceServices(AbstractModule module)
        {
            module.ReplaceTransient<IPasswordHasher<User>, BCryptPasswordHasher<User>>(PasswordHasher);
        }
        
        protected override void SetupPostProcess(ServiceProvider provider)
        {
            ExistingUser = new User
            {
                Id = 1,
                Email = "solo@yolo.com",
                Username = "soloyolo",
                Bio = "I work at statefarm",
                Password = PasswordHasher.HashPassword(null, PlainTextPassword)
            };
            
            ExistingUser2 = new User
            {
                Id = 2,
                Email = "solo2@yolo2.com",
                Username = "soloyolo2",
                Bio = "I work at statefarm",
                Password = PasswordHasher.HashPassword(null, PlainTextPassword)
            };
            UserRepository = provider.GetService<IUserRepository>();
            WithUserRepoContainingDefaultUsers().GetAwaiter().GetResult();
            WithAuthenticatedUserContext();
        }

        public UsersModuleSetupFixture() : base(new UsersModule()) {}

        public async Task WithUserRepoContainingDefaultUsers()
        {
            await WithUserRepoContainingUsers(ExistingUser, ExistingUser2);
        }
        internal async Task WithUserRepoContainingUsers(params User[] users)
        {
            await UserRepository.DeleteAll();
            foreach (var user in users)
            {
                await AddUserToUserRepo(user);
            }
        }

        internal async Task AddUserToUserRepo([NotNull] User user)
        {
            await UserRepository.Create(user);
        }

        public void WithAuthenticatedUserContext()
        {
            WithUserContextReturning(true, ExistingUser.Id, ExistingUser.Username, ExistingUser.Email, AuthenticatedUserToken);
        }
    }
}
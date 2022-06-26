using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Core.Modules;
using Application.Core.Testing;
using Application.Users.Domain.Entities;
using Application.Users.Domain.Infrastructure.Repositories;
using Application.Users.Domain.Setup.Configuration;
using Application.Users.Domain.Setup.Module;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ScottBrady91.AspNetCore.Identity;

namespace Application.Users.Domain.Tests.Unit.Setup
{
    public class UsersModuleSetupFixture : AbstractModuleSetupFixture
    {
        public string PlainTextPassword { get; } = "soloyolo99";
        internal BCryptPasswordHasher<UserEntity> PasswordHasher = new ();
        internal UserEntity ExistingUserEntity { get; private set; }
        internal UserEntity ExistingUser2 { get; private set; }
        
        internal IUserRepository UserRepository { get; private set; }

        protected override void AddConfiguration(IDictionary<string, string> configuration)
        {
            configuration.Add($"{nameof(JwtSettings)}:{nameof(JwtSettings.Secret)}", "secretsecretsecretsecretsecretsecret");
            configuration.Add($"{nameof(JwtSettings)}:{nameof(JwtSettings.ValidIssuer)}", "issuer");
            configuration.Add($"{nameof(JwtSettings)}:{nameof(JwtSettings.ValidAudience)}", "audience");
        }
        
        protected override void ReplaceServices(AbstractModule module)
        {
            module.ReplaceTransient<IPasswordHasher<UserEntity>, BCryptPasswordHasher<UserEntity>>(PasswordHasher);
        }
        
        protected override void SetupPostProcess(ServiceProvider provider)
        {
            ExistingUserEntity = new UserEntity
            {
                Id = 1,
                Email = "solo@yolo.com",
                Username = "soloyolo",
                Bio = "I work at statefarm",
                Password = PasswordHasher.HashPassword(null, PlainTextPassword)
            };
            
            ExistingUser2 = new UserEntity
            {
                Id = 2,
                Email = "solo2@yolo2.com",
                Username = "soloyolo2",
                Bio = "I work at statefarm",
                Password = PasswordHasher.HashPassword(null, PlainTextPassword)
            };
            UserRepository = provider.GetService<IUserRepository>();
            WithAuthenticatedUserContext();
        }

        public UsersModuleSetupFixture() : base(new UsersModule()) {}

        public async Task WithUserRepoContainingDefaultUsers()
        {
            await WithUserRepoContainingUsers(ExistingUserEntity, ExistingUser2);
        }
        internal async Task WithUserRepoContainingUsers(params UserEntity[] users)
        {
            await UserRepository.DeleteAll();
            foreach (var user in users)
            {
                await AddUserToUserRepo(user);
            }
        }

        internal async Task AddUserToUserRepo([NotNull] UserEntity userEntity)
        {
            await UserRepository.Create(userEntity);
        }

        public void WithAuthenticatedUserContext()
        {
            WithUserContextReturning(true, ExistingUserEntity.Id, ExistingUserEntity.Username, ExistingUserEntity.Email, AuthenticatedUserToken);
        }
    }
}
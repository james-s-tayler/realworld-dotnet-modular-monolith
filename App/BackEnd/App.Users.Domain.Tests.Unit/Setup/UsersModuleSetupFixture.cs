using System.Collections.Generic;
using App.Core.Modules;
using App.Core.Testing;
using App.Users.Domain.Entities;
using App.Users.Domain.Infrastructure.Repositories;
using App.Users.Domain.Setup.Configuration;
using App.Users.Domain.Setup.Module;
using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ScottBrady91.AspNetCore.Identity;

namespace App.Users.Domain.Tests.Unit.Setup
{
    public class UsersModuleSetupFixture : AbstractModuleSetupFixture
    {
        public string PlainTextPassword { get; } = "soloyolo99";
        private string HashedPassword { get; }
        
        internal BCryptPasswordHasher<UserEntity> PasswordHasher = new ();
        internal UserEntity ExistingUserEntity { get; private set; }
        internal UserEntity ExistingUser2 { get; private set; }
        
        internal IUserRepository UserRepository { get; private set; }

        public UsersModuleSetupFixture() : base(new UsersModule())
        {
            HashedPassword = PasswordHasher.HashPassword(null, PlainTextPassword);
        }
        
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
            UserRepository = provider.GetService<IUserRepository>();
        }

        public override void PerTestSetup()
        {
            var existingUser = new UserEntity
            {
                Id = AuthenticatedUserId,
                Email = AuthenticatedUserEmail,
                Username = AuthenticatedUserUsername,
                Password = HashedPassword,
                Bio = AuthenticatedUserBio
            };
            UserRepository.Create(existingUser).GetAwaiter().GetResult();
            ExistingUserEntity = UserRepository.GetByUsername(existingUser.Username).GetAwaiter().GetResult();
            
            //in all of the other modules we can get away with letting the base class call this before the call to PerTestSetup,
            //but that's because the other modules users table allows us to insert the randomly generated id directly into the table
            //whereas UserRepository.Create() returns the Id of the created user whose info we then need to populated into the UserContext 
            WithUserContextReturning(ExistingUserEntity.Id,
                ExistingUserEntity.Username,
                ExistingUserEntity.Email,
                AutoFixture.Create<string>());

            var existingUser2 = AutoFixture.Create<UserEntity>();
            existingUser2.Email = $"{AutoFixture.Create<string>()}@{AutoFixture.Create<string>()}.com";
            existingUser2.Password = HashedPassword;
            UserRepository.Create(existingUser2).GetAwaiter().GetResult();
            ExistingUser2 = UserRepository.GetByUsername(existingUser2.Username).GetAwaiter().GetResult();
        }
    }
}
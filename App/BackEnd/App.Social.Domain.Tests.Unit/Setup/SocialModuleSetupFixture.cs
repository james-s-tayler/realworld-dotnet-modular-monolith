using System.Collections.Generic;
using App.Core.Modules;
using App.Core.Testing;
using App.Social.Domain.Entities;
using App.Social.Domain.Infrastructure.Repositories;
using App.Social.Domain.Setup.Module;
using App.Users.Domain.Contracts.DTOs;
using App.Users.Domain.Contracts.Operations.Commands.RegisterUser;
using AutoFixture;
using Microsoft.Extensions.DependencyInjection;

namespace App.Social.Domain.Tests.Unit.Setup
{
    public class SocialModuleSetupFixture : AbstractModuleSetupFixture
    {
        internal IUserRepository UserRepository { get; private set; }
        
        internal UserEntity AuthenticatedUser { get; private set; }
        internal UserEntity FollowedUser { get; private set; }
        internal UserEntity UnfollowedUser { get; private set; }
        
        public SocialModuleSetupFixture() : base(new SocialModule()) {}

        protected override void AddConfiguration(IDictionary<string, string> configuration)
        {
            
        }

        protected override void ReplaceServices(AbstractModule module)
        {
            
        }

        protected override void SetupPostProcess(ServiceProvider provider)
        {
            UserRepository = provider.GetService<IUserRepository>();
        }

        public override void PerTestSetup()
        {
            var registerAuthenticatedUser = new RegisterUserCommandResult
            {
                RegisteredUser = new UserDTO
                {
                    Id = AuthenticatedUserId,
                    Bio = AuthenticatedUserBio,
                    Image = AuthenticatedUserImage,
                    Username = AuthenticatedUserUsername
                }
            };
            
            Mediator.Publish(registerAuthenticatedUser).GetAwaiter().GetResult();
            AuthenticatedUser = UserRepository.GetByUsername(AuthenticatedUserUsername).GetAwaiter().GetResult();
            
            var registerFollowedUser = AutoFixture.Create<RegisterUserCommandResult>();
            registerFollowedUser.RegisteredUser.Email = $"{AutoFixture.Create<string>()}@{AutoFixture.Create<string>()}.com";
            Mediator.Publish(registerFollowedUser).GetAwaiter().GetResult();
            UserRepository.FollowUser(registerFollowedUser.RegisteredUser.Id).GetAwaiter().GetResult();
            FollowedUser = UserRepository.GetByUsername(registerFollowedUser.RegisteredUser.Username).GetAwaiter().GetResult();
            
            var registerUnfollowedUser = AutoFixture.Create<RegisterUserCommandResult>();
            registerUnfollowedUser.RegisteredUser.Email = $"{AutoFixture.Create<string>()}@{AutoFixture.Create<string>()}.com";
            Mediator.Publish(registerUnfollowedUser).GetAwaiter().GetResult();
            UnfollowedUser = UserRepository.GetByUsername(registerUnfollowedUser.RegisteredUser.Username).GetAwaiter().GetResult();
        }
    }
}
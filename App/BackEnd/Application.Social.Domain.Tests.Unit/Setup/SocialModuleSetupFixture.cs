using System.Collections.Generic;
using Application.Core.Modules;
using Application.Core.Testing;
using Application.Social.Domain.Entities;
using Application.Social.Domain.Infrastructure.Repositories;
using Application.Social.Domain.Setup.Module;
using Application.Users.Domain.Contracts.DTOs;
using Application.Users.Domain.Contracts.Operations.Commands.RegisterUser;
using Microsoft.Extensions.DependencyInjection;
using AutoFixture;

namespace Application.Social.Domain.Tests.Unit.Setup
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
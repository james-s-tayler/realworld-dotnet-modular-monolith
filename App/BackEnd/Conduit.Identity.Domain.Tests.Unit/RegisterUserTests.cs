using System;
using System.Threading.Tasks;
using Conduit.Identity.Domain.Contracts.RegisterUser;
using Conduit.Identity.Domain.Entities;
using Conduit.Identity.Domain.Interactions.Outbound.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Conduit.Identity.Domain.Tests.Unit
{
    public class RegisterUserTests
    {

        [Fact]
        public async Task GivenANewUser_WhenRegistered_ThenNewUserIdReturned()
        {
            var random = new Random();
            var userId = random.Next(1, 1000);
            
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(repository => repository.Create(It.IsAny<User>())).Returns(Task.FromResult(userId));

            var services = new ServiceCollection();
            services.AddTransient(_ => userRepo.Object);

            services.AddMediatR(IdentityDomain.Assembly);
            //services.AddValidatorsFromAssembly(IdentityDomain.Assembly);
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CommandValidationPipelineBehavior<,>));

            var provider = services.BuildServiceProvider();
            var mediatR = provider.GetRequiredService<IMediator>();

            var registerUserCommand = new RegisterUserCommand
            {
                NewUser = new NewUserDTO
                {
                    Email = "solo@yolo.com",
                    Username = "soloyolo",
                    Password = "soloyolo"
                }
            };

            var result = await mediatR.Send(registerUserCommand);
            
            Assert.Equal(userId, result.UserId);
        }
    }
}
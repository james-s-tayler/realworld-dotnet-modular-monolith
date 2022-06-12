using System.Threading.Tasks;
using MediatR;

namespace Conduit.Users.Domain.Infrastructure.Services
{
    internal class SocialService : ISocialService
    {
        private readonly IMediator _mediator;

        public SocialService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<bool> IsFollowing(int userId)
        {
            return default;
        }
    }
}
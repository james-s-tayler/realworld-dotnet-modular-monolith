using System.Threading.Tasks;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Users.Domain.Contracts;
using App.Users.Domain.Contracts.Operations.Queries.GetProfile;
using MediatR;

namespace App.Content.Domain.Infrastructure.Services
{
    internal class SocialService : ISocialService
    {
        private readonly IMediator _mediator;

        public SocialService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<OperationResponse<GetProfileQueryResult>> GetProfile(string username)
        {
            return await _mediator.Send(new GetProfileQuery { Username = username });
        }
    }
}
using System.Threading.Tasks;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.Social.Domain.Contracts.Operations.Queries.GetProfile;
using MediatR;

namespace Application.Content.Domain.Infrastructure.Services
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
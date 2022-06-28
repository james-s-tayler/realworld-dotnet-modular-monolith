using System.Threading;
using System.Threading.Tasks;
using Application.Content.Domain.Contracts.Operations.Commands.PublishArticle;
using Application.Content.Domain.Entities;
using Application.Content.Domain.Infrastructure.Mappers;
using Application.Content.Domain.Infrastructure.Repositories;
using Application.Content.Domain.Infrastructure.Services;
using Application.Core.Context;
using Application.Core.PipelineBehaviors.OperationResponse;
using JetBrains.Annotations;
using MediatR;

namespace Application.Content.Domain.Operations.Commands.PublishArticle
{
    internal class PublishArticleCommandHandler : IRequestHandler<PublishArticleCommand, OperationResponse<PublishArticleCommandResult>>
    {
        private readonly IUserContext _userContext;
        private readonly ISocialService _socialService;
        private readonly IArticleRepository _articleRepository;

        public PublishArticleCommandHandler([NotNull] IArticleRepository articleRepository, 
            [NotNull] ISocialService socialService, 
            [NotNull] IUserContext userContext)
        {
            _articleRepository = articleRepository;
            _socialService = socialService;
            _userContext = userContext;
        }

        public async Task<OperationResponse<PublishArticleCommandResult>> Handle(PublishArticleCommand request, CancellationToken cancellationToken)
        {
            var userEntity = new UserEntity { UserId = _userContext.UserId, Username = _userContext.Username };
            var articleId = await _articleRepository.Create(request.NewArticle.ToArticleEntity(userEntity));
            
            var article = await _articleRepository.GetById(articleId);
            var getProfileQueryResult = await _socialService.GetProfile(article.Author.Username);
            var authorProfile = getProfileQueryResult.Response.Profile;

            return OperationResponseFactory.Success(new PublishArticleCommandResult
            {
                Article = article.ToArticleDTO(authorProfile)
            });
        }
    }
}
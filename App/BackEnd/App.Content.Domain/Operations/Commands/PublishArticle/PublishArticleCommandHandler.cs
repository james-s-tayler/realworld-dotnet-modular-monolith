using System.Threading;
using System.Threading.Tasks;
using App.Content.Domain.Contracts.Operations.Commands.PublishArticle;
using App.Content.Domain.Entities;
using App.Content.Domain.Infrastructure.Mappers;
using App.Content.Domain.Infrastructure.Repositories;
using App.Content.Domain.Infrastructure.Services;
using App.Core.Context;
using App.Core.PipelineBehaviors.OperationResponse;
using JetBrains.Annotations;
using MediatR;

namespace App.Content.Domain.Operations.Commands.PublishArticle
{
    internal class PublishArticleCommandHandler : IRequestHandler<PublishArticleCommand, OperationResponse<PublishArticleCommandResult>>
    {
        private readonly IUserContext _userContext;
        private readonly IUsersService _usersService;
        private readonly IArticleRepository _articleRepository;

        public PublishArticleCommandHandler([NotNull] IArticleRepository articleRepository,
            [NotNull] IUsersService usersService,
            [NotNull] IUserContext userContext)
        {
            _articleRepository = articleRepository;
            _usersService = usersService;
            _userContext = userContext;
        }

        public async Task<OperationResponse<PublishArticleCommandResult>> Handle(PublishArticleCommand request, CancellationToken cancellationToken)
        {
            var userEntity = new UserEntity { UserId = _userContext.UserId, Username = _userContext.Username };
            var articleId = await _articleRepository.Create(request.NewArticle.ToArticleEntity(userEntity));

            var article = await _articleRepository.GetById(articleId, _userContext.UserId);
            var getProfileQueryResult = await _usersService.GetProfile(article.Author.Username);
            var authorProfile = getProfileQueryResult.Response.Profile;

            return OperationResponseFactory.Success(new PublishArticleCommandResult
            {
                ArticleId = articleId,
                UserId = _userContext.UserId,
                Article = article.ToArticleDTO(authorProfile)
            });
        }
    }
}
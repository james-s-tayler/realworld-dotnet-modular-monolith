using System.Threading;
using System.Threading.Tasks;
using App.Core.Context;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Content.Domain.Contracts.Operations.Queries.GetArticleById;
using App.Content.Domain.Entities;
using App.Content.Domain.Infrastructure.Mappers;
using App.Content.Domain.Infrastructure.Repositories;
using App.Content.Domain.Infrastructure.Services;
using JetBrains.Annotations;
using MediatR;

namespace App.Content.Domain.Operations.Queries.GetArticleById
{
    internal class GetArticleByIdQueryHandler : IRequestHandler<GetArticleByIdQuery, OperationResponse<GetArticleByIdQueryResult>>
    {
        private readonly IUserContext _userContext;
        private readonly IUsersService _usersService;
        private readonly IArticleRepository _articleRepository;

        public GetArticleByIdQueryHandler([NotNull] IUserContext userContext,
            [NotNull] IArticleRepository articleRepository,
            [NotNull] IUsersService usersService)
        {
            _userContext = userContext;
            _articleRepository = articleRepository;
            _usersService = usersService;
        }

        public async Task<OperationResponse<GetArticleByIdQueryResult>> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
        {
            var article = await _articleRepository.GetById(request.ArticleId, _userContext.UserId);
            if ( article == null )
                return OperationResponseFactory.NotFound<GetArticleByIdQuery, OperationResponse<GetArticleByIdQueryResult>>(typeof(ArticleEntity), request.ArticleId);

            //what if this operation fails???
            var getProfileQueryResult = await _usersService.GetProfile(article.Author.Username);
            var authorProfile = getProfileQueryResult.Response.Profile;

            return OperationResponseFactory.Success(new GetArticleByIdQueryResult
            {
                Article = article.ToArticleDTO(authorProfile)
            });
        }
    }
}
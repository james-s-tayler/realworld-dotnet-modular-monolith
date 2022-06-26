using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Content.Domain.Contracts.Operations.Queries.GetSingleArticle;
using Application.Content.Domain.Infrastructure.Mappers;
using Application.Content.Domain.Infrastructure.Repositories;
using Application.Core.Context;
using Application.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Application.Content.Domain.Operations.Queries.GetSingleArticle
{
    internal class GetSingleArticleQueryHandler : IRequestHandler<GetSingleArticleQuery, OperationResponse<GetSingleArticleQueryResult>>
    {
        private readonly IUserContext _userContext;
        private readonly IArticleRepository _articleRepository;

        public GetSingleArticleQueryHandler(IUserContext userContext,
            IArticleRepository articleRepository)
        {
            _userContext = userContext;
            _articleRepository = articleRepository;
        }

        public async Task<OperationResponse<GetSingleArticleQueryResult>> Handle(GetSingleArticleQuery request, CancellationToken cancellationToken)
        {
            var article = await _articleRepository.GetBySlug(request.Slug);
            if (article == null)
                throw new ArgumentNullException(nameof(article));

            //get author profile here
            
            return new OperationResponse<GetSingleArticleQueryResult>(new GetSingleArticleQueryResult
            {
                Article = article.ToArticleDTO(null, false, 0)
            });
        }
    }
}
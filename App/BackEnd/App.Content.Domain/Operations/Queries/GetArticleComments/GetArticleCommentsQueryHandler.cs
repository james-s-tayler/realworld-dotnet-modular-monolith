using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Content.Domain.Contracts.DTOs;
using App.Content.Domain.Contracts.Operations.Queries.GetArticleComments;
using App.Content.Domain.Infrastructure.Mappers;
using App.Content.Domain.Infrastructure.Repositories;
using App.Content.Domain.Infrastructure.Services;
using App.Core.PipelineBehaviors.OperationResponse;
using JetBrains.Annotations;
using MediatR;

namespace App.Content.Domain.Operations.Queries.GetArticleComments
{
    internal class GetArticleCommentsQueryHandler : IRequestHandler<GetArticleCommentsQuery, OperationResponse<GetArticleCommentsQueryResult>>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ISocialService _socialService;
        private readonly ICommentRepository _commentRepository;

        public GetArticleCommentsQueryHandler([NotNull] IArticleRepository articleRepository,
            [NotNull] ISocialService socialService,
            [NotNull] ICommentRepository commentRepository)
        {
            _articleRepository = articleRepository;
            _socialService = socialService;
            _commentRepository = commentRepository;
        }

        public async Task<OperationResponse<GetArticleCommentsQueryResult>> Handle(GetArticleCommentsQuery request, CancellationToken cancellationToken)
        {
            if (await ArticleNotFound(request.Slug))
            {
                return OperationResponseFactory.Success(new GetArticleCommentsQueryResult
                {
                    Comments = new List<SingleCommentDTO>()
                });
            }

            var article = await _articleRepository.GetBySlug(request.Slug);
            var comments = await _commentRepository.GetCommentsByArticleId(article.Id);

            var commentDtos = new List<SingleCommentDTO>();

            //rather than looping some sort of bulk-get would be more performant
            foreach (var comment in comments)
            {
                var getProfileQueryResult = await _socialService.GetProfile(comment.Author.Username);
                commentDtos.Add(comment.ToCommentDTO(getProfileQueryResult.Response.Profile));
            }
            
            return OperationResponseFactory.Success(new GetArticleCommentsQueryResult
            {
                Comments = commentDtos
            });
        }

        private async Task<bool> ArticleNotFound(string articleSlug)
        {
            return !await _articleRepository.ExistsBySlug(articleSlug);
        }
    }
}
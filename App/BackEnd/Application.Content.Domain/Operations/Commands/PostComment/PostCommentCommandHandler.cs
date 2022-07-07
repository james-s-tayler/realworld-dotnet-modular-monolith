using System.Threading;
using System.Threading.Tasks;
using Application.Content.Domain.Contracts.Operations.Commands.PostComment;
using Application.Content.Domain.Infrastructure.Mappers;
using Application.Content.Domain.Infrastructure.Repositories;
using Application.Content.Domain.Infrastructure.Services;
using Application.Core.PipelineBehaviors.OperationResponse;
using JetBrains.Annotations;
using MediatR;

namespace Application.Content.Domain.Operations.Commands.PostComment
{
    internal class PostCommentCommandHandler : IRequestHandler<PostCommentCommand, OperationResponse<PostCommentCommandResult>>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly ISocialService _socialService;
        

        public PostCommentCommandHandler([NotNull] IArticleRepository articleRepository, 
            [NotNull] ISocialService socialService, 
            [NotNull] ICommentRepository commentRepository)
        {
            _articleRepository = articleRepository;
            _socialService = socialService;
            _commentRepository = commentRepository;
        }

        public async Task<OperationResponse<PostCommentCommandResult>> Handle(PostCommentCommand request, CancellationToken cancellationToken)
        {
            var article = await _articleRepository.GetBySlug(request.ArticleSlug);

            var commentEntity = await _commentRepository.PostComment(request.NewComment.ToComment(article));
            var getProfileQueryResult = await _socialService.GetProfile(commentEntity.Author.Username);
            var commenterProfile = getProfileQueryResult.Response.Profile;

            return OperationResponseFactory.Success(new PostCommentCommandResult
            {
                Comment = commentEntity.ToCommentDTO(commenterProfile)
            });
        }
    }
}
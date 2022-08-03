using System.Threading;
using System.Threading.Tasks;
using App.Content.Domain.Contracts.Operations.Commands.PostComment;
using App.Content.Domain.Entities;
using App.Content.Domain.Infrastructure.Mappers;
using App.Content.Domain.Infrastructure.Repositories;
using App.Content.Domain.Infrastructure.Services;
using App.Core.Context;
using App.Core.PipelineBehaviors.OperationResponse;
using JetBrains.Annotations;
using MediatR;

namespace App.Content.Domain.Operations.Commands.PostComment
{
    internal class PostCommentCommandHandler : IRequestHandler<PostCommentCommand, OperationResponse<PostCommentCommandResult>>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IUserContext _userContext;
        private readonly IUsersService _usersService;


        public PostCommentCommandHandler([NotNull] IArticleRepository articleRepository,
            [NotNull] IUsersService usersService,
            [NotNull] ICommentRepository commentRepository,
            [NotNull] IUserContext userContext)
        {
            _articleRepository = articleRepository;
            _usersService = usersService;
            _commentRepository = commentRepository;
            _userContext = userContext;
        }

        public async Task<OperationResponse<PostCommentCommandResult>> Handle(PostCommentCommand request, CancellationToken cancellationToken)
        {
            var article = await _articleRepository.GetBySlug(request.ArticleSlug, _userContext.UserId);
            var commentAuthor = new UserEntity { UserId = _userContext.UserId, Username = _userContext.Username };

            var commentEntity = await _commentRepository.PostComment(commentAuthor, request.NewComment.ToComment(article));
            var getProfileQueryResult = await _usersService.GetProfile(commentEntity.Author.Username);
            var commenterProfile = getProfileQueryResult.Response.Profile;

            return OperationResponseFactory.Success(new PostCommentCommandResult
            {
                Comment = commentEntity.ToCommentDTO(commenterProfile)
            });
        }
    }
}
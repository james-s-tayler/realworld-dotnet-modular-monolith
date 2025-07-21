using System.Threading;
using System.Threading.Tasks;
using App.Content.Domain.Contracts.Operations.Commands.DeleteComment;
using App.Content.Domain.Infrastructure.Repositories;
using App.Core.Context;
using App.Core.PipelineBehaviors.OperationResponse;
using JetBrains.Annotations;
using MediatR;

namespace App.Content.Domain.Operations.Commands.DeleteComment
{
    internal class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, OperationResponse<DeleteCommentCommandResult>>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserContext _userContext;

        public DeleteCommentCommandHandler([NotNull] ICommentRepository commentRepository,
            [NotNull] IUserContext userContext)
        {
            _commentRepository = commentRepository;
            _userContext = userContext;
        }

        public async Task<OperationResponse<DeleteCommentCommandResult>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            // should be done as IAuthorizer
            if (!await _commentRepository.ExistsByUserAndId(_userContext.UserId, request.CommentId) )
            {
                return OperationResponseFactory
                    .NotAuthorized<DeleteCommentCommand, OperationResponse<DeleteCommentCommandResult>>("comment doesn't belong to user");
            }
            
            await _commentRepository.DeleteComment(request.ArticleSlug, request.CommentId);

            return OperationResponseFactory.Success(new DeleteCommentCommandResult());
        }
    }
}
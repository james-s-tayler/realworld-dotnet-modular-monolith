using System.Threading;
using System.Threading.Tasks;
using Application.Content.Domain.Contracts.Operations.Commands.DeleteComment;
using Application.Content.Domain.Infrastructure.Repositories;
using Application.Core.PipelineBehaviors.OperationResponse;
using JetBrains.Annotations;
using MediatR;

namespace Application.Content.Domain.Operations.Commands.DeleteComment
{
    internal class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, OperationResponse<DeleteCommentCommandResult>>
    {
        private readonly ICommentRepository _commentRepository;

        public DeleteCommentCommandHandler([NotNull] ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<OperationResponse<DeleteCommentCommandResult>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            await _commentRepository.DeleteComment(request.ArticleSlug, request.CommentId);

            return OperationResponseFactory.Success(new DeleteCommentCommandResult());
        }
    }
}
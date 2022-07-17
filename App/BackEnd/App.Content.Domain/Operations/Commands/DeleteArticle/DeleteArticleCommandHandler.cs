using System.Threading;
using System.Threading.Tasks;
using App.Content.Domain.Contracts.Operations.Commands.DeleteArticle;
using App.Content.Domain.Entities;
using App.Content.Domain.Infrastructure.Repositories;
using App.Core.Context;
using App.Core.PipelineBehaviors.OperationResponse;
using JetBrains.Annotations;
using MediatR;

namespace App.Content.Domain.Operations.Commands.DeleteArticle
{
    internal class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand, OperationResponse<DeleteArticleCommandResult>>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IUserContext _userContext;

        public DeleteArticleCommandHandler([NotNull] IArticleRepository articleRepository, 
            [NotNull] IUserContext userContext)
        {
            _articleRepository = articleRepository;
            _userContext = userContext;
        }

        public async Task<OperationResponse<DeleteArticleCommandResult>> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
        {
            if (!await _articleRepository.ExistsBySlug(request.Slug))
                return OperationResponseFactory.NotFound<DeleteArticleCommand, OperationResponse<DeleteArticleCommandResult>>(typeof(ArticleEntity), request.Slug);
            
            await _articleRepository.Delete(_userContext.UserId, request.Slug);

            return OperationResponseFactory.Success(new DeleteArticleCommandResult());
        }
    }
}
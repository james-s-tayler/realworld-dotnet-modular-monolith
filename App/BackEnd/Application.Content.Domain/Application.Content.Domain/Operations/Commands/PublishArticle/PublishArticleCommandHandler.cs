using System.Threading;
using System.Threading.Tasks;
using Application.Content.Domain.Contracts.Operations.Commands.PublishArticle;
using Application.Content.Domain.Infrastructure.Mappers;
using Application.Content.Domain.Infrastructure.Repositories;
using Application.Core.Context;
using Application.Core.PipelineBehaviors.OperationResponse;
using JetBrains.Annotations;
using MediatR;

namespace Application.Content.Domain.Operations.Commands.PublishArticle
{
    internal class PublishArticleCommandHandler : IRequestHandler<PublishArticleCommand, OperationResponse<PublishArticleCommandResult>>
    {
        private readonly IUserContext _userContext;
        private readonly IArticleRepository _articleRepository;

        public PublishArticleCommandHandler([NotNull] IUserContext userContext,
            [NotNull] IArticleRepository articleRepository)
        {
            _userContext = userContext;
            _articleRepository = articleRepository;
        }

        public async Task<OperationResponse<PublishArticleCommandResult>> Handle(PublishArticleCommand request, CancellationToken cancellationToken)
        {
            var articleId = await _articleRepository.Create(request.NewArticle.ToArticleEntity());
            
            var article = await _articleRepository.GetById(articleId);
            
            //add author info from user context
            
            return new OperationResponse<PublishArticleCommandResult>(new PublishArticleCommandResult
            {
                Article = article.ToArticleDTO(null, false, 0)
            });
        }
    }
}
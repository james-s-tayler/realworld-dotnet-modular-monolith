using System.Threading;
using System.Threading.Tasks;
using Application.Content.Domain.Contracts.Operations.Commands.EditArticle;
using Application.Content.Domain.Entities;
using Application.Content.Domain.Infrastructure.Mappers;
using Application.Content.Domain.Infrastructure.Repositories;
using Application.Content.Domain.Infrastructure.Services;
using Application.Core.Context;
using Application.Core.PipelineBehaviors.OperationResponse;
using JetBrains.Annotations;
using MediatR;

namespace Application.Content.Domain.Operations.Commands.EditArticle
{
    internal class EditArticleCommandHandler : IRequestHandler<EditArticleCommand, OperationResponse<EditArticleCommandResult>>
    {
        private readonly IUserContext _userContext;
        private readonly ISocialService _socialService;
        private readonly IArticleRepository _articleRepository;

        public EditArticleCommandHandler([NotNull] IArticleRepository articleRepository, 
            [NotNull] ISocialService socialService, 
            [NotNull] IUserContext userContext)
        {
            _articleRepository = articleRepository;
            _socialService = socialService;
            _userContext = userContext;
        }

        public async Task<OperationResponse<EditArticleCommandResult>> Handle(EditArticleCommand request, CancellationToken cancellationToken)
        {
            var article = await _articleRepository.GetBySlug(request.Slug);
            if(article == null)
                return OperationResponseFactory.NotFound<EditArticleCommand, OperationResponse<EditArticleCommandResult>>(typeof(ArticleEntity), request.Slug);

            article.Title = !string.IsNullOrEmpty(request.UpdatedArticle.Title) ? request.UpdatedArticle.Title : article.Title;
            article.Description = !string.IsNullOrEmpty(request.UpdatedArticle.Description) ? request.UpdatedArticle.Description : article.Description;
            article.Body = !string.IsNullOrEmpty(request.UpdatedArticle.Body) ? request.UpdatedArticle.Body : article.Body;

            await _articleRepository.Update(article);
            article = await _articleRepository.GetById(article.Id);
            
            var getProfileQueryResult = await _socialService.GetProfile(article.Author.Username);
            var authorProfile = getProfileQueryResult.Response.Profile;

            return OperationResponseFactory.Success(new EditArticleCommandResult
            {
                Article = article.ToArticleDTO(authorProfile)
            });
        }
    }
}
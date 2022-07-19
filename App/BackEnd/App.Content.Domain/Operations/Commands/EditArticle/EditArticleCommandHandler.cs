using System.Threading;
using System.Threading.Tasks;
using App.Content.Domain.Contracts.Operations.Commands.EditArticle;
using App.Content.Domain.Entities;
using App.Content.Domain.Infrastructure.Mappers;
using App.Content.Domain.Infrastructure.Repositories;
using App.Content.Domain.Infrastructure.Services;
using App.Core.Context;
using App.Core.PipelineBehaviors.OperationResponse;
using JetBrains.Annotations;
using MediatR;

namespace App.Content.Domain.Operations.Commands.EditArticle
{
    internal class EditArticleCommandHandler : IRequestHandler<EditArticleCommand, OperationResponse<EditArticleCommandResult>>
    {
        private readonly IUserContext _userContext;
        private readonly IUsersService _usersService;
        private readonly IArticleRepository _articleRepository;

        public EditArticleCommandHandler([NotNull] IArticleRepository articleRepository, 
            [NotNull] IUsersService usersService, 
            [NotNull] IUserContext userContext)
        {
            _articleRepository = articleRepository;
            _usersService = usersService;
            _userContext = userContext;
        }

        public async Task<OperationResponse<EditArticleCommandResult>> Handle(EditArticleCommand request, CancellationToken cancellationToken)
        {
            var article = await _articleRepository.GetBySlug(request.Slug, _userContext.UserId);
            if(article == null)
                return OperationResponseFactory.NotFound<EditArticleCommand, OperationResponse<EditArticleCommandResult>>(typeof(ArticleEntity), request.Slug);

            article.Title = !string.IsNullOrEmpty(request.UpdatedArticle.Title) ? request.UpdatedArticle.Title : article.Title;
            article.Description = !string.IsNullOrEmpty(request.UpdatedArticle.Description) ? request.UpdatedArticle.Description : article.Description;
            article.Body = !string.IsNullOrEmpty(request.UpdatedArticle.Body) ? request.UpdatedArticle.Body : article.Body;

            await _articleRepository.Update(article);
            article = await _articleRepository.GetById(article.Id, _userContext.UserId);
            
            var getProfileQueryResult = await _usersService.GetProfile(article.Author.Username);
            var authorProfile = getProfileQueryResult.Response.Profile;

            return OperationResponseFactory.Success(new EditArticleCommandResult
            {
                Article = article.ToArticleDTO(authorProfile)
            });
        }
    }
}
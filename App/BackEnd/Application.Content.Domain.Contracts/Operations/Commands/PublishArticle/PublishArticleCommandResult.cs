using Application.Content.Domain.Contracts.DTOs;
using Application.Core.DataAccess;
using MediatR;

namespace Application.Content.Domain.Contracts.Operations.Commands.PublishArticle
{
    public class PublishArticleCommandResult : ContractModel, INotification
    {
        public SingleArticleDTO Article { get; set; }
    }
}
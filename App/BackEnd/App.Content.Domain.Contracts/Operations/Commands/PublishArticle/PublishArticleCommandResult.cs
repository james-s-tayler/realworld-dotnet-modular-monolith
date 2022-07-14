using App.Content.Domain.Contracts.DTOs;
using App.Core.DataAccess;
using MediatR;

namespace App.Content.Domain.Contracts.Operations.Commands.PublishArticle
{
    public class PublishArticleCommandResult : ContractModel, INotification
    {
        public SingleArticleDTO Article { get; set; }
    }
}
using System.Diagnostics.CodeAnalysis;
using App.Content.Domain.Contracts.DTOs;
using App.Core.DataAccess;
using MediatR;

namespace App.Content.Domain.Contracts.Operations.Commands.PublishArticle
{
    [ExcludeFromCodeCoverage]
    public class PublishArticleCommandResult : ContractModel, INotification
    {
        public int ArticleId { get; set; }
        public int UserId { get; set; }
        public SingleArticleDTO Article { get; set; }
    }
}
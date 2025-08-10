using System.Diagnostics.CodeAnalysis;
using App.Core.DataAccess;
using MediatR;

namespace App.Content.Domain.Contracts.Operations.Commands.DeleteArticle
{
    [ExcludeFromCodeCoverage]
    public class DeleteArticleCommandResult : ContractModel, INotification
    {
        public int ArticleId { get; set; }
        public int UserId { get; set; }
    }
}
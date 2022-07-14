using App.Content.Domain.Contracts.DTOs;
using App.Core.DataAccess;
using MediatR;

namespace App.Content.Domain.Contracts.Operations.Commands.UnfavoriteArticle
{
    public class UnfavoriteArticleCommandResult : ContractModel, INotification
    {
        public SingleArticleDTO Article { get; set; }
    }
}
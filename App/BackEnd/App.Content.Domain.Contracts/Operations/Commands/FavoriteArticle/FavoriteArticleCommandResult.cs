using System.Diagnostics.CodeAnalysis;
using App.Content.Domain.Contracts.DTOs;
using App.Core.DataAccess;
using MediatR;

namespace App.Content.Domain.Contracts.Operations.Commands.FavoriteArticle
{
    [ExcludeFromCodeCoverage]
    public class FavoriteArticleCommandResult : ContractModel, INotification
    {
        public SingleArticleDTO Article { get; set; }
    }
}
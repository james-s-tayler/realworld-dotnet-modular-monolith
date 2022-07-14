using App.Content.Domain.Contracts.DTOs;
using App.Core.DataAccess;
using MediatR;

namespace App.Content.Domain.Contracts.Operations.Commands.EditArticle
{
    public class EditArticleCommandResult : ContractModel, INotification
    {
        public SingleArticleDTO Article { get; set; }
    }
}
using App.Core.DataAccess;
using MediatR;

namespace App.Content.Domain.Contracts.Operations.Commands.DeleteArticle
{
    public class DeleteArticleCommandResult : ContractModel, INotification {}
}
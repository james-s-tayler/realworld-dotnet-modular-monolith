using Application.Core.DataAccess;
using MediatR;

namespace Application.Content.Domain.Contracts.Operations.Commands.DeleteArticle
{
    public class DeleteArticleCommandResult : ContractModel, INotification {}
}
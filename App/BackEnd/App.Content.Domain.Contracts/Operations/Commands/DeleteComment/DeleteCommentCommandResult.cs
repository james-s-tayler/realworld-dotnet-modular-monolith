using App.Core.DataAccess;
using MediatR;

namespace App.Content.Domain.Contracts.Operations.Commands.DeleteComment
{
    public class DeleteCommentCommandResult : ContractModel, INotification
    {
    }
}
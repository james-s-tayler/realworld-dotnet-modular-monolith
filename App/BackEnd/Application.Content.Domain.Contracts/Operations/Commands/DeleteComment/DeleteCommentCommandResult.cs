using Application.Core.DataAccess;
using MediatR;

namespace Application.Content.Domain.Contracts.Operations.Commands.DeleteComment
{
    public class DeleteCommentCommandResult : ContractModel, INotification
    {
    }
}
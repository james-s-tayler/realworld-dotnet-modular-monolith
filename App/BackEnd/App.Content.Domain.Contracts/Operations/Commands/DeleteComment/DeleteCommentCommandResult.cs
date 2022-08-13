using System.Diagnostics.CodeAnalysis;
using App.Core.DataAccess;
using MediatR;

namespace App.Content.Domain.Contracts.Operations.Commands.DeleteComment
{
    [ExcludeFromCodeCoverage]
    public class DeleteCommentCommandResult : ContractModel, INotification
    {
    }
}
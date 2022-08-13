using System.Diagnostics.CodeAnalysis;
using App.Content.Domain.Contracts.DTOs;
using App.Core.DataAccess;
using MediatR;

namespace App.Content.Domain.Contracts.Operations.Commands.PostComment
{
    [ExcludeFromCodeCoverage]
    public class PostCommentCommandResult : ContractModel, INotification
    {
        public SingleCommentDTO Comment { get; set; }
    }
}
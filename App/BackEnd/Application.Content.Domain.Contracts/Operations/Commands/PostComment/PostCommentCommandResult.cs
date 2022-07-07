using Application.Content.Domain.Contracts.DTOs;
using Application.Core.DataAccess;
using MediatR;

namespace Application.Content.Domain.Contracts.Operations.Commands.PostComment
{
    public class PostCommentCommandResult : ContractModel, INotification
    {
        public SingleCommentDTO Comment { get; set; }
    }
}
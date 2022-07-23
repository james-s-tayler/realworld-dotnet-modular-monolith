using App.Core.DataAccess;
using App.Content.Domain.Contracts.DTOs;
using MediatR;

namespace App.Content.Domain.Contracts.Operations.Queries.GetArticleById
{
    public class GetArticleByIdQueryResult : ContractModel, INotification
    {
        public SingleArticleDTO Article { get; set; }
    }
}
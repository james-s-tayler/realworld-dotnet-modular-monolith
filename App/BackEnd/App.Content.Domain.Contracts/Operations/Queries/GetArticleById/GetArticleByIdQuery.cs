using System;
using System.ComponentModel.DataAnnotations;
using App.Core.DataAccess;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Content.Domain.Contracts.Operations.Queries.GetArticleById
{
    public class GetArticleByIdQuery : ContractModel, IRequest<OperationResponse<GetArticleByIdQueryResult>>
    {
        [Range(1,Int32.MaxValue)]
        public int ArticleId { get; set; }
    } 
}
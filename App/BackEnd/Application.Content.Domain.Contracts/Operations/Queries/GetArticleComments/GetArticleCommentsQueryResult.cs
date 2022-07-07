using System.Collections.Generic;
using Application.Content.Domain.Contracts.DTOs;
using Application.Core.DataAccess;

namespace Application.Content.Domain.Contracts.Operations.Queries.GetArticleComments
{
    public class GetArticleCommentsQueryResult : ContractModel
    {
        public List<SingleCommentDTO> Comments { get; set; }
    }
}
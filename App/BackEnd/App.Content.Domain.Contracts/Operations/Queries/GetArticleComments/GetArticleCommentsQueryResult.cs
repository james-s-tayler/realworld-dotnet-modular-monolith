using System.Collections.Generic;
using App.Content.Domain.Contracts.DTOs;
using App.Core.DataAccess;

namespace App.Content.Domain.Contracts.Operations.Queries.GetArticleComments
{
    public class GetArticleCommentsQueryResult : ContractModel
    {
        public List<SingleCommentDTO> Comments { get; set; }
    }
}
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using App.Content.Domain.Contracts.DTOs;
using App.Core.DataAccess;

namespace App.Content.Domain.Contracts.Operations.Queries.GetArticleComments
{
    [ExcludeFromCodeCoverage]
    public class GetArticleCommentsQueryResult : ContractModel
    {
        public List<SingleCommentDTO> Comments { get; set; }
    }
}
using Application.Core.DataAccess;

namespace Application.Content.Domain.Contracts.Operations.Queries.GetTags
{
    public class GetTagsQueryResult : ContractModel
    {
        public string[] Tags { get; set; }
    }
}
using App.Core.DataAccess;

namespace App.Content.Domain.Contracts.Operations.Queries.GetTags
{
    public class GetTagsQueryResult : ContractModel
    {
        public string[] Tags { get; set; }
    }
}
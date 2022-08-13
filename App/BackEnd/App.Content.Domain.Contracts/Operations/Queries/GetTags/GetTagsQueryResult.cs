using System.Diagnostics.CodeAnalysis;
using App.Core.DataAccess;

namespace App.Content.Domain.Contracts.Operations.Queries.GetTags
{
    [ExcludeFromCodeCoverage]
    public class GetTagsQueryResult : ContractModel
    {
        public string[] Tags { get; set; }
    }
}
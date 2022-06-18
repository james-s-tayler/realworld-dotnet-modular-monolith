using Application.Core.DataAccess;
using Application.Social.Domain.Contracts.DTOs;

namespace Application.Social.Domain.Contracts.Operations.Queries.GetProfile
{
    public class GetProfileQueryResult : ContractModel
    {
        public ProfileDTO Profile { get; set; }
    }
}
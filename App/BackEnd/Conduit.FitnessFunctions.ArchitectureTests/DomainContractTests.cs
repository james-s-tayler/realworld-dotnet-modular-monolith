using ArchUnitNET.xUnit;
using Conduit.Core.DataAccess;
using FluentAssertions;
using JetBrains.Annotations;
using MediatR;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Conduit.FitnessFunctions.ArchitectureTests
{
    [Collection(nameof(ArchitectureTestCollection))]
    public class DomainContractTests
    {
        private readonly ArchitectureTestSetupFixture _conduit;

        public DomainContractTests([NotNull] ArchitectureTestSetupFixture conduit)
        {
            _conduit = conduit;
        }

        [Fact]
        public void DomainContractsMustBePublic()
        {
            Classes().That().Are(_conduit.DomainContractClasses)
                .Should().BePublic()
                .Because("this is how other parts of the system interact with a given domain module")
                .Check(_conduit.Architecture);
        }
        
        [Fact]
        public void DomainContractsFollowNamingConvention()
        {
            Classes().That().Are(_conduit.DomainContractClasses)
                .Should().HaveNameEndingWith("Command")
                .OrShould().HaveNameEndingWith("Query")
                .OrShould().HaveNameEndingWith("Result")
                .OrShould().HaveNameEndingWith("DTO")
                .Because("semantics matter")
                .Check(_conduit.Architecture);
        }
        
        [Fact]
        public void DomainContractsMustSubclassContractModel()
        {
            Classes().That().Are(_conduit.DomainContractClasses)
                .Should().BeAssignableTo(typeof(ContractModel))
                .Because("Fody.Tracer [NoTrace] is applied to ContractModel.ToString() to reduce log noise")
                .Check(_conduit.Architecture);
        }
        
        [Fact]
        public void DomainOperationsResideInCorrectNamespace()
        {
            Classes().That().Are(_conduit.DomainOperations)
                .Should().ResideInNamespace(@".*Domain.Contracts.Commands.*|.*Domain.Contracts.Queries.*", true)
                .Because("Domain operations must indicate whether they mutate state or not")
                .Check(_conduit.Architecture);
        }
        
        [Fact]
        public void DomainOperationsFollowNamingConvention()
        {
            Classes().That().Are(_conduit.DomainOperations)
                .Should().HaveNameEndingWith("Command")
                .OrShould().HaveNameEndingWith("Query")
                .Because("Domain operations must indicate whether they mutate state or not")
                .Check(_conduit.Architecture);
        }
        
        [Fact]
        public void CommandsResideInCorrectNamespace()
        {
            Classes().That().Are(_conduit.Commands)
                .Should().ResideInNamespace(".*Domain.Contracts.Commands.*", true)
                .Because("Domain operations must indicate whether they mutate state or not")
                .Check(_conduit.Architecture);
        }
        
        [Fact]
        public void QueriesResideInCorrectNamespace()
        {
            Classes().That().Are(_conduit.Queries)
                .Should().ResideInNamespace(".*Domain.Contracts.Queries.*", true)
                .Because("Domain operations must indicate whether they mutate state or not")
                .Check(_conduit.Architecture);
        }
        
        /*
         implement rules to ensure:
         - operation name is prepended to query
         - operation name is in the namespace
         - IRequest is of Type OperationResponse<****Result>
         - Result lives next to request
         - contracts doesn't depend on domain and/or only depends on Core, 
         namespace Conduit.Users.Domain.Contracts.Queries.GetCurrentUser
        {
            public class GetCurrentUserQuery : ContractModel, IRequest<OperationResponse<GetCurrentUserQueryResult>> {}
        }*/
    }
}
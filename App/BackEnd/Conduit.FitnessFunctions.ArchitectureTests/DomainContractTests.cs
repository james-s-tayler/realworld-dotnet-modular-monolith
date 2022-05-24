using System.Linq;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.xUnit;
using Conduit.Core.DataAccess;
using Conduit.Core.PipelineBehaviors;
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
        
        [Fact]
        public void DomainOperationsReturnOperationResponseOfOperationNameResult()
        {
            Classes().That().Are(_conduit.DomainOperations)
                .Should().FollowCustomCondition(domainOperation =>
                {
                    var outerGenericParameter = domainOperation.Dependencies
                        .Where(d => d.GetType() == typeof(ImplementsInterfaceDependency))
                        .Where(d => d.Target.FullName == "MediatR.IRequest`1")
                        .Select(d => d.TargetGenericArguments.Single())
                        .Single();

                    var innerGenericParameter = outerGenericParameter.GenericArguments.Single();

                    var isOperationResponse = outerGenericParameter.Type.NameEndsWith("OperationResponse`1");
                    var isOperationNameResult = innerGenericParameter.Type.FullNameMatches($"{domainOperation.FullName}Result");
                    var pass = isOperationResponse && isOperationNameResult;
                    
                    return new ConditionResult(domainOperation, pass, "does not match");
                }, "implement IRequest<OperationResponse<${OperationName}Result>>")
                .Because("MediatR pipeline behaviors rely on this return type")
                .Check(_conduit.Architecture);
        }
        
        /*
         implement rules to ensure:
         - operation name is in the namespace
         - contracts doesn't depend on domain and/or only depends on Core, 
         namespace Conduit.Users.Domain.Contracts.Queries.GetCurrentUser
        {
            public class GetCurrentUserQuery : ContractModel, IRequest<OperationResponse<GetCurrentUserQueryResult>> {}
        }*/
    }
}
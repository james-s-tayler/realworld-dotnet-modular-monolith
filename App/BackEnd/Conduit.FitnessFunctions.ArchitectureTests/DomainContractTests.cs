using System.Linq;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.xUnit;
using Conduit.Core;
using Conduit.Core.DataAccess;
using JetBrains.Annotations;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Conduit.FitnessFunctions.ArchitectureTests
{
    [Collection(nameof(ArchitectureTestCollection))]
    public class DomainContractTests
    {
        private readonly ArchitectureTestSetupFixture _application;

        public DomainContractTests([NotNull] ArchitectureTestSetupFixture application)
        {
            _application = application;
        }

        [Fact]
        public void DomainContractsMustBePublic()
        {
            Classes().That().Are(_application.DomainContractClasses)
                .Should().BePublic()
                .Because("this is how other parts of the system interact with a given domain module")
                .Check(_application.Architecture);
        }
        
        [Fact]
        public void DomainContractsFollowNamingConvention()
        {
            Classes().That().Are(_application.DomainContractClasses)
                .Should().HaveNameEndingWith("Command")
                .OrShould().HaveNameEndingWith("Query")
                .OrShould().HaveNameEndingWith("Result")
                .OrShould().HaveNameEndingWith("DTO")
                .OrShould().HaveNameEndingWith("Enum")
                .OrShould().HaveNameEndingWith("DomainContracts")
                .Because("the semantics should indicate what the model is for")
                .Check(_application.Architecture);
        }
        
        [Fact]
        public void DomainContractsMustSubclassContractModel()
        {
            Classes().That().Are(_application.DomainContractClasses)
                .Should().BeAssignableTo(typeof(ContractModel))
                .Because("Fody.Tracer [NoTrace] is applied to ContractModel.ToString() to reduce log noise")
                .Check(_application.Architecture);
        }
        
        [Fact]
        public void DomainOperationsResideInCorrectNamespace()
        {
            Classes().That().Are(_application.DomainOperations)
                .Should().ResideInNamespace(@".*Domain.Contracts.Commands.*|.*Domain.Contracts.Queries.*", true)
                .Because("Domain operations must indicate whether they mutate state or not")
                .Check(_application.Architecture);
        }
        
        [Fact]
        public void DomainOperationsEndInEitherCommandOrQuery()
        {
            Classes().That().Are(_application.DomainOperations)
                .Should().HaveNameEndingWith("Command")
                .OrShould().HaveNameEndingWith("Query")
                .Because("Domain operations must indicate whether they mutate state or not")
                .Check(_application.Architecture);
        }
        
        [Fact]
        public void CommandsResideInCorrectNamespace()
        {
            Classes().That().Are(_application.Commands)
                .Should().FollowCustomCondition(command =>
                {
                    var operationName = command.Name.Replace("Command", "");
                    var pass = command.ResidesInNamespace($".*Domain.Contracts.Commands.{operationName}", true);
                    return new ConditionResult(command, pass, "does not match");
                }, "reside in Domain.Contracts.Commands.{OperationName}")
                .Because("this is the convention")
                .Check(_application.Architecture);
        }
        
        [Fact]
        public void QueriesResideInCorrectNamespace()
        {
            Classes().That().Are(_application.Queries)
                .Should().FollowCustomCondition(query =>
                {
                    var operationName = query.Name.Replace("Query", "");
                    var pass = query.ResidesInNamespace($".*Domain.Contracts.Queries.{operationName}", true);
                    return new ConditionResult(query, pass, "does not match");
                }, "reside in Domain.Contracts.Queries.{OperationName}")
                .Because("this is the convention")
                .Check(_application.Architecture);
        }
        
        [Fact]
        public void DomainOperationsReturnOperationResponseOfOperationNameResult()
        {
            Classes().That().Are(_application.DomainOperations)
                .Should().FollowCustomCondition(domainOperation =>
                {
                    var outerGenericParameter = domainOperation.Dependencies
                        .Where(d => d.GetType() == typeof(ImplementsInterfaceDependency))
                        .Where(d => d.Target.FullName == "MediatR.IRequest`1")
                        .Select(d => d.TargetGenericArguments.Single())
                        .Single();
                    var innerGenericParameter = outerGenericParameter.GenericArguments.Single();
                    var operationName = domainOperation.Name.Replace("Command", "").Replace("Query", "");
                    
                    var isOperationResponse = outerGenericParameter.Type.NameEndsWith("OperationResponse`1");
                    var isOperationNameResult = innerGenericParameter.Type.FullNameMatches($"{domainOperation.FullName}Result");
                    var isResultNextToOperation = innerGenericParameter.Type.ResidesInNamespace($".*Domain.Contracts.*.{operationName}", true);
                    
                    var pass = isOperationResponse && isOperationNameResult && isResultNextToOperation;
                    
                    return new ConditionResult(domainOperation, pass, "does not match");
                }, "implement IRequest<OperationResponse<${OperationName}Result>>")
                .Because("MediatR pipeline behaviors rely on this return type")
                .Check(_application.Architecture);
        }
        
        [Fact]
        public void DomainContractsShouldOnlyDependOnCore()
        {
            var coreNamespace = typeof(ConduitCore).Namespace;
            Classes().That().Are(_application.DomainContractClasses)
                .Should().OnlyDependOnTypesThat().ResideInNamespace($"{coreNamespace}|.*Domain.Contracts.*|System.*|MediatR.*|Destructurama.*", true)
                .Because("contracts shouldn't be responsible for any business logic")
                .Check(_application.Architecture);
        }
    }
}
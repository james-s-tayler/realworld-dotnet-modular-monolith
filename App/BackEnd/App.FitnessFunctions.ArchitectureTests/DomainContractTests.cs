using System.Diagnostics.CodeAnalysis;
using System.Linq;
using App.Core;
using App.Core.DataAccess;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.xUnit;
using MediatR;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace App.FitnessFunctions.ArchitectureTests
{
    [Collection(nameof(ArchitectureTestCollection))]
    public class DomainContractTests
    {
        private readonly ArchitectureTestSetupFixture _application;

        public DomainContractTests([JetBrains.Annotations.NotNull] ArchitectureTestSetupFixture application)
        {
            _application = application;
        }

        [Fact]
        public void DomainContractsMustBePublic()
        {
            Classes().That().Are(_application.DomainContracts)
                .Should().BePublic()
                .Because("this is how other parts of the system interact with a given domain module")
                .Check(_application.Architecture);
        }

        [Fact]
        public void DomainContractsFollowNamingConvention()
        {
            Classes().That().Are(_application.DomainContracts)
                .Should().HaveNameEndingWith("Command")
                .OrShould().HaveNameEndingWith("Query")
                .OrShould().HaveNameEndingWith("CommandResult")
                .OrShould().HaveNameEndingWith("QueryResult")
                .OrShould().HaveNameEndingWith("DTO")
                .OrShould().HaveNameEndingWith("Enum")
                .OrShould().HaveNameEndingWith("DomainContracts")
                .Because("the semantics should indicate what the model is for")
                .Check(_application.Architecture);
        }

        [Fact]
        public void DomainContractsMustBeExcludedFromCodeCoverage()
        {
            Classes().That().Are(_application.DomainContracts)
                .Should().HaveAnyAttributes(typeof(ExcludeFromCodeCoverageAttribute))
                .Because("This makes coverage of actual business logic more accurate")
                .Check(_application.Architecture);
        }

        [Fact]
        public void DomainContractsMustSubclassContractModel()
        {
            Classes().That().Are(_application.DomainContracts)
                .Should().BeAssignableTo(typeof(ContractModel))
                .Because("Fody.Tracer [NoTrace] is applied to ContractModel.ToString() to reduce log noise")
                .Check(_application.Architecture);
        }

        [Fact]
        public void CommandResultsMustImplementINotification()
        {
            Classes().That().Are(_application.DomainContracts)
                .And().HaveNameEndingWith("CommandResult")
                .Should().BeAssignableTo(typeof(INotification))
                .Because("this is how events are published for other modules to consume")
                .Check(_application.Architecture);
        }

        [Fact]
        public void DomainOperationsResideInCorrectNamespace()
        {
            Classes().That().Are(_application.DomainOperations)
                .Should().ResideInNamespaceMatching(@".*Domain.Contracts.Operations.Commands.*|.*Domain.Contracts.Operations.Queries.*")
                .Because("Domain operations must indicate whether they mutate state or not")
                .Check(_application.Architecture);
        }

        [Fact]
        public void DTOsResideInCorrectNamespace()
        {
            Classes().That().Are(_application.DomainContracts)
                .And().HaveNameEndingWith("DTO")
                .Should().ResideInNamespaceMatching(@".*Domain.Contracts.DTOs")
                .Because("this is the convention")
                .Check(_application.Architecture);
        }

        [Fact]
        public void EnumsResideInCorrectNamespace()
        {
            Classes().That().Are(_application.DomainContracts)
                .And().HaveNameEndingWith("Enum")
                .Should().ResideInNamespaceMatching(@".*Domain.Contracts.Enums")
                .Because("this is the convention")
                .WithoutRequiringPositiveResults()
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
                    var pass = command.ResidesInNamespaceMatching($".*Domain.Contracts.Operations.Commands.{operationName}");
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
                    var pass = query.ResidesInNamespaceMatching($".*Domain.Contracts.Operations.Queries.{operationName}");
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
                    var isResultNextToOperation = innerGenericParameter.Type.ResidesInNamespaceMatching($".*Domain.Contracts.*.{operationName}");

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
            Classes().That().Are(_application.DomainContracts)
                .Should().OnlyDependOnTypesThat().ResideInNamespaceMatching($"{coreNamespace}|" +
                    ".*Domain.Contracts.*|" +
                    "System.*|" +
                    "MediatR.*|" +
                    "Destructurama.*")
                .Because("contracts shouldn't be responsible for any business logic")
                .Check(_application.Architecture);
        }
    }
}
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Application.FitnessFunctions.ArchitectureTests
{
    [Collection(nameof(ArchitectureTestCollection))]
    public class DomainTests
    {
        private readonly ArchitectureTestSetupFixture _application;

        public DomainTests(ArchitectureTestSetupFixture application)
        {
            _application = application;
        }

        [Fact]
        public void DomainClassesShouldBeInternal()
        {
            Classes().That().Are(_application.DomainClasses)
                .Should()
                .BeInternal()
                .Because("only the contracts of the domain should be public")
                .Check(_application.Architecture);
        }
        
        [Fact]
        public void DomainOperationHandlersEndInEitherCommandHandlerOrQueryHandler()
        {
            Classes().That().Are(_application.DomainOperationHandlers)
                .Should().HaveNameEndingWith("CommandHandler")
                .OrShould().HaveNameEndingWith("QueryHandler")
                .Because("Domain operations must indicate whether they mutate state or not")
                .Check(_application.Architecture);
        }
        
        [Fact]
        public void CommandHandlersResideInCorrectNamespace()
        {
            Classes().That().Are(_application.CommandHandlers)
                .Should().FollowCustomCondition(commandHandler =>
                {
                    var operationName = commandHandler.Name.Replace("CommandHandler", "");
                    var pass = commandHandler.ResidesInNamespace($".*Domain.Operations.Commands.{operationName}", true);
                    return new ConditionResult(commandHandler, pass, "does not match");
                }, "reside in Domain.Operations.Commands.{OperationName}")
                .Because("this is the convention")
                .Check(_application.Architecture);
        }
        
        [Fact]
        public void QueryHandlersResideInCorrectNamespace()
        {
            Classes().That().Are(_application.QueryHandlers)
                .Should().FollowCustomCondition(query =>
                {
                    var operationName = query.Name.Replace("QueryHandler", "");
                    var pass = query.ResidesInNamespace($".*Domain.Operations.Queries.{operationName}", true);
                    return new ConditionResult(query, pass, "does not match");
                }, "reside in Domain.Operations.Queries.{OperationName}")
                .Because("this is the convention")
                .Check(_application.Architecture);
        }
    }
}
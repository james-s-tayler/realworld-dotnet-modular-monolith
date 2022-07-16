using System.Data.Common;
using System.Security.Claims;
using App.Core.Context;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.xUnit;
using Microsoft.AspNetCore.Http;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace App.FitnessFunctions.ArchitectureTests
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
        public void EntitiesShouldFollowNamingConvention()
        {
            Classes().That().Are(_application.Entities)
                .Should()
                .HaveNameEndingWith("Entity")
                .Because("this avoids compilation failures arising from having a property with the same name as the class")
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
        
        [Fact]
        public void DomainModulesShouldResideInCorrectNamespace()
        {
            Classes().That().Are(_application.DomainModules)
                .Should()
                .ResideInNamespace(".*.Domain.Setup.Module", true)
                .Because("that's the convention")
                .Check(_application.Architecture);
        }
        
        [Fact]
        public void DomainClassesShouldNotDirectlyReferenceHttpContext()
        {
            Classes().That().Are(_application.DomainClasses)
                .Should()
                .NotDependOnAny(typeof(IHttpContextAccessor))
                .Because("that should be handled by abstractions in App.Core")
                .Check(_application.Architecture);
        }
        
        [Fact]
        public void DomainClassesShouldNotDirectlyReferenceClaimsPrincipal()
        {
            Classes().That().Are(_application.DomainClasses)
                .Should()
                .NotDependOnAny(typeof(ClaimsPrincipal))
                .Because("that should be handled by abstractions in App.Core. This is a notoriously tricky class to instantiate correctly. See: https://www.benday.com/2021/08/13/3-common-problems-with-claimsidentity-and-claimsprincipal-in-asp-net-core/")
                .Check(_application.Architecture);
        }
        
        [Fact]
        public void RepositoriesShouldNotDirectlyReferenceUserContext()
        {
            Classes().That().Are(_application.DomainClasses)
                .And().DependOnAny(typeof(DbConnection))
                .Should()
                .NotDependOnAny(typeof(IUserContext))
                .Because("repositories should be passed all the data they are expected to persist/query. This makes testing more flexible and requires less mocking when you want to test behavior that differs between authenticated and unauthenticated users.")
                .Check(_application.Architecture);
        }
    }
}
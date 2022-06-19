using System.Linq;
using System.Reflection;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Classes;
using FluentAssertions;
using Xunit;

namespace Application.FitnessFunctions.ArchitectureTests
{
    [Collection(nameof(ArchitectureTestCollection))]
    public class SanityChecks
    {
        private readonly ArchitectureTestSetupFixture _application;

        public SanityChecks(ArchitectureTestSetupFixture application)
        {
            _application = application;
        }

        [Fact]
        public void ClassesAreBeingEnumerated()
        {
            var fixtureType = _application.GetType();
            var classProperties = fixtureType.GetProperties().Where(property => property.PropertyType == typeof(IObjectProvider<Class>));

            foreach (var classProperty in classProperties)
            {
                var classProvider = classProperty.GetMethod.Invoke(_application, null) as GivenClassesConjunctionWithDescription;
                
                //assert
                Assert.True(classProvider != null, $"{classProperty.Name} should not be null");
                classProvider.GetObjects(_application.Architecture)
                    .Should()
                    .NotBeEmpty($"{classProvider.Description} should be being enumerated correctly but none were found");
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Conduit.API.Tests.Integration
{
    [CollectionDefinition(nameof(IntegrationTestCollection))]
    public class IntegrationTestCollection : ICollectionFixture<WebApplicationFactory<Program>> { }
}
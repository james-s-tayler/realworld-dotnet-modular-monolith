using App.Feed.Domain.Contracts.DTOs;
using App.Feed.Domain.Entities;

namespace App.Feed.Domain.Infrastructure.Mappers
{
    internal static class ExampleMapper
    {
        internal static ExampleDTO ToExampleDTO(this ExampleEntity exampleEntity)
        {
            return new ExampleDTO
            {
                Id = exampleEntity.Id,
                SensitiveValue = exampleEntity.Something
            };
        }
    }
}
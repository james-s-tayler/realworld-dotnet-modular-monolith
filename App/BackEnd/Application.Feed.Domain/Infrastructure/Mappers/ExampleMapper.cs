using Application.Feed.Domain.Contracts.DTOs;
using Application.Feed.Domain.Entities;

namespace Application.Feed.Domain.Infrastructure.Mappers
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
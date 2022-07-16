using App.ModuleName.Domain.Contracts.DTOs;
using Application.ModuleName.Domain.Entities;

namespace Application.ModuleName.Domain.Infrastructure.Mappers
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
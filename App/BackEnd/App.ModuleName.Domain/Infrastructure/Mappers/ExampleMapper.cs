using App.ModuleName.Domain.Contracts.DTOs;
using App.ModuleName.Domain.Entities;

namespace App.ModuleName.Domain.Infrastructure.Mappers
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
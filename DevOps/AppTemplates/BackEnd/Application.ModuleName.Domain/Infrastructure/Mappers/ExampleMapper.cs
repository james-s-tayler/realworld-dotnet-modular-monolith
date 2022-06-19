using Application.ModuleName.Domain.Contracts.DTOs;
using Application.ModuleName.Domain.Entities;

namespace Application.ModuleName.Domain.Infrastructure.Mappers
{
    internal static class ExampleMapper
    {
        internal static ExampleDTO ToExampleDTO(this Example example)
        {
            return new ExampleDTO
            {
                Id = example.Id,
                SensitiveValue = example.Something
            };
        }
    }
}
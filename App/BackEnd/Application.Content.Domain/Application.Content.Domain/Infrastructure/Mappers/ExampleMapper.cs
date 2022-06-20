using Application.Content.Domain.Contracts.DTOs;
using Application.Content.Domain.Entities;

namespace Application.Content.Domain.Infrastructure.Mappers
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
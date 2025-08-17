using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using App.Core.DataAccess;

namespace App.Content.Domain.Contracts.DTOs
{
    [ExcludeFromCodeCoverage]
    public class EditArticleDTO : ContractModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }

        public string GetSlug()
        {
            return Title?.Replace(" ", "-").ToLowerInvariant() ?? string.Empty;
        }
    }
}
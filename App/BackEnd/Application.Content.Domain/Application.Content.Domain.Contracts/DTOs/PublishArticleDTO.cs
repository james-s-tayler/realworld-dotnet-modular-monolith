using System.ComponentModel.DataAnnotations;
using Application.Core.DataAccess;

namespace Application.Content.Domain.Contracts.DTOs
{
    public class PublishArticleDTO : ContractModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Body { get; set; }
        
        public string[] TagList { get; set; }

        public string GetSlug()
        {
            return Title.Replace(" ", "-").ToLowerInvariant();
        }
    }
}
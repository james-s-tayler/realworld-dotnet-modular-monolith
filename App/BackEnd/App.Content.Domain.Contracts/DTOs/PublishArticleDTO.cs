using System;
using System.ComponentModel.DataAnnotations;
using App.Core.DataAccess;

namespace App.Content.Domain.Contracts.DTOs
{
    public class PublishArticleDTO : ContractModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Body { get; set; }

        public string[] TagList { get; set; } = Array.Empty<string>();

        public string GetSlug()
        {
            return Title.Replace(" ", "-").ToLowerInvariant();
        }
    }
}
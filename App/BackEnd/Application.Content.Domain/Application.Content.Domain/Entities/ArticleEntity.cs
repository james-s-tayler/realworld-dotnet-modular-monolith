using System;
using System.Collections.Generic;

namespace Application.Content.Domain.Entities
{
    internal class ArticleEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public List<TagEntity> TagList { get; set; } = new ();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string GetSlug()
        {
            return Title.Replace(" ", "-").ToLowerInvariant();
        }
    }
}
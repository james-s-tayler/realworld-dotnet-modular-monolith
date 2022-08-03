using System;
using System.Collections.Generic;

namespace App.Content.Domain.Entities
{
    internal class ArticleEntity
    {
        public int Id { get; set; }
        public UserEntity Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public List<TagEntity> TagList { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Favorited { get; set; }
        public int FavoritesCount { get; set; }

        public string GetSlug()
        {
            return Title.Replace(" ", "-").ToLowerInvariant();
        }
    }
}
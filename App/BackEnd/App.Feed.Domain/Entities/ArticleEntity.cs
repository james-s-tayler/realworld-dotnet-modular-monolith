using System;

namespace App.Feed.Domain.Entities
{
    internal class ArticleEntity
    {
        public int ArticleId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
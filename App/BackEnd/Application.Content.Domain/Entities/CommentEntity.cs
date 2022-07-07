using System;

namespace Application.Content.Domain.Entities
{
    internal class CommentEntity
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public UserEntity Author { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
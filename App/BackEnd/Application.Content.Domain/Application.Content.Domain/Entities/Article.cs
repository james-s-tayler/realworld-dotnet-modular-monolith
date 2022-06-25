using System;

namespace Application.Content.Domain.Entities
{
    internal class Article
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public string[] TagList { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }

        public string GetSlug()
        {
            return Title.Replace(" ", "-").ToLowerInvariant();
        }
    }
}
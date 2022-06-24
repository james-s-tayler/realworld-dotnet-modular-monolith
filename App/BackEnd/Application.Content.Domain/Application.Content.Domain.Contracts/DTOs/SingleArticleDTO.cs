using System;
using Application.Core.DataAccess;
using Application.Social.Domain.Contracts.DTOs;
using Destructurama.Attributed;

namespace Application.Content.Domain.Contracts.DTOs
{
    public class SingleArticleDTO : ContractModel
    {
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public string[] TagList { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Favorited { get; set; }
        public int FavoritesCount { get; set; }
        public ProfileDTO Author { get; set; }
    }
}
using System;
using Application.Core.DataAccess;
using Application.Social.Domain.Contracts.DTOs;

namespace Application.Content.Domain.Contracts.DTOs
{
    public class SingleCommentDTO : ContractModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Body { get; set; }
        public ProfileDTO Author { get; set; }
    }
}
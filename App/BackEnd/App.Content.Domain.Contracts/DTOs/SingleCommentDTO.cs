using System;
using App.Core.DataAccess;
using App.Social.Domain.Contracts.DTOs;

namespace App.Content.Domain.Contracts.DTOs
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
using System.ComponentModel.DataAnnotations;
using Application.Core.DataAccess;

namespace Application.Content.Domain.Contracts.DTOs
{
    public class PostCommentDTO : ContractModel
    {
        [Required]
        public string Body { get; set; }
    }
}
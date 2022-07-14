using System.ComponentModel.DataAnnotations;
using App.Core.DataAccess;

namespace App.Content.Domain.Contracts.DTOs
{
    public class PostCommentDTO : ContractModel
    {
        [Required]
        public string Body { get; set; }
    }
}
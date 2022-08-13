using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using App.Core.DataAccess;

namespace App.Content.Domain.Contracts.DTOs
{
    [ExcludeFromCodeCoverage]
    public class PostCommentDTO : ContractModel
    {
        [Required]
        public string Body { get; set; }
    }
}
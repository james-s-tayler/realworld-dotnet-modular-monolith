using App.Core.DataAccess;

namespace App.Content.Domain.Contracts.DTOs
{
    public class EditArticleDTO : ContractModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
    }
}
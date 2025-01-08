using CVManager.DAL.Entities;

namespace CVManager.WebApplication.Models
{
    public class CVViewModel
    {
        public string CVId { get; set; }
        public string? ProfilePicturePath { get; set; }

        public string Summary { get; set; }

        public string UserId { get; set; }
    }
}

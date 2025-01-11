using System.ComponentModel.DataAnnotations;

namespace CVManager.WebApplication.Models
{
    public class ChangeProjectViewModel
    {
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Du måste ha ett projektnamn.")]
        [StringLength(100, ErrorMessage = "Projektnamnet får vara högst 100 tecken.")]
        public string ProjectName { get; set; }
        [StringLength(500, ErrorMessage = "Projektbeskrivning får vara högst 500 tecken.")]
        public string ProjectDescription { get; set; }

    }
}

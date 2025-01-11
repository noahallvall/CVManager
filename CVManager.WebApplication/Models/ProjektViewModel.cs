using System.ComponentModel.DataAnnotations;

namespace CVManager.WebApplication.Models
{
    public class ProjektViewModel
    {
        [Required(ErrorMessage = "Vänligen ange projektnamn.")]
        [StringLength(255, ErrorMessage = "Projektnamnet får inte vara längre än 255 tecken.")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "Vänligen ange projektbeskrivning.")]
        [StringLength(1000, ErrorMessage = "Projektbeskrivningen får inte vara längre än 1000 tecken.")]
        public string ProjectDescription { get; set; }
    }
}

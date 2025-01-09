using System.ComponentModel.DataAnnotations;

namespace CVManager.WebApplication.Models
{
    public class CVAltViewModel
    {
        [Required(ErrorMessage = "Vänligen skriv en text om ditt cv.")]
        public string Summary { get; set; }

        public string? ProfilePicturePath { get; set; }

    }
}

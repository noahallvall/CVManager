using System.ComponentModel.DataAnnotations;

namespace CVManager.WebApplication.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vänligen skriv ett användarnamn.")]
        [StringLength(255)]

        public string AnvandarNamn { get; set; }

        [Required(ErrorMessage = "Vänligen skriv ett lösenord.")]
        [DataType(DataType.Password)]

        public string Losenord { get; set; }

        public bool RememberMe { get; set; }
    }
}

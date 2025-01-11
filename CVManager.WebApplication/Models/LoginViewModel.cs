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
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Lösenordet måste vara mellan 6 och 100 tecken.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
         ErrorMessage = "Lösenordet måste innehålla minst en stor bokstav, en liten bokstav, en siffra och ett specialtecken.")]

        public string Losenord { get; set; }

        public bool RememberMe { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace CVManager.WebApplication.Models
{
    public class UserRegisterViewModel
    {
        [Required(ErrorMessage = "Vänligen skriv ett användarnamn.")]
        [StringLength(255, ErrorMessage = "Användarnamnet får inte vara längre än 255 tecken.")]
        [RegularExpression(@"^[a-zA-Z0-9]+$",
            ErrorMessage = "Användarnamnet får endast innehålla bokstäver (a-z, A-Z) och siffror (0-9). Inga specialtecken som å, ä, ö eller mellanslag är tillåtna.")]



        public string AnvandarNamn { get; set; }

        [Required(ErrorMessage = "Vänligen skriv ett lösenord.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Lösenordet måste vara mellan 6 och 100 tecken.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
            ErrorMessage = "Lösenordet måste innehålla minst en stor bokstav, en liten bokstav, en siffra och ett specialtecken.")]
        public string Losenord { get; set; }

        [DataType(DataType.Password)]
        [Compare("Losenord", ErrorMessage = "Lösenorden matchar inte.")]
        [Display(Name = "Bekräfta lösenordet")]
        public string BekraftaLosenord { get; set; }

        public bool IsPrivateProfile { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace CVManager.WebApplication.Models
{
    public class UserRegisterViewModel
    {
        [Required(ErrorMessage = "Vänligen skriv ett användarnamn.")]
        [StringLength(255, ErrorMessage = "Användarnamnet får inte vara längre än 255 tecken.")]
        public string AnvandarNamn { get; set; }

        [Required(ErrorMessage = "Vänligen skriv ett lösenord.")]
        [DataType(DataType.Password)]
        public string Losenord { get; set; }

        [DataType(DataType.Password)]
        [Compare("Losenord", ErrorMessage = "Lösenorden matchar inte.")]
        [Display(Name = "Bekräfta lösenordet")]
        public string BekraftaLosenord { get; set; }
        public bool IsPrivateProfile { get; set; }


    }
}

using System.ComponentModel.DataAnnotations;

namespace CVManager.WebApplication.Models
{
    public class UserRegisterViewModel
    {
        [Required(ErrorMessage = "Vänligen skriv ett användarnamn.")]
        [StringLength(255)]

        public string AnvandarNamn { get; set; }

        [Required(ErrorMessage = "Vänligen skriv ett lösenord.")]
        [DataType(DataType.Password)]
        [Compare("BekraftaLosenord")]

        public string Losenord { get; set; }

        [Required(ErrorMessage = "Vänligen bekräfta lösenordet")]
        [DataType (DataType.Password)]
        [Display(Name = "Bekrafta losenordet")]

        public string BekraftaLosenord  { get; set; }
    }
}

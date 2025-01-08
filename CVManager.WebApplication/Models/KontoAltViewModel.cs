using System.ComponentModel.DataAnnotations;

namespace CVManager.WebApplication.Models
{
    public class KontoAltViewModel
    {
        [StringLength(255)]

        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public string Address { get; set; }
        public string Email { get; set; }

        public string Phone { get; set; }

        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]

        public string? Losenord { get; set; }

        public bool IsPrivateProfile { get; set; }
    }
}

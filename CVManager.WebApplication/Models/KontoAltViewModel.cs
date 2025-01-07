using System.ComponentModel.DataAnnotations;

namespace CVManager.WebApplication.Models
{
    public class KontoAltViewModel
    {
        [StringLength(255)]

        public string FirstName { get; set; }

        [DataType(DataType.Password)]

        public string Losenord { get; set; }

        public bool IsPrivateProfile { get; set; }
    }
}

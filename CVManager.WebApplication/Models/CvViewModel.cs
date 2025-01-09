using CVManager.DAL.Entities;

namespace CVManager.WebApplication.Models
{
    public class CvViewModel
    {

        public User User { get; set; } // Användarens information
        public CV CV { get; set; } // Användarens enda CV


    }
}

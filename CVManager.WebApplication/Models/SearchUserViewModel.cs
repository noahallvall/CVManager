using CVManager.DAL.Entities;

namespace CVManager.WebApplication.Models
{
    public class SearchUserViewModel
    {
      
        public string FullName { get; set; } 
        public bool IsPrivateProfile { get; set; }

        public string PhoneNumber { get; set; } // Phone number property
        public string Email { get; set; } // Email property
        public string Address { get; set; }
        public CV CV { get; set; } 


    }
}

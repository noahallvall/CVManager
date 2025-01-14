using CVManager.DAL.Entities;

namespace CVManager.WebApplication.Models
{
    public class SearchUserViewModel
    {
      
        public string FullName { get; set; } 
        public bool IsPrivateProfile { get; set; } 
        public CV CV { get; set; } 


    }
}

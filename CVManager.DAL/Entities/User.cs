using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CVManager.DAL.Entities
{
    public class User : IdentityUser
    {
        public User() 
        {
            FirstName = "Pepega";
            LastName = "Pepega";
            Address = "Pepegatan 13";
            Email = "Gooner@gmail.com";
            Phone = "1234567890";
        }
        public int Id { get; set; }

        public CV? CV { get; set; }

        public bool IsPrivateProfile {  get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

    }
}

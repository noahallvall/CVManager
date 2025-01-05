using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CVManager.DAL.Entities
{
    public class User : IdentityUser
    {
        //public int UserID { get; set; }

        public string FirstName { get; set; }    

        public bool IsPrivateProfile { get; set; }

        //Foreign key
        public int CVId { get; set; }
    }
}

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

        public int Id { get; set; }

        public CV? CV { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVManager.DAL.Entities
{
    public class Experience
    {
        public int ExperienceId { get; set; }

        public string CompanyName { get; set; }

        public string Role { get; set; }   



        // Foreign Key to CV
        public int CVId { get; set; }
    }
}

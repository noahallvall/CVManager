using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVManager.DAL.Entities
{
    public class Education
    {
        public int EducationId { get; set; }


        // Foreign Key to CV
        public int CVId { get; set; }
    }
}

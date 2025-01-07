using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace CVManager.DAL.Entities
{
   public class CV
    {
        public int CVId { get; set; }
        public string ProfilePicturePath { get; set; }
        public string Summary { get; set; }
        public string ContactInformation { get; set; }


        public virtual ICollection<Skill> Skills { get; set; }
        public virtual ICollection<Experience> Experiences { get; set; }
        public virtual ICollection<Education> Educations { get; set; }

        public User User { get; set; } = null!;
        
        //För att kunna navigera många till många förhållandet. cv - cvproject - project. 
        public virtual ICollection<CVProject> CVProjects { get; set; }

    }
}

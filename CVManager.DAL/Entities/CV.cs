using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CVManager.DAL.Entities
{
   public class CV
    {
        public int CVId { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public string Summary { get; set; }
        public string UserId { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }
        public virtual ICollection<Experience> Experiences { get; set; }
        public virtual ICollection<Education> Educations { get; set; }
        public User User { get; set; } = null!;       
        public virtual ICollection<CVProject> CVProjects { get; set; }
        public virtual ICollection<Message> Messages { get; set; }

    }
}

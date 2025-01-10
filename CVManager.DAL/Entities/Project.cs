using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVManager.DAL.Entities
{
    public class Project
    {
        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string ProjectDescription { get; set; }

        public DateTime? UploadDate { get; set; }

        public string ownerId { get; set; }

        public virtual User User { get; set; }

        //För att kunna navigera många till många förhållandet. cv - cvproject - project. 
        public virtual ICollection<CVProject>? CVProjects { get; set; }
    }
}

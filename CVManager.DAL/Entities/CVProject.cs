using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVManager.DAL.Entities
{
    public class CVProject
    {

        //Foreign keys, sambandstabell
        public int CVId { get; set; }
        public CV CV { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}

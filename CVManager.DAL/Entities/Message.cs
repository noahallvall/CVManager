using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVManager.DAL.Entities
{
    public class Message
    {
        public int MessageId { get; set; }



        // Foreign Key to CV
        public int CVId { get; set; }
    }
}

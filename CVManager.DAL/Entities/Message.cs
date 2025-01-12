using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVManager.DAL.Entities
{
    public class Message
    {
        public string MessageId { get; set; }

        public string MessageContent { get; set; }

        // Foreign Key to CV
        public int? CVSentId { get; set; }
        public int CVRecievedId { get; set; }

        public bool IsRead  { get; set; }

        public string? SendersName { get; set; }

        public CV CV { get; set; }
    }
}

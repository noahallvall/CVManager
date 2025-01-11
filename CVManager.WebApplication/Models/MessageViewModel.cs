using System.ComponentModel.DataAnnotations;

namespace CVManager.WebApplication.Models
{
    public class MessageViewModel
    {
        public string MessageId { get; set; }

        [Required(ErrorMessage = "Meddelandet får inte vara tomt.")]
        [StringLength(500, ErrorMessage = "Meddelandet får vara högst 500 tecken.")]
        public string MessageContent { get; set; }
        public int CVSentId { get; set; }
        public int CVRecievedId { get; set; }
        public bool IsRead { get; set; }
        public string Sender { get; set; }
        public string Reciever { get; set; }
    }
}

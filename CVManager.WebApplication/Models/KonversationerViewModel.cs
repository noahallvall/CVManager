using CVManager.DAL.Entities;

namespace CVManager.WebApplication.Models
{
    public class KonversationerViewModel
    {
        public List<Message>? RecievedMessages { get; set; }
        public List<Message>? SentMessages { get; set; }
    }
}

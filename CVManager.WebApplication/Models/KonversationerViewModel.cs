using CVManager.DAL.Entities;

namespace CVManager.WebApplication.Models
{
    public class KonversationerViewModel
    {
        public List<Message>? RecievedMessages { get; set; }
        public List<Message>? SentMessages { get; set; }
        public List<User>? Users { get; set; }
        public List<CV>? CVS { get; set; }

    }
}

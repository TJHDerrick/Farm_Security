using Fresh_Farm_Market_214142Z.Model;

namespace Fresh_Farm_Market_214142Z.Services
{
    public interface IEmailService
    {
        public void SendEmail(Message message);
    }
}

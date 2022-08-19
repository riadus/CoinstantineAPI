using System.Threading.Tasks;

namespace CoinstantineAPI.Email
{
    public interface IEmailSender
    {
        Task SendEmail(EmailObject emailObject);
    }
}

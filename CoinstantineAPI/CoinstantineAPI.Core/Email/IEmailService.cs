using System.Threading.Tasks;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Core.Email
{
    public interface IEmailService
    {
        Task SendConfirmationEmail(UserIdentity userIdentity);
        Task SendResetPassword(UserIdentity userIdentity);
        Task SendUsername(UserIdentity userIdentity);
    }
}

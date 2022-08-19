using System.Threading.Tasks;
using CoinstantineAPI.Core;
using CoinstantineAPI.Core.Email;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Email
{
    public class EmailService : IEmailService
    {
        private readonly IEmailSender _emailSender;
        private readonly string BaseUrl = Constants.WebsiteUrl;

        public EmailService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public Task SendConfirmationEmail(UserIdentity recipient)
        {
            var emailObject = new EmailObject
            {
                Recipient = recipient,
                TemplateId = Constants.AccountCreationTemplate,
                Template = new
                {
                    recipient.FirstName,
                    Url = $"{BaseUrl}/account/confirmation/{recipient.UserId}?confirmationCode={recipient.ConfirmationCode}",
                    recipient.ConfirmationCode,
                    recipient.UserId
                }
            };

            return _emailSender.SendEmail(emailObject);
        }

        public Task SendResetPassword(UserIdentity recipient)
        {
            var emailObject = new EmailObject
            {
                Recipient = recipient,
                TemplateId = Constants.ResetPasswordTemplate,
                Template = new
                {
                    recipient.FirstName,
                    Url = $"{BaseUrl}/account/reset/{recipient.UserId}?confirmationCode={recipient.ConfirmationCode}"
                }
            };
            return _emailSender.SendEmail(emailObject);
        }

        public Task SendUsername(UserIdentity recipient)
        {
            var emailObject = new EmailObject
            {
                Recipient = recipient,
                TemplateId = Constants.SendUsernameTemplate,
                Template = new
                {
                    recipient.FirstName,
                    recipient.Username,
                    ConnectionUrl = $"{BaseUrl}/account/login"
                }
            };
            return _emailSender.SendEmail(emailObject);
        }
    }
}

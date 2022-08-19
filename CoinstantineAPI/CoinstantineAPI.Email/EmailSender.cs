using System.Collections.Generic;
using System.Threading.Tasks;
using CoinstantineAPI.Core;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CoinstantineAPI.Email
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmail(EmailObject emailObject)
        {
            var client = new SendGridClient(Constants.SendGridApiKey);
            var msg = new SendGridMessage
            {
                From = new EmailAddress("noreply@coinstantine.com", "Coinstantine")
            };
            msg.TemplateId = emailObject.TemplateId;
            msg.Personalizations = new List<Personalization>
            {
                new Personalization
                    {
                        Tos = new List<EmailAddress>
                        {
                            new EmailAddress{ Email = emailObject.Recipient.EmailAddress, Name = emailObject.Recipient.FirstName}
                        },
                        TemplateData = emailObject.Template
                    }
                };

            await client.SendEmailAsync(msg);
        }
    }
}

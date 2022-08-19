using System;
using System.Threading.Tasks;

namespace CoinstantineAPI.Core.Services
{
    public interface INotificationCenter
    {
		Task SendNotification(string message, string email);
		Task SendSilentNotification(string email, string translationKey, PartToUpdate partToUpdate);
    }
}

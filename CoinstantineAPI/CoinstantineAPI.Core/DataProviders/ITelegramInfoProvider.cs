using System.Threading.Tasks;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Core.DataProvider
{
    public interface ITelegramInfoProvider
    {
        Task ProcessConversationOnTelegram(string username);
        (TelegramProfile TelegramProfile, string Phonenumber) GetTelegramProfile(string username, bool dispose = false);
    }
}

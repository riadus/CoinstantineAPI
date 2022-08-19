using System;
using System.Linq;
using System.Threading.Tasks;
using CoinstantineAPI.TelegramProvider.Entities;
using Telegram.Bot.Types;

namespace CoinstantineAPI.TelegramProvider
{
    public interface ITelegramBotManager
    {
        Task HandleUpdate(AppUpdate update);
        Task StartListening();
        Task StartListeningForUser(string username, Action<AppUpdate> callback);
    }
}
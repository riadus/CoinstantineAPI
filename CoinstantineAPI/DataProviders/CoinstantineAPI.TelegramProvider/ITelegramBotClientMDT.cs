using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace CoinstantineAPI.DataProvider.TelegramProvider
{
    public interface ITelegramBotClientMDT
    {
        Task CleanUpdates(string username);
        Task<List<Update>> GetUpdates(Func<Update, bool> predicate);
        Task StartConversation(Update update);
		Task EndConversation(Update update);
        Task StartListening();
    }
}
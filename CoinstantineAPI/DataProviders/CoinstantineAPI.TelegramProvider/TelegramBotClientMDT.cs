using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoinstantineAPI.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CoinstantineAPI.DataProvider.TelegramProvider
{
    public class TelegramBotClientMDT : ITelegramBotClientMDT
    {
        private readonly TelegramBotClient _bot;
        private readonly List<Update> _updates;
        private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        private bool _started;
        private int _lastOffset = 557621748;

        public TelegramBotClientMDT()
        {
            _bot = new TelegramBotClient(Constants.TelegramBotToken);
            _updates = new List<Update>();
        }

        public async Task StartListening()
        {
            if (_started) { return; }
            try
            {
                _started = true;
                while (true)
                {
                    try
                    {
                        await Task.Delay(1000);
                        await _semaphoreSlim.WaitAsync();
                        var updates = await _bot.GetUpdatesAsync(_lastOffset + 1);
                        _updates.AddRange(updates);
                        _lastOffset = _updates.LastOrDefault()?.Id ?? _lastOffset;
                    }
                    finally
                    {
                        _semaphoreSlim.Release();
                    }
                }
            }
            finally
            {
                _started = false;
            }
        }

        public async Task<List<Update>> GetUpdates(Func<Update, bool> predicate)
        {
            try
            {
                await _semaphoreSlim.WaitAsync();
                return _updates.Where(predicate).ToList();
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public async Task CleanUpdates(string username)
        {
            try
            {
                await _semaphoreSlim.WaitAsync();
                _updates.RemoveAll(x => x.Message?.From?.Username == username);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public async Task StartConversation(Update update)
        {
            await _bot.SendTextMessageAsync(update.Message.Chat.Id,
                                                    $"Hi {update.Message.From.FirstName}, thanks for using our app! This bot has only one purpose, for now. Check your phone number");
            var message = await _bot.SendTextMessageAsync(
                        chatId: update.Message.Chat.Id,
                        text: "Share your contact info using the keyboard reply markup provided.",
                           replyMarkup: new ReplyKeyboardMarkup(
                               keyboardRow: new[] { KeyboardButton.WithRequestContact("Share Contact"), },
                               resizeKeyboard: true,
                    oneTimeKeyboard: true));
        }

        public async Task EndConversation(Update update)
		{
			await _bot.SendTextMessageAsync(update.Message.Chat.Id, "Thank you. You can go back to Coinstantine app now.");
		}
    }
}
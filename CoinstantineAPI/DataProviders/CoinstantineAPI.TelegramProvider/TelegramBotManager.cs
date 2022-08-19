using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using CoinstantineAPI.Core;
using CoinstantineAPI.TelegramProvider.Entities;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace CoinstantineAPI.TelegramProvider
{
    public class TelegramBotManager : ITelegramBotManager
    {
        private readonly TelegramBotClient _bot;
        private readonly string _url;
        private readonly ConcurrentDictionary<string, (DateTime, Action<AppUpdate>)> _currentUsers;
        private readonly int _timeout;
        private bool _started;
        public TelegramBotManager()
        {
            _bot = new TelegramBotClient(Constants.TelegramBotToken);

            _currentUsers = new ConcurrentDictionary<string, (DateTime, Action<AppUpdate>)>();
            _timeout = 15;
            if (int.TryParse(Constants.TelegramTimeout, out var timeout))
            {
                _timeout = timeout;
            }
            _url = Constants.TelegramUrlWebhook;
        }

        public async Task StartListening()
        {
            if (_started)
            {
                return;
            }
            _started = true;
            await _bot.SetWebhookAsync($"{_url}/api/telegramWebhook");
        }

        public async Task HandleUpdate(AppUpdate update)
        {
            if (update == null)
            {
                return;
            }
            if (!await UserSubscribed(update))
            {
                return;
            }
            if (update.Message?.Text == "/start")
            {
                await StartConversation(update);
            }
            else if (update.Message?.Contact != null)
            {
                await EndConversation(update);
            }
            else if (update?.Message?.Entities?.Any(x => x.Type == "phone_number") ?? false)
            {
                await UseProvidedButton(update);
            }
            else
            {
                await WontProcess(update);
            }
        }

        private async Task<bool> UserSubscribed(AppUpdate update)
        {
            var username = update?.Message?.From?.Username;
            if (username == null)
            {
                return false;
            }
            if (_currentUsers.ContainsKey(username.ToLower()))
            {
                return true;
            }
            await UseTheAppPlease(update);
            return false;
        }

        private async Task UseTheAppPlease(AppUpdate update)
        {
            await _bot.SendTextMessageAsync(update.Message.Chat.Id, $"Hi, please use the 'Start conversation' button from the app. So I can link {update.Message.From.Username} to a Coinstantine user. Thanks !");
        }

        private async Task UseProvidedButton(AppUpdate update)
        {
            var message = await _bot.SendTextMessageAsync(
                        chatId: update.Message.Chat.Id,
                        text: "Please use the provided button to share your details. Do not enter them directly",
                           replyMarkup: new ReplyKeyboardMarkup(
                               keyboardRow: new[] { KeyboardButton.WithRequestContact("Share Contact"), },
                               resizeKeyboard: true,
                    oneTimeKeyboard: true));
        }

        private async Task WontProcess(AppUpdate update)
        {
            await _bot.SendTextMessageAsync(update.Message.Chat.Id, "I see you're trying to tell me something here... Aren't you ?");
        }

        public async Task StartConversation(AppUpdate update)
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

        public async Task EndConversation(AppUpdate update)
        {
            await _bot.SendTextMessageAsync(update.Message.Chat.Id, "Thank you. You can go back to Coinstantine app now.");
            var username = update?.Message?.From?.Username;
            if (username != null && _currentUsers.ContainsKey(username.ToLower()))
            {
                _currentUsers.TryRemove(username.ToLower(), out var details);
                details.Item2?.Invoke(update);
            }
        }

        private void CleanUp()
        {
            var list = _currentUsers.Where(x => (DateTime.Now - x.Value.Item1).TotalMinutes > 15);
            foreach (var item in list)
            {
                _currentUsers.TryRemove(item.Key, out var date);
            }
        }

        public async Task StartListeningForUser(string username, Action<AppUpdate> callback)
        {
            await StartListening();
            _currentUsers.TryAdd(username.ToLower(), (DateTime.Now, callback));
        }
    }
}
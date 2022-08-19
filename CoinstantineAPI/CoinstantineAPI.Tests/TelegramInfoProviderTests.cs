using System;
using System.Threading.Tasks;
using CoinstantineAPI.TelegramProvider;
using CoinstantineAPI.TelegramProvider.Entities;
using CoinstantineAPI.Tests.Builders;
using FakeItEasy;
using Xunit;

namespace CoinstantineAPI.Tests
{
    public class TelegramInfoProviderTests
    {
        [Fact]
        public async Task Start_Converstation_Should_Start_Getting_Updates()
        {
            var telegramBotClient = A.Fake<ITelegramBotManager>();
            var service = new TelegramInfoProviderBuilder()
                .WithTelegramBotClient(telegramBotClient)
                .Build();
            
            var username = "moh";

            await service.ProcessConversationOnTelegram(username);
            A.CallTo(() => telegramBotClient.StartListeningForUser(username, A<Action<AppUpdate>>.Ignored)).MustHaveHappened();
        }
    }
}

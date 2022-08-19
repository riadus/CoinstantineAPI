using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace CoinstantineAPI.DiscordBot
{
    public static class DiscordBotDependencyInjection
    {
        public static IServiceCollection AddDiscordBot(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IDiscordBot, CommandHandlingService>()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton(provider => new CommandService(new CommandServiceConfig
                {
                    // Again, log level:
                    LogLevel = LogSeverity.Info,

                    // There's a few more properties you can set,
                    // for example, case-insensitive commands.
                    CaseSensitiveCommands = false,
                }));

            return serviceCollection;
        }
    }
}

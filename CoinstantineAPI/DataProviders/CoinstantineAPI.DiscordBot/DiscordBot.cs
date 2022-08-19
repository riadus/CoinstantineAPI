using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CoinstantineAPI.Core;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.Games;
using CoinstantineAPI.Core.Statistics;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Data;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;

namespace CoinstantineAPI.DiscordBot
{
    public class CommandHandlingService : IDiscordBot
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discordClient;
        private readonly IServiceProvider _services;

        public CommandHandlingService(DiscordSocketClient discordClient, CommandService commandServices, IServiceProvider services)
        {
            _discordClient = discordClient;
            _commands = commandServices;
            _services = services;
            // Hook CommandExecuted to handle post-command-execution logic.
            _commands.CommandExecuted += CommandExecutedAsync;
            // Hook MessageReceived so we can process each message to see
            // if it qualifies as a command.
            _discordClient.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync()
        {
            if(string.IsNullOrEmpty(Constants.DiscordBotToken))
            {
                return;
            }
            await _discordClient.LoginAsync(TokenType.Bot, Constants.DiscordBotToken);
            await _discordClient.StartAsync();
            // Register modules that are public and inherit ModuleBase<T>.
            await _commands.AddModuleAsync(typeof(MentionDiscordCommands), _services);
            await _commands.AddModuleAsync(typeof(StatDiscordCommands), _services); 
            await _commands.AddModuleAsync(typeof(GiveAwaysCommands), _services); 
        }

        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // Ignore system messages, or messages from other bots
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            // This value holds the offset where the prefix ends
            var argPos = 0;
            // Perform prefix check. You may want to replace this with
            // ()
            // for a more traditional command format like !help.
            var hasPrefix = message.HasCharPrefix('!', ref argPos);
            var mentions = message.HasMentionPrefix(_discordClient.CurrentUser, ref argPos);
            if (!mentions && !hasPrefix) return;

            var context = new SocketCommandContext(_discordClient, message);
            // Perform the execution of the command. In this method,
            // the command service will perform precondition and parsing check
            // then execute the command if one is matched.
            await _commands.ExecuteAsync(context, argPos, _services);
            // Note that normally a result will be returned by this format, but here
            // we will handle the result in CommandExecutedAsync,
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // command is unspecified when there was a search failure (command not found); we don't care about these errors
            if (!command.IsSpecified)
                return;

            // the command was successful, we don't care about this result, unless we want to log that a command succeeded.
            if (result.IsSuccess)
                return;

            // the command failed, let's notify the user that something happened.
            await context.Channel.SendMessageAsync($"error: {result}");
        }
    }

    public class GiveAwaysCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IGameService _gameService;

        public GiveAwaysCommands(IGameService gameService)
        {
            _gameService = gameService;
        }

        [Command("giveaway")]
        public async Task GiveAway(string user, int amount, string description = "")
        {
            bool success = false;
            try
            {
                success = await _gameService.GiveAway("Coinstantine on Discord", user, amount, Context.User.Id.ToString(), description);
            }
            catch(Exception)
            {
                await ReplyAsync($"You're not allowed to give away CSN. That was a smart move though");
                return;
            }
            if(success)
            {
                await ReplyAsync($"Congrats {user}, you received {amount} CSN!");
                return;
            }
            await ReplyAsync($"Wait... Am I supposed to know {user}?");
        }
    }

    public class StatDiscordCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IStatisticsProvider _statisticsProvider;

        public StatDiscordCommands(IStatisticsProvider statisticsProvider)
        {
            _statisticsProvider = statisticsProvider;
        }

        [Command("stats")]
        public async Task Stats()
        {
            var statistics = await _statisticsProvider.GetStatistics();
            await ReplyAsync(statistics.Print());
        }

        [Command("force-stats")]
        public async Task ForcedStats()
        {
            var statistics = await _statisticsProvider.GetForcedStatistics(Context.User.Id.ToString());
            if(statistics == null)
            {
                await ReplyAsync($"Sorry, you don't have the permissions");
                return;
            }

            await ReplyAsync(statistics.Print());
        }
    }

    public class MentionDiscordCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IContextProvider _contextProvider;
        private readonly IUsersService _usersService;

        public MentionDiscordCommands(IContextProvider contextProvider, IUsersService usersService)
        {
            _contextProvider = contextProvider;
            _usersService = usersService;
        }

        [Command("help")]
        public async Task LoveYou()
        {
            await ReplyAsync("!username {YourUsernameInCoinstantineApp} -- Validates your Discord account");
            await ReplyAsync("!my-balance -- gives your current balance");
        }

        [Command("username")]
        public async Task Subscribe([Remainder] string username)
        {
            await ReplyAsync($"👋 Hi, {Context.User.Username}! Let me check...");
            var apiUser = await _usersService.GetUserFromUsername(username);
            if(apiUser == null)
            {
                await ReplyAsync($"Hum... The username {username} does not exist. Do you even {Context.User.Username} ?");
                return;
            }
            var alreadySubscribed = await _usersService.AddDiscordProfile(apiUser, new DiscordProfile
            {
                ApiUser = apiUser,
               DiscordServerName = Context.Guild.Name,
               DiscordUserIdentifier = Context.User.Id.ToString(),
               Username = Context.User.Username,
               Validated = true,
               ValidationDate = DateTime.Now,
               JoinedDate = Context.User.CreatedAt.UtcDateTime
            });

            if(alreadySubscribed)
            {
                await ReplyAsync($"I knew it! Your face is familiar, you're already registred {Context.User.Username}");
                return;
            }
            await ReplyAsync($"All good {Context.User.Username}. Thanks for joining.");
        }

        [Command("my-balance")]
        public async Task MyBalance()
        {
            await ReplyAsync($"👋 Hi, {Context.User.Username}! Let me check...");
            using (var context = _contextProvider.CoinstantineContext)
            {
                var discordProfile = await context.DiscordProfiles
                    .Include(x => x.ApiUser)
                        .ThenInclude(x => x.BlockchainInfo)
                    .FirstOrDefaultAsync(x => x.DiscordUserIdentifier == Context.User.Id.ToString()
                && x.DiscordServerName == Context.Guild.Name);

                if(discordProfile == null)
                {
                    await ReplyAsync($"Hey, {Context.User.Username}! I don't know you actually.");
                    return;
                }

                await ReplyAsync($"Hey, {Context.User.Username}! Your current balance is : {discordProfile.ApiUser.BlockchainInfo.Coinstantine} CSN");
            }
        }

        [Command("balance")]
        public async Task Balance(string user)
        {
            await ReplyAsync($"👋 Hi, {Context.User.Username}! Let me check...");
            using (var context = _contextProvider.CoinstantineContext)
            {
                var userApi = await _usersService.GetUserFromDiscord(user, "Coinstantine");
                if (userApi == null)
                {
                    await ReplyAsync($"Hey, {Context.User.Username}! I don't know {user} actually. Please use this command line first : !username {user}");
                    return;
                }

                await ReplyAsync($"Hey, {Context.User.Username}! {user}'s current balance is : {userApi.BlockchainInfo.Coinstantine} CSN");
            }
        }
    }
}

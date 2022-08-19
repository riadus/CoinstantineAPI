using System.Threading.Tasks;

namespace CoinstantineAPI.DiscordBot
{
    public interface IDiscordBot
    {
        Task InitializeAsync();
    }
}

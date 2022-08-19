using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Users.Unicity.Strategies;

namespace CoinstantineAPI.Users.Unicity
{
    public interface IUnicityConstraintsFactory
    {
        IUnicityConstraintsStrategie GetStrategie(UnicityTopic topic);
    }
}

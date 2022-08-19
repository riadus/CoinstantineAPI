using System.Threading.Tasks;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Core.Users
{
    public interface IUserCreationService
    {
        Task<bool> SubscribeUser(AccountCreationModel accountCreationModel);
        Task<UserIdentity> GetUser(string email);
        Task<(ApiUser, bool)> CreateApiUser(AzureUser azureUser, string username);

        Task<AccountCorrect> IsAccountCorrect(AccountCreationModel accountCreationModel);
        bool CorrectPassword(string password);
        Task<UserIdentity> GetUserFromConfirmationCode(string confirmationCode);
        Task<bool> CreateApiUser(UserIdentity userIdentity);
        Task<bool> SendUsername(string email);
        Task<bool> ResetPassword(string email);
        Task<bool> ChangePassword(AccountChangeModel accountChangeModel);
        Task CreateAdmin();
    }
}

namespace CoinstantineAPI.Core.Users
{
    public interface IPasswordService
    {
        (string Hash, string Salt) CreatePasswordHash(string password);
        bool VerifyPasswordHash(string password, string hash, string salt);
    }
}

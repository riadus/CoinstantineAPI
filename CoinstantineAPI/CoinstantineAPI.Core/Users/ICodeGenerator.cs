namespace CoinstantineAPI.Core.Users
{
    public interface ICodeGenerator
    {
        string GenerateCode();
        string GenerateCode(int length);
    }
}

namespace CoinstantineAPI.Scan.Interfaces
{
    public interface IApiClientFactory
    {
        IApiClient GetApiClient(ApiType type);
    }
}

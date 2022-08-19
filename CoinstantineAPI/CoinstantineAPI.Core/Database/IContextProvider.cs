namespace CoinstantineAPI.Core.Database
{
    public interface IContextProvider
    {
        IContext CoinstantineContext { get; }
    }
}

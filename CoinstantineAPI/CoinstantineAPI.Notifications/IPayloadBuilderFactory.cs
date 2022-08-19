namespace CoinstantineAPI.Notifications
{
    public interface IPayloadBuilderFactory
    {
        IPayloadBuilder GetBuilder(Platform platform);
    }
}

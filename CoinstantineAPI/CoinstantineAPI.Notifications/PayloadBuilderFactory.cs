namespace CoinstantineAPI.Notifications
{
    public class PayloadBuilderFactory : IPayloadBuilderFactory
    {
        public IPayloadBuilder GetBuilder(Platform platform)
        {
            return platform == Platform.iOS ? (IPayloadBuilder) new PayloadBuilderForiOS() : new PayloadBuilderForAndroid();
        }
    }
}

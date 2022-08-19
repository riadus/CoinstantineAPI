using CoinstantineAPI.Notifications.Android;

namespace CoinstantineAPI.Notifications
{
    public class PayloadBuilderForAndroid : PayloadBuilder
    {
        public override IPayload Build()
        {
            return new Payload
            {
                Data = new AndroidData
                {
                    Body = _body,
                    PartToUpdate = _partToUpdate,
                    Silent = 1,
                    Title = _title
                }
            };
        }

        public override IPayload BuildSilent()
        {
            return new Payload
            {
                Data = new AndroidData
                {
                    Body = _body,
                    PartToUpdate = _partToUpdate,
                    Silent = 0,
                    Title = _title,
                    TranslationKey = _translationKey
                }
            };
        }
    }
}
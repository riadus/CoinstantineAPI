using CoinstantineAPI.Core.Extensions;
using CoinstantineAPI.Notifications.iOS;

namespace CoinstantineAPI.Notifications
{
    public class PayloadBuilderForiOS : PayloadBuilder
    {
        public override IPayload Build()
        {
            APS aps;
            if (_body.IsNullOrEmpty())
            {
                aps = new SimpleAPS
                {
                    Alert = _title
                };
            }
            else
            {
                var alert = new Alert
                {
                    Title = _title,
                    Body = _body
                };
                aps = new ComplexAPS
                {
                    Alert = alert
                };
            }
            aps.Badge = _badge;
            aps.ContentAvailable = _contentAvailable ? (int?)1 : null;
            aps.Sound = _sound ? "default" : null;
            aps.PartToUpdate = _partToUpdate;
            return _builtPayload = new Payload
            {
                APS = aps
            };
        }

        public override IPayload BuildSilent()
        {
            return _builtPayload = new Payload
            {
                APS = new SimpleAPS
                {
                    ContentAvailable = 1,
                    PartToUpdate = _partToUpdate,
                    TranslationKey = _translationKey
                }
            };
        }
    }
}

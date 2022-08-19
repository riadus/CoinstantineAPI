using CoinstantineAPI.Core.Services;
using CoinstantineAPI.Notifications.iOS;

namespace CoinstantineAPI.Notifications
{
    public abstract class PayloadBuilder : IPayloadBuilder
    {
        protected string _title;
        protected string _body;
        protected int _badge;
        protected bool _contentAvailable;
        protected bool _sound;
        protected string _translationKey;
        protected PartToUpdate? _partToUpdate;
        protected Payload _builtPayload;

        public IPayloadBuilder SetBadge(int badge)
        {
            _badge = badge;
            return this;
        }

        public IPayloadBuilder SetBody(string body)
        {
            _body = body;
            return this;
        }

        public IPayloadBuilder SetContentAvailable()
        {
            _contentAvailable = true;
            return this;
        }

        public IPayloadBuilder SetSound()
        {
            _sound = true;
            return this;
        }

        public IPayloadBuilder SetTitle(string title)
        {
            _title = title;
            return this;
        }

        public IPayloadBuilder SetPartToUpdate(PartToUpdate partToUpdate)
        {
            _partToUpdate = partToUpdate;
            return this;
        }

        public IPayloadBuilder SetTranslationKey(string translationKey)
        {
            _translationKey = translationKey;
            return this;
        }

        public abstract IPayload Build();
        public abstract IPayload BuildSilent();
    }
}

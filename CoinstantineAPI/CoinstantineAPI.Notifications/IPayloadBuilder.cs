using CoinstantineAPI.Core.Services;

namespace CoinstantineAPI.Notifications
{
    public interface IPayloadBuilder
    {
		IPayload Build();
		IPayload BuildSilent();
		IPayloadBuilder SetTitle(string title);
		IPayloadBuilder SetBody(string body);
        IPayloadBuilder SetTranslationKey(string translationKey);
		IPayloadBuilder SetSound();
		IPayloadBuilder SetBadge(int badge);
		IPayloadBuilder SetContentAvailable();
		IPayloadBuilder SetPartToUpdate(PartToUpdate partToUpdate);
    }

    public interface IPayload
    {

    }
}

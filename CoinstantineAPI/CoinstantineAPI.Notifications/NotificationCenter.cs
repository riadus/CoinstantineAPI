using System;
using System.Threading.Tasks;
using CoinstantineAPI.Core;
using CoinstantineAPI.Core.Extensions;
using CoinstantineAPI.Core.Services;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Logging;

namespace CoinstantineAPI.Notifications
{
	public class NotificationCenter : INotificationCenter
	{
		private readonly NotificationHubClient _hub;
        private readonly IPayloadBuilder _payloadBuilderForAnroid;
        private readonly IPayloadBuilder _payloadBuilderForiOS;
        private readonly ILogger _logger;

        public NotificationCenter(IPayloadBuilderFactory payloadBuilderFactory,
                                  ILoggerFactory loggerFactory)
		{
            _hub = NotificationHubClient.CreateClientFromConnectionString(Constants.AzureNotificationAccessSignature, Constants.AzureNotificationHubsName);
            _payloadBuilderForAnroid = payloadBuilderFactory.GetBuilder(Platform.Android);
            _payloadBuilderForiOS = payloadBuilderFactory.GetBuilder(Platform.iOS);
            _logger = loggerFactory.CreateLogger(GetType());
		}

		public async Task SendNotification(string message, string email)
        {
            await SendNotificationToAppleDevice(message, email);
            await SendNotificationToAndroidDevice(message, email);
        }

        public async Task SendSilentNotification(string email, string translationKey, PartToUpdate partToUpdate)
        {
            await SendSilentNotificationToAppleDevice(email, translationKey, partToUpdate);
            await SendSilentNotificationToAndroidDevice(email, translationKey, partToUpdate);
        }

        private async Task SendNotificationToAppleDevice(string message, string email)
        {
            try
            {
                var payload = _payloadBuilderForiOS.SetTitle("Coinstantine")
                                             .SetBody(message)
                                             .Build();

                var serializedPayload = payload.Serialize();
                await _hub.SendAppleNativeNotificationAsync(serializedPayload, email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SendNotification()", message, email);
            }
        }

        private async Task SendNotificationToAndroidDevice(string message, string email)
        {
            try
            {
                var payload = _payloadBuilderForAnroid.SetTitle("Coinstantine")
                                             .SetBody(message)
                                             .Build();

                var serializedPayload = payload.Serialize();
                var result = await _hub.SendFcmNativeNotificationAsync(serializedPayload, email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SendNotification()", message, email);
            }
        }

        private async Task SendSilentNotificationToAppleDevice(string email, string translationKey, PartToUpdate partToUpdate)
        {
            try
            {
                var payload = _payloadBuilderForiOS.SetPartToUpdate(partToUpdate)
                                                   .SetTranslationKey(translationKey)
                                                   .BuildSilent();

                var serializedPayload = payload.Serialize();
                await _hub.SendAppleNativeNotificationAsync(serializedPayload, email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SendSilentNotification()", email, partToUpdate);
            }
        }

        private async Task SendSilentNotificationToAndroidDevice(string email, string translationKey, PartToUpdate partToUpdate)
        {
            try
            {
                var payload = _payloadBuilderForAnroid.SetPartToUpdate(partToUpdate)
                                                      .SetTranslationKey(translationKey)
                                                      .BuildSilent();

                var serializedPayload = payload.Serialize();
                await _hub.SendFcmNativeNotificationAsync(serializedPayload, email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SendSilentNotification()", email, partToUpdate);
            }
        }
    }
}

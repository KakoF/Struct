using MetricsConfiguration.Domain.Interface.Client;
using MetricsConfiguration.Domain.Interfaces.Notifications;
using MetricsConfiguration.Domain.Notifications;
using Microsoft.Extensions.Logging;

namespace MetricsConfiguration.Infrastructure.Client
{
    public class AtlasTriggerConfigurationClient : ClientAbstract, IAtlasTriggerConfigurationClient
    {
        protected override string _nameClient => "AtlasTriggerConfigurationClient";
        public AtlasTriggerConfigurationClient(IHttpClientFactory clientFactory, ILogger<AtlasTriggerConfigurationClient> logger, INotificationHandler<Notification> notification) : base(clientFactory, logger, notification)
        {
        }

        protected override string ParseMessageError(string error)
        {
            return error;
        }
    }
}
using MetricsConfiguration.Domain.Interface.Client;
using MetricsConfiguration.Domain.Interfaces.Notifications;
using MetricsConfiguration.Domain.Notifications;
using Microsoft.Extensions.Logging;

namespace MetricsConfiguration.Infrastructure.Client
{
    public class ChuckClient : ClientAbstract, IChuckClient
    {
        protected override string _nameClient => "ChuckClient";
        public ChuckClient(IHttpClientFactory clientFactory, ILogger<ChuckClient> logger, INotificationHandler<Notification> notification) : base(clientFactory, logger, notification)
        {
        }

        protected override string ParseMessageError(string error)
        {
            return error;
        }
    }
}
using MetricsConfiguration.Domain.Interface.Client;
using MetricsConfiguration.Domain.Interfaces.Notifications;
using MetricsConfiguration.Domain.Notifications;
using Microsoft.Extensions.Logging;

namespace MetricsConfiguration.Infrastructure.Client
{
    public class AdviceClient : ClientAbstract, IAdviceClient
    {
        protected override string _nameClient => "AdviceClient";
        public AdviceClient(IHttpClientFactory clientFactory, ILogger<AdviceClient> logger, INotificationHandler<Notification> notification) : base(clientFactory, logger, notification)
        {
        }

        protected override string ParseMessageError(string error)
        {
            return error;
        }
    }
}
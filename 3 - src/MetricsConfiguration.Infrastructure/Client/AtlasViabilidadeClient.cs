using MetricsConfiguration.Domain.Interface.Client;
using MetricsConfiguration.Domain.Interfaces.Notifications;
using MetricsConfiguration.Domain.Model.Client.Base;
using MetricsConfiguration.Domain.Notifications;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MetricsConfiguration.Infrastructure.Client
{
    public class AtlasViabilidadeClient : ClientAbstract, IAtlasViabilidadeClient
    {
        protected override string _nameClient => "AtlasViabilidadeClient";
        public AtlasViabilidadeClient(IHttpClientFactory clientFactory, ILogger<AtlasViabilidadeClient> logger, INotificationHandler<Notification> notification) : base(clientFactory, logger, notification)
        {

            _client.DefaultRequestHeaders.Add("SESSID", "1");
            _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/121.0.0.0 Safari/537.36");

        }

        protected override string ParseMessageError(string error)
        {
            try
            {
                return JsonConvert.DeserializeObject<AtlasErrorClients>(error).Message;
            }
            catch
            {
                return error;
            }
        }
    }
}
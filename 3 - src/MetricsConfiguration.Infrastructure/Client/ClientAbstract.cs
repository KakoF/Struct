using MetricsConfiguration.Domain.Interface.Client;
using MetricsConfiguration.Domain.Interfaces.Notifications;
using MetricsConfiguration.Domain.Notifications;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace MetricsConfiguration.Infrastructure.Client
{
    public abstract class ClientAbstract : IClientMethods
    {
        protected abstract string _nameClient { get; }
        protected readonly HttpClient _client;
        protected readonly ILogger<ClientAbstract> _logger;
        private readonly INotificationHandler<Notification> _notification;
        public ClientAbstract(IHttpClientFactory clientFactory, ILogger<ClientAbstract> logger, INotificationHandler<Notification> notification)
        {
            _client = clientFactory.CreateClient(_nameClient);
            _logger = logger;
            _notification = notification;
        }

        protected abstract string ParseMessageError(string error);

        public virtual async Task<T> GetAsync<T>(string path)
        {
            try
            {
                var clientResponse = await _client.GetAsync(path);
                if (clientResponse.IsSuccessStatusCode)
                {
                    var response = JsonConvert.DeserializeObject<T>(await clientResponse.Content.ReadAsStringAsync());
                    return response;
                }
                var error = await clientResponse.Content.ReadAsStringAsync();
                _notification.AddNotification((int)clientResponse.StatusCode, ParseMessageError(error));
                _logger.LogError("Erro em resposta do client: {}, {}", $"Erro {_client.BaseAddress}/{path}", error);
                return default;
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro em consumir client: {}, {}", $"Erro {_client.BaseAddress}/{path}", ex);
                return default;
            }

        }


        public virtual async Task<T> PostAsync<T>(string path, object data)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(data), UnicodeEncoding.UTF8, "application/json");
                var clientResponse = await _client.PostAsync(path, content);
                if (clientResponse.IsSuccessStatusCode)
                {
                    var response = JsonConvert.DeserializeObject<T>(await clientResponse.Content.ReadAsStringAsync());
                    return response;
                }
                var error = await clientResponse.Content.ReadAsStringAsync();
                _notification.AddNotification((int)clientResponse.StatusCode, ParseMessageError(error));
                _logger.LogError("Erro em resposta do client: {}, {}", $"Erro {_client.BaseAddress}/{path}", error);
                return default;
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro em consumir client: {}, {}", $"Erro {_client.BaseAddress}/{path}", ex);
                return default;
            }

        }

        public virtual async Task<T> PutAsync<T>(string path, object data)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(data), UnicodeEncoding.UTF8, "application/json");
                var clientResponse = await _client.PutAsync(path, content);
                if (clientResponse.IsSuccessStatusCode)
                {
                    var response = JsonConvert.DeserializeObject<T>(await clientResponse.Content.ReadAsStringAsync());
                    return response;
                }
                var error = await clientResponse.Content.ReadAsStringAsync();
                _notification.AddNotification((int)clientResponse.StatusCode, ParseMessageError(error));
                _logger.LogError("Erro em resposta do client: {}, {}", $"Erro {_client.BaseAddress}/{path}", error);
                return default;
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro em consumir client: {}, {}", $"Erro {_client.BaseAddress}/{path}", ex);
                return default;
            }
        }

        public virtual async Task<T> DeleteAsync<T>(string path)
        {
            try
            {
                var clientResponse = await _client.DeleteAsync(path);
                if (clientResponse.IsSuccessStatusCode)
                {
                    var response = JsonConvert.DeserializeObject<T>(await clientResponse.Content.ReadAsStringAsync());
                    return response;
                }
                var error = await clientResponse.Content.ReadAsStringAsync();
                _notification.AddNotification((int)clientResponse.StatusCode, ParseMessageError(error));
                _logger.LogError("Erro em resposta do client: {}, {}", $"Erro {_client.BaseAddress}/{path}", error);
                return default;
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro em consumir client: {}, {}", $"Erro {_client.BaseAddress}/{path}", ex);
                return default;
            }
        }

       
    }
}
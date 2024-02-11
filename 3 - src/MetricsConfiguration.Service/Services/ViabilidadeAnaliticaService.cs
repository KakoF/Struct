using MetricsConfiguration.Domain.Exceptions;
using MetricsConfiguration.Domain.Interface.Client;
using MetricsConfiguration.Domain.Interface.Services;
using MetricsConfiguration.Domain.Interfaces.Notifications;
using MetricsConfiguration.Domain.Model.Client;
using MetricsConfiguration.Domain.Model.Rule;
using MetricsConfiguration.Domain.Notifications;

namespace MetricsConfiguration.Service.Services
{
    public class ViabilidadeAnaliticaService : IViabilidadeAnaliticaService
    {
        private readonly IAtlasViabilidadeClient _client;
        private readonly INotificationHandler<Notification> _notification;

        public ViabilidadeAnaliticaService(IAtlasViabilidadeClient client, INotificationHandler<Notification> notification)
        {
            _client = client;
            _notification = notification;
        }
        public async Task<RuleModel> GetRuleAsync(int id)
        {
            return await _client.GetAsync<RuleModel>($"rule/{id}");
        }

        public Task<RuleModel> GetRuleDomainExceptionAsync(int id)
        {
            throw new DomainException("Regra não encontrada", 404);
        }

        public Task<RuleModel> GetRuleRuleNotificationAsync(int id)
        {
            _notification.AddNotification(404, "Regra não encontrada", $"Regra {id} não encontrada na base");
            return Task.FromResult<RuleModel>(null);
        }

        public Task<RuleModel> CreateAsync(CreateRuleRequest request)
        {
            var rule = new RuleModel()
            {
                Id = 1,
                Name = request.Name,
                Description = request.Description,
            };
            return Task.FromResult<RuleModel>(rule);
        }
    }
}

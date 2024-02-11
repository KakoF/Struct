using MetricsConfiguration.Domain.Model.Client;
using MetricsConfiguration.Domain.Model.Rule;

namespace MetricsConfiguration.Domain.Interface.Services
{
    public interface IViabilidadeAnaliticaService
    {
        Task<RuleModel> CreateAsync(CreateRuleRequest request);
        Task<RuleModel> GetRuleAsync(int id);
        Task<RuleModel> GetRuleDomainExceptionAsync(int id);
        Task<RuleModel> GetRuleRuleNotificationAsync(int id);
    }
}

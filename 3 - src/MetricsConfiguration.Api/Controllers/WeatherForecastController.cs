using MetricsConfiguration.Domain.Interface.Cache;
using MetricsConfiguration.Domain.Interface.Services;
using MetricsConfiguration.Domain.Model.Client;
using MetricsConfiguration.Domain.Model.Rule;
using Microsoft.AspNetCore.Mvc;

namespace MetricsConfiguration.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IRedisCacheStorage _redis;
        private readonly IAdviceService _adviceService;
        private readonly IChuckService _chuckService;
        private readonly IViabilidadeAnaliticaService _viabilidadeAnaliticaService;


        public WeatherForecastController(IRedisCacheStorage redis, IAdviceService adviceService, IChuckService chuckService, IViabilidadeAnaliticaService viabilidadeAnaliticaService)
        {
            _redis = redis;
            _adviceService = adviceService;
            _chuckService = chuckService;
            _viabilidadeAnaliticaService = viabilidadeAnaliticaService;
        }
        [HttpGet]
        public async Task<string> GetAsync()
        {
            var result = await _redis.GetOrCreateAsync("teste", () => Retorna(), DateTimeOffset.Now.AddMinutes(5));
            return result;
        }


        [HttpGet]
        [Route("RandomAdvice")]
        public async Task<AdviceModel> RandomAdviceAsync()
        {
            return await _adviceService.GetRandomAdviceAsync();
        }


        [HttpGet]
        [Route("RandomJoke")]
        public async Task<ChuckNorrisModel> RandomJokeAsync()
        {
            return await _chuckService.GetRandomJokeAsync();
        }


        [HttpGet]
        [Route("Rule/{id}")]
        public async Task<RuleModel> RuleAsync(int id)
        {
            return await _viabilidadeAnaliticaService.GetRuleAsync(id);
        }

        [HttpGet]
        [Route("Rule/DomainException/{id}")]
        public async Task<RuleModel> RuleDomainExceptionAsync(int id)
        {
            return await _viabilidadeAnaliticaService.GetRuleDomainExceptionAsync(id);
        }

        [HttpGet]
        [Route("Rule/Notification/{id}")]
        public async Task<RuleModel> RuleNotificationAsync(int id)
        {
            return await _viabilidadeAnaliticaService.GetRuleRuleNotificationAsync(id);
        }

        [HttpPost]
        [Route("Rule")]
        public async Task<RuleModel> CreateAsync([FromBody] CreateRuleRequest request)
        {
            return await _viabilidadeAnaliticaService.CreateAsync(request);
        }

        private Task<string> Retorna()
        {
            return Task.FromResult<string>("Função teste");
        }
    }
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsConfiguration.Domain.Model.Client
{
    public class ChuckNorrisModel
    {

        public List<object> Categories { get; set; }
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
        [JsonProperty("icon_url")]
        public string IconUrl { get; set; }
        public string Id { get; set; }
        [JsonProperty("update_at")]
        public string UpdatedAt { get; set; }
        public string Url { get; set; }
        public string Value { get; set; }
    }
}
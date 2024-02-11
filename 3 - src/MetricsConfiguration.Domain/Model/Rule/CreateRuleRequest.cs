using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsConfiguration.Domain.Model.Rule
{
    public class CreateRuleRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiEasier.Dm.Models.Dynamic
{
    public class DynamicResource
    {
        public string Name { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawl_console
{
    public class DanhMuc
    {
        [JsonProperty("link")]
        public string href { get; set; }
        [JsonProperty("text")]
        public string title { get; set; }
        public string id { get; set; }
    }
}

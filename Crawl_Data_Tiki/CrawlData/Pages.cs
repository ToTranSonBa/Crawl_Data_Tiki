using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawl_Data_Tiki.CrawlData
{
    public class Pages
    {
        public int LastPage { get; set; }
        public int Total { get; set; }
        public int PerPage { get; set; } = 40;
    }
}

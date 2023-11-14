using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawl_Data_Tiki.CrawlData
{
    public class Discount
    {
        public Guid ProductId { get; set; }
        public Guid Id { get; set; }
        public string label { get; set; }
        public string Short_Description { get; set; }
        public string period { get; set; }
    }
}

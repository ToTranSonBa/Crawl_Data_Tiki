using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawl_Data_Tiki.ClassExtention
{
    [Serializable]
    public class PlanData
    {
        public List<PlanItem> Jobs { get; set; }
    }
}

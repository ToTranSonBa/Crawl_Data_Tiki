using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawl_Data_Tiki
{
    public class ExtentionMethod
    {
        public static bool IsEqualDate(DateTime d1, DateTime d2)
        {
            return (d1.Year == d2.Year && d1.Month == d2.Month && d1.Day == d2.Day);
        }
    }
}

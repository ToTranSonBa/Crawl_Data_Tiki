using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawl_console
{
    public class Shop
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public int StoreID { get; set; }
        public bool IsOfficial { get; set; }
        public float AvgRating { get; set; }
        public int ReviewCount { get; set; }
        public int TotalFollower {  get; set; }
    }
}

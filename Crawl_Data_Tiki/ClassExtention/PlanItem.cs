using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawl_Data_Tiki.ClassExtention
{
    public class PlanItem
    {
        public DateTime StartDate { get; set; } = DateTime.Now;
        public Point StartTime { get; set; } = new Point(0, 0); 
        public Point EndTime { get; set; } = new Point(0, 0);
        public DateTime EndDate { get; set; } = DateTime.Now;
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public int Repeat { get; set; } = 0;
        public string status { get; set; } = "COMING";
        public static List<string> ListStatus = new List<string> { "DONE", "DOING", "COMING", "MISSED" };
        public static List<int> ListRepeat = new List<int> { 0, 1, 7, 15, 30 };
    }
    public enum Status
    {
        DONE,
        DOING,
        COMING,
        MISSED
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawl_Data_Tiki.CrawlData
{
    public class Product
    {
        public int Id { get; set; }
        public string sku {  get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public int Price { get; set; }
        public int OriginalPrice { get; set; }
        public int Discount { get; set; }
        public int DiscountRate { get; set; }
        public int QuantitySold { get; set; }
        public float Rating { get; set; }
        public int ReviewCount { get; set; }
        public int CategoriesID { get; set; }
        public string CategoryName { get; set; }
        public string DanhMuc {  get; set; }
        public string DanhMucId { get; set; }
        public Brand Brand { get; set; }
        public Shop Shop { get; set; }

        

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Crawl_console
{
    public static class Extensions
    {
        public static string GetCategory(this DanhMuc danhMuc)
        {
            var href = danhMuc.href;
            int lastSlashIndex = href.LastIndexOf('/');

            if (lastSlashIndex != -1)
            {
                // Tìm chuỗi ở giữa hai ký tự '/'
                string categoryName = href.Substring(lastSlashIndex + 2);
                return categoryName;
            }
            else
            {
                return null;
            }
        }
        public static string GetUrlKey(this DanhMuc muc)
        {
            var href = muc.href;
            string[] parts = href.Split('/');

            // Lấy phần tử thứ 3 nếu tồn tại
            if (parts.Length >= 4)
            {
                string urlKey = parts[3];
                return urlKey;
            }
            else
            {
                return null;
            }
        }
     }
}

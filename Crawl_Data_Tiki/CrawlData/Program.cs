using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Crawl_Data_Tiki.CrawlData 
{
    public class Program1
    {
        //static void ProcessItem(PreviewProduct item, int currentIndex, int totalItems)
        //{
        //    Console.CursorVisible = false;
        //    Console.Write($"\rProcessing item {currentIndex}/{totalItems}: {item}");
        //}
        static async Task Main()
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            Crawl_DanhMuc_href danhmuc = new Crawl_DanhMuc_href();
            var List = danhmuc.CrawlWithApi();
            var ListDM = await  List;
            //khai báo class xuất file xlsx
            ExportFileXlsx exportFile = new ExportFileXlsx();
            //xuat danh muc qua file xlsx
            //var danhMucPath = "crawl_danhmuc.xlsx";
            //exportFile.ExportDanhMucToFileXlsx(ListDM, danhMucPath);
            //Console.WriteLine("Xuất file Danhmuc.xlsx thành công!");
            int i = 1;
            List<PreviewProduct> listPD = new List<PreviewProduct>();
            foreach (var item in ListDM)
            {
                Crawl_product_href crawl_Product_Href = new Crawl_product_href(item);
                var dataCrawl = crawl_Product_Href.CrawlByDanhMucWithApi();
                var transformData = await dataCrawl;
                listPD.AddRange(transformData);
                if (i < 0) break;
                i--;
            }
            //var previewProductPath = "crawl_Preview_Product.xlsx";
            //exportFile.ExportPreviewProductToFileXlsx(listPD, previewProductPath);
            //Console.WriteLine("Xuất file PreviewProduct.xlsx thành công!");

            //int j = 20;
            //int index = 1;
            //int length = listPD.Count;
            List<Product> listProduct = new List<Product>();
            foreach (var item in listPD)
            {
                //ProcessItem(item, index++, length);

                CrawlDataProduct CrawProduct = new CrawlDataProduct();
                var product = await CrawProduct.CrawlProductWithApi(item);
                if(product.Id  != 0)
                    listProduct.Add(product);
                //if(j < 0) break; j--;
            }
            var productPath = "crawl_Product.xlsx";
            exportFile.ExportProductToFileXlsx(listProduct, productPath);
            MessageBox.Show("Xuất file Product.xlsx thành công!");
        }
    }
    public class ExportFileXlsx
    {
        public void ExportDanhMucToFileXlsx(List<DanhMuc> list, string filePath)
        {
            using (var package = new ExcelPackage())
            {

                // Tạo một sheet trong file Excel
                var worksheet = package.Workbook.Worksheets.Add("DanhMuc");

                // Thêm dòng tiêu đề
                worksheet.Cells["A1"].Value = "Id";
                worksheet.Cells["B1"].Value = "Title";
                worksheet.Cells["C1"].Value = "Href";

                for(int i = 0; i < list.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = list[i].id;
                    worksheet.Cells[i + 2, 2].Value = list[i].title;
                    worksheet.Cells[i + 2, 3].Value = list[i].href;
                }

                // Lưu file Excel vào đường dẫn đã chỉ định
                FileInfo excelFile = new FileInfo(filePath);
                package.SaveAs(excelFile);
            }
        }

        public void ExportPreviewProductToFileXlsx(List<PreviewProduct> list, string filePath)
        {

            using (var package = new ExcelPackage())
            {


                // Tạo một sheet trong file Excel
                var worksheet = package.Workbook.Worksheets.Add("Product");

                // Thêm dòng tiêu đề
                worksheet.Cells["A1"].Value = "DanhMuc";
                worksheet.Cells["B1"].Value = "DanhMuc ID";
                worksheet.Cells["C1"].Value = "ProductName";
                worksheet.Cells["D1"].Value = "Id";

                for (int i = 0; i < list.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = list[i].ProductType;
                    worksheet.Cells[i + 2, 2].Value = list[i].TypeId;
                    worksheet.Cells[i + 2, 3].Value = list[i].ProductName;
                    worksheet.Cells[i + 2, 4].Value = list[i].ProductId;
                }

                // Lưu file Excel vào đường dẫn đã chỉ định
                FileInfo excelFile = new FileInfo(filePath);
                package.SaveAs(excelFile);
            }
        }
        public void ExportProductToFileXlsx(List<Product> list, string filePath)
        {
            using (var package = new ExcelPackage())
            {
                // Tạo một sheet trong file Excel
                var worksheet = package.Workbook.Worksheets.Add("Product");

                // Thêm dòng tiêu đề
                worksheet.Cells["A1"].Value = "ID";
                worksheet.Cells["B1"].Value = "sku";
                worksheet.Cells["C1"].Value = "Name";
                worksheet.Cells["D1"].Value = "Short Description";
                worksheet.Cells["E1"].Value = "Price";
                worksheet.Cells["F1"].Value = "Original Price";
                worksheet.Cells["G1"].Value = "Discount";
                worksheet.Cells["H1"].Value = "Discount Rate";
                worksheet.Cells["I1"].Value = "Quantity sold";
                worksheet.Cells["J1"].Value = "Rating";
                worksheet.Cells["K1"].Value = "Review count";
                worksheet.Cells["L1"].Value = "Brand ID";
                worksheet.Cells["M1"].Value = "Brand Name";
                worksheet.Cells["N1"].Value = "Category ID";
                worksheet.Cells["O1"].Value = "Category Name";
                worksheet.Cells["P1"].Value = "Store ID";
                worksheet.Cells["Q1"].Value = "Store Name";
                worksheet.Cells["R1"].Value = "Store Is Official";
                worksheet.Cells["S1"].Value = "Store Avg Rating";
                worksheet.Cells["T1"].Value = "Store Review Count";
                worksheet.Cells["U1"].Value = "Store Total Follower";
                worksheet.Cells["V1"].Value = "Danh muc";
                worksheet.Cells["W1"].Value = "Danh muc Id";

                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Id == 0)
                    { continue; }
                    worksheet.Cells[i + 2, 1].Value = list[i].Id;
                    worksheet.Cells[i + 2, 2].Value = list[i].sku;
                    worksheet.Cells[i + 2, 3].Value = list[i].Name;
                    worksheet.Cells[i + 2, 4].Value = list[i].ShortDescription;
                    worksheet.Cells[i + 2, 5].Value = list[i].Price;
                    worksheet.Cells[i + 2, 6].Value = list[i].OriginalPrice;
                    worksheet.Cells[i + 2, 7].Value = list[i].Discount;
                    worksheet.Cells[i + 2, 8].Value = list[i].DiscountRate;
                    worksheet.Cells[i + 2, 9].Value = list[i].QuantitySold;
                    worksheet.Cells[i + 2, 10].Value = list[i].Rating;
                    worksheet.Cells[i + 2, 11].Value = list[i].ReviewCount;
                    worksheet.Cells[i + 2, 12].Value = list[i].Brand.id;
                    worksheet.Cells[i + 2, 13].Value = list[i].Brand.Name;
                    worksheet.Cells[i + 2, 14].Value = list[i].CategoriesID;
                    worksheet.Cells[i + 2, 15].Value = list[i].CategoryName;
                    worksheet.Cells[i + 2, 16].Value = list[i].Shop.StoreID;
                    worksheet.Cells[i + 2, 17].Value = list[i].Shop.Name;
                    worksheet.Cells[i + 2, 18].Value = list[i].Shop.IsOfficial;
                    worksheet.Cells[i + 2, 19].Value = list[i].Shop.AvgRating;
                    worksheet.Cells[i + 2, 20].Value = list[i].Shop.ReviewCount;
                    worksheet.Cells[i + 2, 21].Value = list[i].Shop.TotalFollower;
                    worksheet.Cells[i + 2, 22].Value = list[i].DanhMuc;
                    worksheet.Cells[i + 2, 23].Value = list[i].DanhMucId;

                }

                // Lưu file Excel vào đường dẫn đã chỉ định
                FileInfo excelFile = new FileInfo(filePath);
                package.SaveAs(excelFile);
            }
        }
    }
    public class Crawl_DanhMuc_href
    {
        public string apiUrl = "https://api.tiki.vn/raiden/v2/menu-config?platform=desktop";
        public async Task<List<DanhMuc>> CrawlWithApi()
        {
            List<DanhMuc> ListDanhMuc = new List<DanhMuc>();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agen", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36 Edg/119.0.0.0");
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {    
                        // Đọc nội dung từ phản hồi
                        string jsonContent = await response.Content.ReadAsStringAsync();

                        // Sử dụng Json.NET để chuyển đổi chuỗi JSON thành đối tượng C#
                        dynamic jsonData = JsonConvert.DeserializeObject(jsonContent);

                        ListDanhMuc = GetItems(jsonData.menu_block?.items);
                    }
                    else
                    {
                        
                    }
                }
                catch (Exception ex)
                {
                    
                }
                return ListDanhMuc;
            }
        }
        private List<DanhMuc> GetItems(dynamic itemsArray)
        {
            List<DanhMuc> result = new List<DanhMuc>();

            if (itemsArray != null)
            {
                foreach (var item in itemsArray)
                {
                    if (item.text == "NGON")
                        continue;
                    var dm = new DanhMuc
                    {
                        title = item.text,
                        href = item.link
                    };
                    dm.id = dm.GetCategory();
                    result.Add(dm);
                }
            }
            return result;
        }
        public List<DanhMuc> crawl()
        {
            List<DanhMuc> ListDanhMuc = new List<DanhMuc>();
            using(WebClient web = new WebClient())
            {
                web.Headers["User-Agen"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36 Edg/119.0.0.0";

                web.Encoding = Encoding.UTF8;
                var html = web.DownloadString("https://tiki.vn/");

                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                var elements = htmlDocument.DocumentNode.Descendants("div")
                     .FirstOrDefault(d => d.Attributes["class"]?.Value == "styles__StyledListItem-sc-w7gnxl-0 cjqkgR");

                if(elements != null)
                {
                    var danhmuc = elements.Descendants("a");

                    foreach(var item in danhmuc)
                    {
                        var dm_href = item.Attributes["href"]?.Value;
                        var dm_title = item.Attributes["title"]?.Value;
                        if (dm_title.ToUpper() == "NGON")
                            continue;
                        DanhMuc danhMuc = new DanhMuc
                        {
                            href = dm_href,
                            title = dm_title,
                        };
                        ListDanhMuc.Add(danhMuc);
                    }
                } else
                {
                    MessageBox.Show("Không có dữ liêu!");
                }
                return ListDanhMuc;
            }
        }
    }

    public class Crawl_product_href
    {
        public readonly DanhMuc _danhmuc;

        public Crawl_product_href(DanhMuc danhMuc)
        {
            _danhmuc = danhMuc;            
        }
        public int GetSizePage ()
        {
            string apiUrl = $"https://tiki.vn/api/personalish/v1/blocks/listings?" +
                $"limit=40&include=advertisement&aggregations=2&version=home-persionalized" +
                $"&trackity_id=6df93526-59aa-f488-2880-849ea5962708" +
                $"&category={_danhmuc.GetCategory()}&page=1&urlKey={_danhmuc.GetUrlKey()}";
            int sizePage = 0;
            using (WebClient web = new WebClient())
            {
                web.Headers["User-Agen"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36 Edg/119.0.0.0";

                web.Encoding = Encoding.UTF8;
                var html = web.DownloadString(_danhmuc.href);

                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                var elements = htmlDocument.DocumentNode.Descendants("a")
                     .SingleOrDefault(d => d.Attributes["class"]?.Value == "hidden-page");
                if(elements != null)
                {
                    var sizeStr = elements.Attributes["data-view-label"].Value;
                    try
                    {
                        sizePage = int.Parse(sizeStr);
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show($"Unable to parse '{sizeStr}'");
                    }
                }
                else
                {
                    sizePage = 0;
                }
                
            }
            return sizePage;
        }
        public async Task<int> GetSizePageWithApi()
        {
            string apiUrl = $"https://tiki.vn/api/personalish/v1/blocks/listings?" +
                $"limit=40&include=advertisement&aggregations=2&version=home-persionalized" +
                $"&trackity_id=6df93526-59aa-f488-2880-849ea5962708" +
                $"&category={_danhmuc.GetCategory()}&page=1&urlKey={_danhmuc.GetUrlKey()}";
            int pageSize = 0;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agen", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36 Edg/119.0.0.0");
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        // Đọc nội dung từ phản hồi
                        string jsonContent = await response.Content.ReadAsStringAsync();

                        // Sử dụng Json.NET để chuyển đổi chuỗi JSON thành đối tượng C#
                        dynamic jsonData = JsonConvert.DeserializeObject(jsonContent);

                        pageSize = jsonData.paging?.last_page;
                       
                    }
                    else
                    {
                        //Console.WriteLine($"Lỗi: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show($"Lỗi: {ex.Message}");
                }
                return pageSize;
            }
        }
        public async Task<List<PreviewProduct>> CrawlByDanhMucWithApi()
        {
            int pageSize = await GetSizePageWithApi();

            if (pageSize > 2)
               pageSize = 2;

            List<PreviewProduct> previewProducts = new List<PreviewProduct>();
            for(int i = 1; i<= pageSize; i++)
            {
                string apiUrl = $"https://tiki.vn/api/personalish/v1/blocks/listings?" +
                $"limit=40&include=advertisement&aggregations=2&version=home-persionalized" +
                $"&trackity_id=6df93526-59aa-f488-2880-849ea5962708" +
                $"&category={_danhmuc.GetCategory()}&page={i}&urlKey={_danhmuc.GetUrlKey()}";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agen", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36 Edg/119.0.0.0");
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(apiUrl);
                        if (response.IsSuccessStatusCode)
                        {
                            // Đọc nội dung từ phản hồi
                            string jsonContent = await response.Content.ReadAsStringAsync();

                            // Sử dụng Json.NET để chuyển đổi chuỗi JSON thành đối tượng C#
                            dynamic jsonData = JsonConvert.DeserializeObject(jsonContent);

                            List<PreviewProduct> items = GetItems(jsonData.data);
                            previewProducts.AddRange(items);
                        }
                        else
                        {
                                //MessageBox.Show($"Lỗi: {response.StatusCode} - {response.ReasonPhrase}");
                        }
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show($"Lỗi: {ex.Message}");
                    }
                }
            }
            
            return previewProducts;
        }

        private List<PreviewProduct> GetItems(dynamic items)
        {
            List<PreviewProduct> result = new List<PreviewProduct>();

            if (items != null)
            {
                foreach (var item in items)
                {
                    if (item.text == "NGON")
                        continue;
                    result.Add(new PreviewProduct
                    {
                        ProductId = item.id,
                        ProductName = item.name,
                        ProductType = _danhmuc.title,
                        TypeId = _danhmuc.id
                    });
                }
            }
            return result;
        }

        //public List<PreviewProduct> CrawlByPage(int page)
        //{
        //    List<PreviewProduct> ListProduct = new List<PreviewProduct>();
        //    using (WebClient web = new WebClient())
        //    {
        //        web.Headers["User-Agen"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36 Edg/119.0.0.0";

        //        web.Encoding = Encoding.UTF8;
        //        string link = _danhmuc.href + $"?page={page}";
        //        var html = web.DownloadString(link);

        //        HtmlDocument htmlDocument = new HtmlDocument();
        //        htmlDocument.LoadHtml(html);

        //        var elements = htmlDocument.DocumentNode.Descendants("a")
        //             .Where(d => d.Attributes["class"]?.Value == "style__ProductLink-sc-7xd6qw-2 fHwskZ product-item");
        //        if(elements != null)
        //        {
        //            foreach(var element in elements)
        //            {
        //                PreviewProduct product = new PreviewProduct
        //                {
        //                    //Href = element.Attributes["href"].Value,
        //                    ProductName = element.InnerText,
        //                    ProductType = _danhmuc.title
        //                };
        //                ListProduct.Add(product);
        //            }
        //        }
        //    }
        //    return ListProduct;
        //}
        //public List<PreviewProduct> CrawlByDanhMuc()
        //{
        //    List<PreviewProduct> previewProducts = new List<PreviewProduct>();

        //    Crawl_product_href crawl_Product_Href = new Crawl_product_href(_danhmuc);
        //    var pageSize = crawl_Product_Href.GetSizePage();
        //    if( pageSize > 1 ) 
        //        pageSize = 1;
        //    for (int i = 1; i <= pageSize; i++)
        //    {
        //        var dataCrawl = CrawlByPage(i);
        //        previewProducts.AddRange(dataCrawl);
        //    }
        //    return previewProducts;
        //}
    }

    public class CrawlDataProduct
    {
        public async Task<Product> CrawlProductWithApi(PreviewProduct previewProduct)
        {
            Product product = new Product();
            string apiUrl = $"https://tiki.vn/api/v2/products/{previewProduct.ProductId}?platform=web&spid={previewProduct.ProductId}&version=3";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agen", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36 Edg/119.0.0.0");
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        // Đọc nội dung từ phản hồi
                        string jsonContent = await response.Content.ReadAsStringAsync();

                        // Sử dụng Json.NET để chuyển đổi chuỗi JSON thành đối tượng C#
                        dynamic jsonData = JsonConvert.DeserializeObject(jsonContent);
                        
                        product.Id = jsonData.id;
                        product.Name = jsonData.name;
                        product.sku = jsonData.sku;
                        product.ShortDescription = jsonData.short_description;
                        product.Price = jsonData.price;
                        product.OriginalPrice = jsonData.original_price;
                        product.Discount = jsonData.discount;
                        product.DiscountRate = jsonData.discount_rate;
                        product.Rating = jsonData.rating_average;
                        product.ReviewCount = jsonData.review_count;
                        if(jsonData.quantity_sold != null)
                            product.QuantitySold = jsonData.quantity_sold.value;
                        else
                        {
                            product.QuantitySold = 0;
                        }
                        if(jsonData.categories != null)
                        {
                            product.CategoriesID = jsonData.categories.id;
                            product.CategoryName = jsonData.categories.name;
                        }
                        else
                        {
                            product.CategoriesID = -1;
                            product.CategoryName = "";
                        }
                        product.DanhMuc = previewProduct.ProductType;
                        product.DanhMucId = previewProduct.TypeId;
                        if(jsonData.brand != null)
                        {
                            product.Brand = new Brand
                            {
                                id = jsonData.brand.id,
                                Name = jsonData.brand.name
                            };
                        }
                        else
                        {
                            product.Brand = new Brand
                            {
                                id = -1,
                                Name = ""
                            };
                        }
                        
                        var newshop = new Shop
                        {
                            Id = jsonData.current_seller.id,
                            Name = jsonData.current_seller.name,
                            StoreID = jsonData.current_seller.store_id,
                        };
                        product.Shop = await CrawlDataShopWithApi(newshop);
                    }
                    else
                    {
                        //MessageBox.Show($"Lỗi: {response.StatusCode} - {response.ReasonPhrase} - product id: {previewProduct.ProductId}");
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show ($"Lỗi: {ex.Message} - product id: {previewProduct.ProductId}");
                }
                return product;
            }
        }
        public async Task<Shop> CrawlDataShopWithApi( Shop shop)
        {
            string apiUrl = $"https://api.tiki.vn/product-detail/v2/widgets/seller?seller_id={shop.Id}&trackity_id=6df93526-59aa-f488-2880-849ea5962708&platform=desktop&version=3";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agen", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36 Edg/119.0.0.0");
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        // Đọc nội dung từ phản hồi
                        string jsonContent = await response.Content.ReadAsStringAsync();

                        // Sử dụng Json.NET để chuyển đổi chuỗi JSON thành đối tượng C#
                        dynamic jsonData = JsonConvert.DeserializeObject(jsonContent);
                        
                        shop.AvgRating = jsonData.data.seller.avg_rating_point;
                        shop.TotalFollower = jsonData.data.seller.total_follower;
                        shop.ReviewCount = jsonData.data.seller.review_count;
                        shop.IsOfficial = jsonData.data.seller.is_official;
                    }
                    else
                    {
                         
                    }
                }
                catch (Exception ex)
                {
                    
                }
                return shop;
            }
        }
    }
}

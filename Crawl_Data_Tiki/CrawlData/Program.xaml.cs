using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Crawl_Data_Tiki.CrawlData
{
    /// <summary>
    /// Interaction logic for Program.xaml
    /// </summary>
    public partial class Program : Window
    {
        public Program()
        {
            InitializeComponent();
            Load();
        }
        async Task Load()
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            Crawl_DanhMuc_href danhmuc = new Crawl_DanhMuc_href();
            var List = danhmuc.CrawlWithApi();
            var ListDM = await List;

            //khai báo class xuất file xlsx
            ExportFileXlsx exportFile = new ExportFileXlsx();


            //xuat danh muc qua file xlsx
            //var danhMucPath = "crawl_danhmuc.xlsx";
            //exportFile.ExportDanhMucToFileXlsx(ListDM, danhMucPath);
            //Console.WriteLine("Xuất file Danhmuc.xlsx thành công!");

            ExitBtn.IsEnabled = false;
            NameProcess.Content = "Crawl Product ID";
            progressBar.Maximum = 2;
            progressBar.Minimum = 0;

            int i = 1;
            List<PreviewProduct> listPD = new List<PreviewProduct>();
            foreach (var item in ListDM)
            {
                Crawl_product_href crawl_Product_Href = new Crawl_product_href(item);
                var dataCrawl = crawl_Product_Href.CrawlByDanhMucWithApi();
                var transformData = await dataCrawl;
                listPD.AddRange(transformData);
                progressBar.Value++;
                if (i < 0) break;
                i--;
            }


            //var previewProductPath = "crawl_Preview_Product.xlsx";
            //exportFile.ExportPreviewProductToFileXlsx(listPD, previewProductPath);
            //Console.WriteLine("Xuất file PreviewProduct.xlsx thành công!");

            NameProcess.Content = "Crawl Product Data";
            progressBar.Maximum = listPD.Count() - 1;
            progressBar.Minimum = 0;

            //int j = 20;
            //int index = 1;
            //int length = listPD.Count;
            List<Product> listProduct = new List<Product>();
            foreach (var item in listPD)
            {
                //ProcessItem(item, index++, length);
                
                CrawlDataProduct CrawProduct = new CrawlDataProduct();
                var product = await CrawProduct.CrawlProductWithApi(item);
                if (product.Id != 0)
                    listProduct.Add(product);
                progressBar.Value++;
                //if(j < 0) break; j--;
            }


            var productPath = "crawl_Product.xlsx";
            exportFile.ExportProductToFileXlsx(listProduct, productPath);
            MessageBox.Show("Xuất file Product.xlsx thành công!");
            ExitBtn.IsEnabled = true;
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

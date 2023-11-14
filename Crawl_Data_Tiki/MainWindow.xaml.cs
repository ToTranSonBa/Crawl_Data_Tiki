using Microsoft.AspNetCore.Http;
using System.Windows;
using Crawl_Data_Tiki.UserControls;
using Crawl_Data_Tiki.ClassExtention;
using System.Runtime.CompilerServices;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Markup;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using System.Numerics;
using System.ComponentModel;
using System.Windows.Threading;

namespace Crawl_Data_Tiki
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region
        private PlanData _Jobs { get; set; }
        private string _filePath = "data.xml";
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            this.Closing += MainWindow_Closing;
            try
            {
                _Jobs = DeserializeFromXml(_filePath) as PlanData;
                _Jobs.Jobs = _Jobs.Jobs.OrderBy(e => e.CreateDate).ToList();
            }
            catch {
                SetDefautData();
            }
            userControl.Content = new DailyPlan(_Jobs, userControl);
        }

        void SetDefautData()
        {
            _Jobs = new PlanData();
            _Jobs.Jobs = new List<PlanItem>();
            _Jobs.Jobs.Add(new PlanItem
            {
                CreateDate = DateTime.Now,
                StartTime = new Point(0, 0),
                EndTime = new Point(0, 0),
                status = PlanItem.ListStatus[(int)Status.COMING],
            });
            _Jobs.Jobs.Add(new PlanItem
            {
                CreateDate = DateTime.Now,
                StartTime = new Point(0, 0),
                EndTime = new Point(0, 0),
                status = PlanItem.ListStatus[(int)Status.DONE],
            });
        }

        private void SerializeToXml(object data, string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
            XmlSerializer sr = new XmlSerializer(typeof(PlanData));
            sr.Serialize(fs, data);
            fs.Close();
        }
        private object DeserializeFromXml(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read);
            try
            {
                XmlSerializer sr = new XmlSerializer(typeof(PlanData));
                object result = sr.Deserialize(fs);
                fs.Close();
                return result;
            }
            catch (Exception ex)
            {
                fs.Close();
                throw new Exception(ex.Message);
            }
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SerializeToXml(_Jobs, _filePath);
        }


    }
}

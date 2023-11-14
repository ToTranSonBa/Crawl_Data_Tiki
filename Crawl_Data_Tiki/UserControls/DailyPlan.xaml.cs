using Crawl_Data_Tiki.ClassExtention;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Reflection.Metadata.BlobBuilder;
using Crawl_Data_Tiki.CrawlData;
using System.Numerics;

namespace Crawl_Data_Tiki.UserControls
{
    /// <summary>
    /// Interaction logic for JobUC.xaml
    /// </summary>
    public partial class DailyPlan : UserControl
    {
        public PlanData _jobs { get; set; }
        public DateTime _date { get; set; }
        public UserControl UserControl;
        #region 
        private DispatcherTimer timer;

        #endregion
        public DailyPlan(PlanData planData, UserControl UC)
        {
            InitializeComponent();
            UserControl = UC;
            _jobs = planData;
            FilterStatus.ItemsSource = PlanItem.ListStatus;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // Kiểm tra mỗi giây
            timer.Tick += Timer_Tick;
            timer.Start();

            ShowAllJob();
        }
        #region CrawlSetting

        private void Timer_Tick(object sender, EventArgs e)
        {
            var list = GetPlanByStatus(Status.COMING.ToString());
            bool plansLeft = false;
            foreach (var plan in list)
            {
                DateTime currentDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
                DateTime planDateTime = new DateTime(plan.StartDate.Year, plan.StartDate.Month, plan.StartDate.Day,
                    (int)plan.StartTime.X, (int)plan.StartTime.Y, 0);
                if (currentDateTime == planDateTime)
                {
                    plan.status = PlanItem.ListStatus[(int)Status.DOING];
                    Program pr = new Program();
                    pr.ShowDialog();
                    plan.status = PlanItem.ListStatus[(int)Status.DONE];
                    plan.EndDate = DateTime.Now;
                    plan.EndTime = new Point(DateTime.Now.Hour, DateTime.Now.Minute);
                    plansLeft = true;
                    AddJobRepeat(plan);
                }
                else if (currentDateTime > planDateTime)
                {
                    plan.status = PlanItem.ListStatus[(int)Status.MISSED];
                    plansLeft = true;
                }
            }
            // Nếu vẫn còn kế hoạch chưa được thực hiện, tạo một vòng lặp mới
            if (plansLeft)
            {

                timer.Stop(); // Dừng Timer để tránh lặp liên tục
                timer.Start(); // Khởi động Timer để bắt đầu một vòng lặp mới
            }
        }

        void AddJobRepeat(PlanItem planItem)
        {
            if(planItem.Repeat != 0)
            {
                PlanItem repeatPlan = new PlanItem
                {
                    Repeat = planItem.Repeat,
                    CreateDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    StartDate = planItem.StartDate.AddDays(planItem.Repeat),
                    StartTime = planItem.StartTime,
                    status = PlanItem.ListStatus[(int)Status.COMING]
                };
                _jobs.Jobs.Add(repeatPlan);
                Addjob(repeatPlan);
            }
        }
        #endregion
        private List<PlanItem> GetPlanByDate(DateTime date)
        {
            return _jobs.Jobs.Where(j => ExtentionMethod.IsEqualDate(j.CreateDate, date) == true).ToList();
        }

        private void ShowJobByDate(DateTime dateTime)
        {
            foreach (var item in GetPlanByDate(dateTime))
            {
                Addjob(item);
            }
        }
        private List<PlanItem> GetPlanByStatus(string Status)
        {
            return _jobs.Jobs.Where(j => j.status == Status).ToList();
        }

        private void ShowJobByStatus(string status)
        {
            foreach (var item in GetPlanByStatus(status))
            {
                Addjob(item);
            }
        }
        private void ShowAllJob()
        {
            foreach (var item in _jobs.Jobs)
            {
                Addjob(item);
            }
        }
        public void Addjob(PlanItem item)
        {
            Job job = new Job(item);
            job.Edited += Job_Edited;
            job.Deleted += Job_Deleted;
            listPlan.Children.Add(job);
        }

        private void Job_Deleted(object sender, EventArgs e)
        {
            Job uc = sender as Job;
            PlanItem job = uc._job;
            listPlan.Children.Remove(uc);
            _jobs.Jobs.Remove(job);
        }

        private void Job_Edited(object sender, EventArgs e)
        {
            if (GetPlanByStatus(Status.DONE.ToString()).Count > 0)
            {
                timer.Stop(); // Dừng Timer để tránh lặp liên tục
                timer.Start(); // Khởi động Timer để bắt đầu một vòng lặp mới
            }
        }

        private void AddJobBtn_Click(object sender, RoutedEventArgs e)
        {
            PlanItem job = new PlanItem()
            {
                StartTime = new Point(DateTime.Now.Hour, DateTime.Now.Minute + 1),
            };
            _jobs.Jobs.Add(job);
            Addjob(job);
            // Nếu vẫn còn kế hoạch chưa được thực hiện, tạo một vòng lặp mới
            if (GetPlanByStatus(Status.DONE.ToString()).Count > 0)
            {
                timer.Stop(); // Dừng Timer để tránh lặp liên tục
                timer.Start(); // Khởi động Timer để bắt đầu một vòng lặp mới
            }
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            UserControl.Content = new CalendarUC(UserControl, _jobs);
        }

        private void allBtn_Click(object sender, RoutedEventArgs e)
        {
            listPlan.Children.Clear();
            ShowAllJob();
        }

        private void FilterStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var status = (sender as ComboBox).SelectedValue.ToString();

            listPlan.Children.Clear();
            ShowJobByStatus(status);
        }

        private void createDayFilter_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var date = (sender as DatePicker).SelectedDate.Value;
            listPlan.Children.Clear();
            ShowJobByDate(date);
        }
    }
}

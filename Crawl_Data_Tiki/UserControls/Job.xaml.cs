using Crawl_Data_Tiki.ClassExtention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
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

namespace Crawl_Data_Tiki.UserControls
{
    /// <summary>
    /// Interaction logic for DailyPlan.xaml
    /// </summary>
    public partial class Job : UserControl
    {
        public PlanItem _job {  get; set; }
        public List<int> hours = Enumerable.Range(0, 23).ToList();
        public List<int> minutes = Enumerable.Range(0, 59).ToList();

        private event EventHandler edited;
        public event EventHandler Edited 
        {
            add { edited += value; } 
            remove { edited -= value; }
        }

        private event EventHandler deleted;
        public event EventHandler Deleted
        {
            add { deleted += value; }
            remove { deleted -= value; }
        }

        public Job(PlanItem job)
        {
            InitializeComponent();
            sHour.ItemsSource = hours;
            eHour.ItemsSource = hours;
            eMinute.ItemsSource = minutes;
            sMinute.ItemsSource = minutes;
            _job = job;
            showInfo();
        }

        void showInfo()
        {
            StartDay.SelectedDate = _job.StartDate;
            EndDay.SelectedDate = _job.EndDate;
            createDay.SelectedDate = _job.CreateDate;

            sMinute.Text = _job.StartTime.Y.ToString();
            sHour.Text = _job.StartTime.X.ToString();

            eMinute.Text = _job.EndTime.Y.ToString();
            eHour.Text = _job.EndTime.X.ToString();

            status.ItemsSource = PlanItem.ListStatus;
            cbRepeat.ItemsSource = PlanItem.ListRepeat;
            cbRepeat.Text = _job.Repeat.ToString();


            if (string.IsNullOrEmpty(_job.status))
                status.Text = Status.COMING.ToString();
            else 
                status.Text = _job.status; 
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            _job.status = status.SelectedValue.ToString();
            _job.Repeat = Convert.ToInt32(cbRepeat.SelectedValue);
            _job.StartTime = new Point() { X = (int)sHour.SelectedValue, Y = (int)sMinute.SelectedValue };
            _job.EndTime = new Point() { X = (int)eHour.SelectedValue, Y = (int)eMinute.SelectedValue };
            if (edited != null)
            {
                edited(this, new EventArgs());
            }
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if(deleted != null)
            {
                deleted(this, new EventArgs());
            }
        }
    }
}

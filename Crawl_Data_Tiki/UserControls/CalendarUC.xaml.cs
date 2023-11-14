using Crawl_Data_Tiki.ClassExtention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Crawl_Data_Tiki;


namespace Crawl_Data_Tiki.UserControls
{
    /// <summary>
    /// Interaction logic for CalendarUC.xaml
    /// </summary>
    public partial class CalendarUC : UserControl
    {
        #region
        private UserControl _UC {  get; set; }
        private List<List<Button>> buttonList { get; set; }
        private PlanData _Jobs { get; set; }
        private List<string> dateOfWeek = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        #endregion
        public CalendarUC(UserControl UC,PlanData job)
        {
            _Jobs = job;
            _UC = UC;
            InitializeComponent();
            LoadMatrix();
        }
        private void LoadMatrix()
        {
            buttonList = new List<List<Button>>();
            for(int i = 0; i < Cons.DayOfColumn;  i++) 
            {
                buttonList.Add(new List<Button>());
                for(int j = 0;  j < Cons.DayOfWeek; j++)
                {
                    Button button = new Button
                    {
                        Width = Cons.dateButtonWidth,
                        Height = Cons.dateButtonHeight,
                        Margin = new Thickness(5),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Background = new SolidColorBrush(Colors.White),
                    };
                    button.Click += Btn_Click;
                    matrixDay.Children.Add(button);
                    buttonList[i].Add(button);
                }
            }
            SetDefautDate(null, null);
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty((sender as Button).Content.ToString()))
                return;
            _UC.Content = new DailyPlan(
                _Jobs, _UC);
        }

        private void AddNumberDateToMatrix(DateTime dateTime)
        {
            DateTime Useday = new DateTime(dateTime.Year, dateTime.Month, 1);
            int line = 0;
            for (int i = 1; i <= DateTime.DaysInMonth(dateTime.Year, dateTime.Month); i++)
            {
                if (line > 4)
                    break;
                int column = dateOfWeek.IndexOf(Useday.DayOfWeek.ToString());
                Button btn = buttonList[line][column];
                btn.Content = i.ToString();

                if(ExtentionMethod.IsEqualDate(Useday, DateTime.Now)) {
                    btn.Background = new SolidColorBrush(Colors.Yellow);
                }

                if (ExtentionMethod.IsEqualDate(Useday, dateTime))
                {
                    btn.Background = new SolidColorBrush(Colors.Aqua);
                }

                if (column >= 6) line++;
                Useday = Useday.AddDays(1);
            }
        }



        private void datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearMatrix();
            var a = datePicker.SelectedDate.Value;
            AddNumberDateToMatrix(a);
        }

        private void ClearMatrix()
        {
            for (int i = 0; i < Cons.DayOfColumn; i++)
            {
                for (int j = 0; j < Cons.DayOfWeek; j++)
                {
                    buttonList[i][j].Content = "";
                    buttonList[i][j].Background = new SolidColorBrush(Colors.White);
                }
            }
        }


        private void SetDefautDate(object sender, RoutedEventArgs e)
        {
            datePicker.SelectedDate = DateTime.Now;
        }

        private void preMonth_Click(object sender, RoutedEventArgs e)
        {
            datePicker.SelectedDate = datePicker.SelectedDate.Value.AddMonths(-1);
        }

        private void nextMonth_Click(object sender, RoutedEventArgs e)
        {
            datePicker.SelectedDate = datePicker.SelectedDate.Value.AddMonths(1);
        }
    }
}

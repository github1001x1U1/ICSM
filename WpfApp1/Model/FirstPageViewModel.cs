using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Common;
using WpfApp1.DataAccess;

namespace WpfApp1.Model
{
    public class FirstPageViewModel : NotifyBase
    {
        private int _instrumentValue = 0;
        public int InstrumentValue
        {
            get { return _instrumentValue; }
            set { _instrumentValue = value; DoNotify(); }
        }

        public ObservableCollection<CoursServicesModel> CoursServicesList { get; set; }
            = new ObservableCollection<CoursServicesModel>();
        public FirstPageViewModel()
        {
            RefreshInitInstrumentValue();
            InitCourseSeries();
        }

        private void InitCourseSeries()
        {
            var cList = LocalDataAccess.GetInstance().GetCoursePlayRecord();
            foreach (var item in cList)
                CoursServicesList.Add(item);
        }

        private bool taskSwitch = true;
        List<Task> taskList = new List<Task>();
        private void RefreshInitInstrumentValue()
        {
            Random random = new Random();
            var task = Task.Factory.StartNew(new Action(async () =>
            {
                while (taskSwitch)
                {
                    InstrumentValue = random.Next(Math.Max(InstrumentValue - 5, 0), Math.Min(InstrumentValue + 5, 100));
                    await Task.Delay(1000);
                }
            }));
            taskList.Add(task);
        }

        public void Dispose()
        {
            try
            {
                taskSwitch = false;
                Task.WaitAll(taskList.ToArray());
            }
            catch { }
        }
    }
}

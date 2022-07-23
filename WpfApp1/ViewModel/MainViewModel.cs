using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Common;
using WpfApp1.Model;

namespace WpfApp1.ViewModel
{
    public class MainViewModel : NotifyBase
    {
        public UserModel UserInfo { get; set; }
        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; DoNotify(); }
        }

        private FrameworkElement _mainContent;

        public FrameworkElement MainContent
        {
            get { return _mainContent; }
            set { _mainContent = value; DoNotify(); }
        }

        public CommandBase NavChangedCommand { get; set; }

        public MainViewModel()
        {
            UserInfo = new UserModel();
            NavChangedCommand = new CommandBase();
            NavChangedCommand.DoExcute += new Action<object>(DoNavChanged);
            NavChangedCommand.DoCanExcute = new Func<object, bool>((o) => true);
            DoNavChanged("FirstPageView");// 打开首页
        }
        private void DoNavChanged(object obj)
        {
            Type type = Type.GetType("WpfApp1.View." + obj.ToString());
            ConstructorInfo cti = type.GetConstructor(Type.EmptyTypes);
            MainContent = cti.Invoke(null) as FrameworkElement;
        }
    }
}

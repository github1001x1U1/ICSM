using System.Windows;
using System.Windows.Input;
using WpfApp1.Common;
using WpfApp1.ViewModel;

namespace WpfApp1.View
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            MainViewModel model = new MainViewModel();
            DataContext = model;
            model.UserInfo.Avatar = GlobalValues.UserInfo.Avatar;
            model.UserInfo.UserName = GlobalValues.UserInfo.RealName;
            model.UserInfo.Gender = GlobalValues.UserInfo.Gender;
            MaxHeight = SystemParameters.PrimaryScreenHeight;// 最大化高度为主屏幕高度
        }

        private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnMax_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

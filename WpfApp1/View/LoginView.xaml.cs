using System.Windows;
using System.Windows.Input;
using WpfApp1.ViewModel;

namespace WpfApp1.View
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }
        // 拖动登录窗口
        private void WinMove_LeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}

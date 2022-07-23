using System.Windows;
using WpfApp1.View;

namespace WpfApp1
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if(new LoginView().ShowDialog() == true)
            {
                new MainView().ShowDialog();
            }
            Current.Shutdown();
        }
    }
}

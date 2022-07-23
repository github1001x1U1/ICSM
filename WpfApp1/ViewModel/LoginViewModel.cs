using System;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Common;
using WpfApp1.DataAccess;
using WpfApp1.Model;

namespace WpfApp1.ViewModel
{
    public class LoginViewModel : NotifyBase
    {
        public CommandBase CloseWindowCommand { get; set; }
        public LoginModel LoginModel { get; set; } = new LoginModel();
        public CommandBase LoginCommand { get; set; }
        // 错误信息
        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; DoNotify(); }
        }
        private Visibility _showProgress = Visibility.Collapsed;
        public Visibility ShowProgress
        {
            get { return _showProgress; }
            set
            {
                _showProgress = value; DoNotify();
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        public LoginViewModel()
        {
            CloseWindowCommand = new CommandBase();
            CloseWindowCommand.DoExcute = new Action<object>((o) =>
              {
                  (o as Window).Close();// 关闭窗口
              });
            CloseWindowCommand.DoCanExcute = new Func<object, bool>((o) => true);// 关闭命令一直可用
            // 登录逻辑
            LoginCommand = new CommandBase();
            LoginCommand.DoExcute = new Action<object>(DoLogin);
            LoginCommand.DoCanExcute = new Func<object, bool>((o) => true);// 关闭命令一直可用
        }
        // 登录逻辑方法
        private void DoLogin(object o)
        {
            ShowProgress = Visibility.Visible;
            ErrorMessage = "";// 初始消息
            if (string.IsNullOrEmpty(LoginModel.UserName))
            {
                ErrorMessage = "用户名不能为空！";
                ShowProgress = Visibility.Collapsed;
                return;
            }
            if (string.IsNullOrEmpty(LoginModel.Password))
            {
                ErrorMessage = "密码不能为空！";
                ShowProgress = Visibility.Collapsed;
                return;
            }
            if (string.IsNullOrEmpty(LoginModel.ValidationCode))
            {
                ErrorMessage = "验证码不能为空！";
                ShowProgress = Visibility.Collapsed;
                return;
            }
            if (LoginModel.ValidationCode.ToLower() != "ic8")
            {
                ErrorMessage = "验证码错误！";
                ShowProgress = Visibility.Collapsed;
                return;
            }
            Task.Run(new Action(async () =>
            {
                await Task.Delay(2000);
                try
                {
                    var user = LocalDataAccess.GetInstance().CheckUserInfo(LoginModel.UserName, LoginModel.Password);
                    if (user == null)
                        throw new Exception("登录失败！用户名或者密码错误！");
                    GlobalValues.UserInfo = user;// 全局变量
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        (o as Window).DialogResult = true;// 验证成功关闭窗口
                    });
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                }
            }));
        }
    }
}

using System;
using System.Windows.Input;

namespace WpfApp1.Common
{
    public class CommandBase : ICommand
    {
        // 事件
        public event EventHandler CanExecuteChanged;
        // 是否可执行
        public Func<object, bool> DoCanExcute { get; set; }
        public bool CanExecute(object parameter)
        {
            return DoCanExcute?.Invoke(parameter) == true;
        }
        // 执行
        public Action<object> DoExcute { get; set; }
        public void Execute(object parameter)
        {
            DoExcute?.Invoke(parameter);
        }
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}

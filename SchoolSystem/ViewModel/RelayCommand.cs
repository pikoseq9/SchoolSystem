using System;
using System.Windows.Input;

namespace SchoolSystem.ViewModel
{
    internal class RelayCommand : ICommand
    {
        private readonly Action<object> _executeWithParameter;
        private readonly Predicate<object> _canExecuteWithParameter;
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        private EventHandler _canExecuteChanged;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _executeWithParameter = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecuteWithParameter = canExecute;
        }

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_executeWithParameter != null)
                return _canExecuteWithParameter == null || _canExecuteWithParameter(parameter);
            else if (_execute != null)
                return _canExecute == null || _canExecute();
            return false;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                _canExecuteChanged += value;
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                _canExecuteChanged -= value;
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            if (_executeWithParameter != null)
                _executeWithParameter(parameter);
            else if (_execute != null)
                _execute();
        }

        public void RaiseCanExecuteChanged()
        {
            _canExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

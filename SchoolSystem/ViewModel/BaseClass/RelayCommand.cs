using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SchoolSystem.ViewModel.BaseClass
{
    internal class RelayCommand : ICommand
    {
        // Pola dla komend z parametrami
        private readonly Action<object> _executeWithParameter;
        private readonly Predicate<object> _canExecuteWithParameter;

        // Pola dla komend bez parametrów
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        // Konstruktor dla komend Z parametrem
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _executeWithParameter = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecuteWithParameter = canExecute;
        }

        // Konstruktor dla komend BEZ parametru (przeciążenie)
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            // Sprawdzamy, który konstruktor został użyty i wywołujemy odpowiednią metodę CanExecute
            if (_executeWithParameter != null)
            {
                return _canExecuteWithParameter == null || _canExecuteWithParameter(parameter);
            }
            else if (_execute != null)
            {
                return _canExecute == null || _canExecute();
            }
            return false; // To nie powinno się zdarzyć, ale dla pewności
        }

        // Event do informowania WPF o zmianach w CanExecute (np. przyciski się aktywują/dezaktywują)
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            // Sprawdzamy, który konstruktor został użyty i wywołujemy odpowiednią metodę Execute
            if (_executeWithParameter != null)
            {
                _executeWithParameter(parameter);
            }
            else if (_execute != null)
            {
                _execute();
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}

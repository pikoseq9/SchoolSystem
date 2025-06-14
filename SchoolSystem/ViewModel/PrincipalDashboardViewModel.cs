using SchoolSystem.Model;
using SchoolSystem.ViewModel.BaseClass;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SchoolSystem.ViewModel
{
    public class PrincipalDashboardViewModel : BaseViewModel
    {
        private BaseViewModel _currentViewModel;
        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }

        public ICommand ShowManageUsersCommand { get; }

        private ObservableCollection<Student> Students;

        public PrincipalDashboardViewModel(ObservableCollection<Student> students)
        {
            Students = students;
            ShowManageUsersCommand = new RelayCommand(ShowManageUsers);

            // Możesz ustawić domyślny widok, np. null lub jakiś inny
            CurrentViewModel = null;
        }

        private void ShowManageUsers()
        {
            CurrentViewModel = new ManageUsersViewModel();
        }
    }
}

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
        public ICommand ShowManageTeachersCommand { get; }
        public ICommand ShowManageClassCommand { get; }

        public PrincipalDashboardViewModel()
        {
            ShowManageUsersCommand = new RelayCommand(ShowManageUsers);
            ShowManageTeachersCommand = new RelayCommand(ShowManageTeachers);
            ShowManageClassCommand = new RelayCommand(ShowManageClass);


            CurrentViewModel = null;
        }

        private void ShowManageUsers()
        {
            CurrentViewModel = new ManageUsersViewModel();
        }
        private void ShowManageTeachers()
        {
            CurrentViewModel = new ManageTeachersViewModel();
        }
        private void ShowManageClass()
        {
            CurrentViewModel = new ManageClassViewModel();
        }

    }
}

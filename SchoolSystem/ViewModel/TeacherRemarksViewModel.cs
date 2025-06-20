using System.Collections.ObjectModel;
using System.Windows.Input;
using SchoolSystem.Helpers;
using SchoolSystem.Model;
using SchoolSystem.Repositories;
using SchoolSystem.ViewModel.BaseClass;

namespace SchoolSystem.ViewModel
{
    public class TeacherRemarksViewModel : BaseViewModel
    {
        private readonly RemarkRepository _remarkRepository;
        private int _studentId;

        public ObservableCollection<Remark> Remarks { get; set; }

        private Remark _selectedRemark;
        public Remark SelectedRemark
        {
            get => _selectedRemark;
            set
            {
                _selectedRemark = value;
                OnPropertyChanged(nameof(SelectedRemark));
                (DeleteRemarkCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand DeleteRemarkCommand { get; }
        public ICommand AddRemarkCommand { get; }

        public TeacherRemarksViewModel(int studentId)
        {
            _studentId = studentId;
            _remarkRepository = new RemarkRepository();
            Remarks = _remarkRepository.GetRemarksWithTeacherName(studentId);

            DeleteRemarkCommand = new RelayCommand(_ => DeleteRemark(), _ => SelectedRemark != null);
            AddRemarkCommand = new RelayCommand(_ => OpenAddRemarkWindow());
        }

        private void DeleteRemark()
        {
            if (SelectedRemark != null)
            {
                _remarkRepository.DeleteRemark(SelectedRemark.Id);
                Remarks.Remove(SelectedRemark);
                SelectedRemark = null;
            }
        }

        private void OpenAddRemarkWindow()
        {
            var window = new SchoolSystem.View.AddRemarkWindow();
            var vm = new AddRemarkViewModel(_studentId, Session.CurrentTeacher.Id, () => window.Close());
            window.DataContext = vm;
            window.ShowDialog();

            Remarks = _remarkRepository.GetRemarksWithTeacherName(_studentId);
            OnPropertyChanged(nameof(Remarks));
        }
    }
}

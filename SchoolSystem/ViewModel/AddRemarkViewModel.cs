using SchoolSystem.Model;
using SchoolSystem.Repositories;
using SchoolSystem.ViewModel.BaseClass;
using System;
using System.Windows;
using System.Windows.Input;

namespace SchoolSystem.ViewModel
{
    public class AddRemarkViewModel : BaseViewModel
    {
        public string RemarkText { get; set; }

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }

        private readonly int _studentId;
        private readonly int _teacherId;
        private readonly Action _closeWindow;

        public AddRemarkViewModel(int studentId, int teacherId, Action closeWindow)
        {
            _studentId = studentId;
            _teacherId = teacherId;
            _closeWindow = closeWindow;

            ConfirmCommand = new RelayCommand(_ => SaveRemark());
            CancelCommand = new RelayCommand(_ => _closeWindow());
        }

        private void SaveRemark()
        {
            if (string.IsNullOrWhiteSpace(RemarkText))
            {
                MessageBox.Show("Treść uwagi nie może być pusta.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var remark = new Remark
            {
                StudentID = _studentId,
                TeacherID = _teacherId,
                Value = RemarkText
            };

            var repo = new RemarkRepository();
            repo.InsertRemark(remark);

            _closeWindow();
        }
    }
}

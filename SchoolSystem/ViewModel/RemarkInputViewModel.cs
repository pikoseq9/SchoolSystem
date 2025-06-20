using SchoolSystem.Model;
using SchoolSystem.ViewModel.BaseClass;
using System;
using System.Windows.Input;

namespace SchoolSystem.ViewModel
{
    public class RemarkInputViewModel : BaseViewModel
    {
        public string Value { get; set; }

        public event Action<Remark> RemarkConfirmed;

        private readonly int _studentId;
        private readonly int _teacherId;

        public ICommand ConfirmCommand { get; }

        public RemarkInputViewModel(int studentId, int teacherId)
        {
            _studentId = studentId;
            _teacherId = teacherId;

            ConfirmCommand = new RelayCommand(_ => ConfirmRemark());
        }

        private void ConfirmRemark()
        {
            RemarkConfirmed?.Invoke(new Remark
            {
                StudentID = _studentId,
                TeacherID = _teacherId,
                Value = this.Value
            });
        }
    }
}

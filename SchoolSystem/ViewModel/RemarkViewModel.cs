using SchoolSystem.Model;
using SchoolSystem.Repositories;
using SchoolSystem.ViewModel.BaseClass;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace SchoolSystem.ViewModel
{
    public class RemarkViewModel : BaseViewModel
    {
        private ObservableCollection<Remark>? _remarks;
        private readonly RemarkRepository _remarkRepository;
        private readonly TeacherRepository _teacherRepository;
        private int _studentId;

        public ObservableCollection<Remark>? Remarks
        {
            get {return _remarks; }
            set
            {
                _remarks = value;
                OnPropertyChanged(nameof(Remarks));
            }
        }

        public RemarkViewModel(int studentId)
        {
            _studentId = studentId;
            _teacherRepository = new TeacherRepository();
            _remarkRepository = new RemarkRepository(_teacherRepository);
            _remarks = new ObservableCollection<Remark>();
            try
            {
                _remarks = _remarkRepository.GetAllRemarksByStudentId(_studentId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd przy pobieraniu uwag: {ex.Message}");
            }
        }

        public RemarkViewModel() : this(0) // Calls the parameterized constructor with a default ID (0 or -1, something invalid)
        {
        }
    }
}
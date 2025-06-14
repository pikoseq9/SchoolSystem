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

        public RemarkViewModel()
        {
            _teacherRepository = new TeacherRepository();
            _remarkRepository = new RemarkRepository(_teacherRepository);
            _remarks = new ObservableCollection<Remark>();
            _studentId = 11; // Testowe ID studenta, do usunięcia po implementacji logowania
            try
            {
                _remarks = _remarkRepository.GetAllRemarksByStudentId(_studentId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd przy pobieraniu uwag: {ex.Message}");
            }
        }
    }
}
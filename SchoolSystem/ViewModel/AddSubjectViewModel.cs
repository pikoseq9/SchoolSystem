using SchoolSystem.Model;
using SchoolSystem.Repositories;
using SchoolSystem.ViewModel.BaseClass;
using System;
using System.Windows;
using System.Windows.Input;

namespace SchoolSystem.ViewModel
{
    public class AddSubjectViewModel : BaseViewModel
    {
        private Subject _subject = new Subject();
        private readonly SubjectRepository _subjectRepository = new SubjectRepository();

        public string Nazwa
        {
            get => _subject.Name ?? string.Empty;
            set { _subject.Name = value; OnPropertyChanged(nameof(Nazwa)); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddSubjectViewModel()
        {
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Nazwa))
            {
                MessageBox.Show("Nazwa przedmiotu nie może być pusta.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_subjectRepository.SubjectNameExists(Nazwa))
            {
                MessageBox.Show("Przedmiot o takiej nazwie już istnieje.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                _subjectRepository.AddSubject(_subject);
                CloseRequested?.Invoke(this, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd dodawania przedmiotu: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Cancel()
        {
            CloseRequested?.Invoke(this, false);
        }

        public event EventHandler<bool>? CloseRequested;
    }
}

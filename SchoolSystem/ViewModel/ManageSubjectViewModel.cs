using SchoolSystem.Model;
using SchoolSystem.Repositories;
using SchoolSystem.View;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System;
using SchoolSystem.ViewModel.BaseClass;
using SchoolSystem.ViewModel;

namespace SchoolSystem.ViewModel
{
    public class ManageSubjectViewModel : BaseViewModel
    {
        private readonly SubjectRepository _subjectRepository = new SubjectRepository();

        private ObservableCollection<Subject> _subjects = new ObservableCollection<Subject>();
        public ObservableCollection<Subject> Subjects
        {
            get => _subjects;
            set
            {
                _subjects = value;
                OnPropertyChanged(nameof(Subjects));
            }
        }

        private Subject? _selectedSubject;
        public Subject? SelectedSubject
        {
            get => _selectedSubject;
            set
            {
                _selectedSubject = value;
                OnPropertyChanged(nameof(SelectedSubject));
                CommandManager.InvalidateRequerySuggested(); // Aktualizacja dostępności komend
            }
        }

        public ICommand AddSubjectCommand { get; }
        public ICommand EditSubjectCommand { get; }
        public ICommand DeleteSubjectCommand { get; }

        public ManageSubjectViewModel()
        {
            AddSubjectCommand = new RelayCommand(AddSubject);
            EditSubjectCommand = new RelayCommand(EditSubject, () => SelectedSubject != null);
            DeleteSubjectCommand = new RelayCommand(DeleteSubject, () => SelectedSubject != null);

            LoadSubjects();
        }

        private void LoadSubjects()
        {
            try
            {
                var subjectsFromDb = _subjectRepository.GetAllSubjects();
                Subjects = new ObservableCollection<Subject>(subjectsFromDb);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd ładowania przedmiotów: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddSubject()
        {
            var vm = new AddSubjectViewModel();
            var view = new AddSubjectView { DataContext = vm };

            var window = new Window
            {
                Title = "Dodaj przedmiot",
                Content = view,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };

            vm.CloseRequested += (s, e) =>
            {
                if (e) LoadSubjects();
                window.DialogResult = e;
                window.Close();
            };

            window.ShowDialog();
        }

        private void EditSubject()
        {
            if (SelectedSubject == null) return;

            // Pobierz świeże dane przedmiotu (opcjonalnie)
            var subjectFromDb = _subjectRepository.GetSubjectById(SelectedSubject.Id);
            if (subjectFromDb == null)
            {
                MessageBox.Show("Wybrany przedmiot nie istnieje już w bazie.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                LoadSubjects();
                return;
            }

            var vm = new EditSubjectViewModel(subjectFromDb);
            var view = new EditSubjectView { DataContext = vm };

            var window = new Window
            {
                Title = "Edytuj przedmiot",
                Content = view,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };

            vm.CloseRequested += (s, e) =>
            {
                if (e)
                {
                    try
                    {
                        _subjectRepository.UpdateSubject(vm.Subject);
                        LoadSubjects();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Błąd aktualizacji przedmiotu: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                window.DialogResult = e;
                window.Close();
            };

            window.ShowDialog();
        }

        private void DeleteSubject()
        {
            if (SelectedSubject == null)
                return;

            if (MessageBox.Show($"Czy na pewno chcesz usunąć przedmiot '{SelectedSubject.Name}'?",
                "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    bool deleted = _subjectRepository.DeleteSubject(SelectedSubject.Id);

                    if (deleted)
                    {
                        Subjects.Remove(SelectedSubject);
                    }
                    else
                    {
                        MessageBox.Show("Nie można usunąć przedmiotu, ponieważ jest on powiązany z innymi rekordami.",
                            "Błąd usuwania", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd usuwania przedmiotu: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
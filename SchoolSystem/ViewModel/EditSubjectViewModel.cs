using SchoolSystem.Model;
using SchoolSystem.Repositories;
using SchoolSystem.ViewModel.BaseClass;
using System;
using System.Windows;
using System.Windows.Input;

public class EditSubjectViewModel : BaseViewModel
{
    private readonly Subject _subject;
    private readonly SubjectRepository _subjectRepository = new SubjectRepository();

    public string Nazwa
    {
        get => _subject.Name ?? string.Empty;
        set { _subject.Name = value; OnPropertyChanged(nameof(Nazwa)); }
    }

    public Subject Subject => _subject;

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public EditSubjectViewModel(Subject subject)
    {
        _subject = subject ?? throw new ArgumentNullException(nameof(subject));
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

        if (_subjectRepository.SubjectNameExistsExceptId(Nazwa, _subject.Id))
        {
            MessageBox.Show("Przedmiot o takiej nazwie już istnieje.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            _subjectRepository.UpdateSubject(_subject);
            CloseRequested?.Invoke(this, true);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Błąd aktualizacji przedmiotu: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }


    private void Cancel()
    {
        CloseRequested?.Invoke(this, false);
    }

    public event EventHandler<bool>? CloseRequested;
}

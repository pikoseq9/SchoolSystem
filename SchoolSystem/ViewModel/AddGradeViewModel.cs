using SchoolSystem.Model;
using SchoolSystem.ViewModel.BaseClass;
using System.Windows.Input;
using System.Windows;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using SchoolSystem.Repositories;

public class AddGradeViewModel : BaseViewModel
{
    public int StudentID { get; set; }

    public int SubjectID { get; set; }
    public DateTime Date { get; set; } = DateTime.Today;
    public decimal Value { get; set; } = 3;
    public string Category { get; set; } = "Sprawdzian";
    public int Weight { get; set; } = 1;

    public ICommand ConfirmCommand { get; }

    public event Action<Grade> GradeConfirmed;
    public ObservableCollection<Subject> Subjects { get; }
    public Subject SelectedSubject { get; set; }

    public AddGradeViewModel(int studentId)
    {
        StudentID = studentId;

        var repo = new SubjectRepository();
        Subjects = repo.GetAllSubjects();

        SelectedSubject = Subjects.FirstOrDefault();

        ConfirmCommand = new RelayCommand(_ => Confirm());
    }


    private void Confirm()
    {
        if (SelectedSubject == null) return;

        var grade = new Grade
        {
            StudentID = StudentID,
            SubjectID = SelectedSubject.Id,
            Date = Date,
            Value = Value,
            Category = Category,
            Weight = Weight
        };



        GradeConfirmed?.Invoke(grade);
        CloseWindow();
    }


    private void CloseWindow()
    {
        Application.Current.Windows
            .OfType<Window>()
            .SingleOrDefault(w => w.DataContext == this)
            ?.Close();
    }
}

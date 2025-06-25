using SchoolSystem.Model;
using SchoolSystem.Repositories;
using SchoolSystem.ViewModel.BaseClass;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using SchoolSystem.View;
using System;

public class AddLessonViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public ObservableCollection<Subject> Subjects { get; set; }
    public ObservableCollection<Room> Rooms { get; set; }
    public ObservableCollection<string> DaysOfWeek { get; set; }
    public ObservableCollection<string> AvailableStartTimes { get; set; }
    public ObservableCollection<Teacher> Teachers { get; set; }
    public Teacher SelectedTeacher { get; set; }

    public Subject SelectedSubject { get; set; }
    public Room SelectedRoom { get; set; }
    public string SelectedDay { get; set; }
    public string StartTime { get; set; } = "08:00";
    public int Duration { get; set; } = 45;

    public ICommand SaveCommand { get; }

    private int _teacherId;
    private int _classId;
    private readonly ScheduleRepository _scheduleRepo;

    public AddLessonViewModel(int teacherId, int classId)
    {
        _teacherId = teacherId;
        _classId = classId;
        _scheduleRepo = new ScheduleRepository();

        Subjects = new ObservableCollection<Subject>(new SubjectRepository().GetAllSubjects());
        Rooms = new ObservableCollection<Room>(new RoomRepository().GetAllRooms());
        DaysOfWeek = new ObservableCollection<string> { "Poniedziałek", "Wtorek", "Środa", "Czwartek", "Piątek" };
        AvailableStartTimes = new ObservableCollection<string>
        {
            "08:00", "09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00"
        };
        Teachers = new ObservableCollection<Teacher>(new TeacherRepository().GetAllTeachers());

        SaveCommand = new RelayCommand(SaveLesson);
    }

    private void SaveLesson()
    {
        if (SelectedSubject == null || SelectedRoom == null || string.IsNullOrWhiteSpace(SelectedDay) || string.IsNullOrWhiteSpace(StartTime))
        {
            MessageBox.Show("Wszystkie pola muszą być wypełnione!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!TimeSpan.TryParse(StartTime, out TimeSpan parsedStartTime))
        {
            MessageBox.Show("Godzina rozpoczęcia musi być w formacie HH:mm (np. 08:00)", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (Duration <= 0)
        {
            MessageBox.Show("Czas trwania musi być większy od 0 minut.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        bool overlapping = _scheduleRepo.IsLessonOverlapping(
            classId: _classId,
            dayOfWeek: SelectedDay,
            startTime: StartTime,
            duration: Duration
        );

        if (overlapping)
        {
            MessageBox.Show("Lekcja koliduje z innymi zajęciami tej klasy!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var lesson = new Lesson(
            id: 0,
            roomID: SelectedRoom.ID,
            subjectID: SelectedSubject.Id,
            classID: _classId,
            teacherID: SelectedTeacher?.Id ?? _teacherId,
            dayOfWeek: SelectedDay,
            startTime: StartTime,
            duration: Duration
        );

        _scheduleRepo.AddLesson(lesson);
        MessageBox.Show("Lekcja dodana pomyślnie!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
        CloseWindow();
    }


    private void CloseWindow()
    {
        foreach (Window window in Application.Current.Windows)
        {
            if (window is AddLessonWindow)
            {
                window.Close();
                break;
            }
        }
    }
}

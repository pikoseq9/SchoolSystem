using SchoolSystem.Model;
using SchoolSystem.Repositories;
using System.Collections.ObjectModel;

namespace SchoolSystem.ViewModel
{
    public class LessonsViewModel
    {
        public ObservableCollection<Lesson> Lessons { get; }

        public LessonsViewModel(int teacherId)
        {
            var repo = new LessonRepository();
            Lessons = repo.GetLessonsByTeacherId(teacherId);
        }
    }
}

using SchoolSystem.Model;
using SchoolSystem.Repositories;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.Sqlite;
using System.IO;
using SchoolSystem.View;

namespace SchoolSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            TestDatabaseConnectionAndDataLoad();
        }

        private void TestDatabaseConnectionAndDataLoad()
        {
            StudentRepository repository = new StudentRepository();
            TeacherRepository teacherRepository = new TeacherRepository();
            Graderepository graderepository = new Graderepository();
            List<Student> students = new List<Student>();
            List<Teacher> teachers = new List<Teacher>();
            List<Grade> grades = new List<Grade>();

        }
    }
}
using SchoolSystem.Model;
using SchoolSystem.Repositories;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using SchoolSystem.ViewModel;
using System.Windows.Data;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.Sqlite;
using System;

using System.IO;

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
            StudentRepository studentrepository = new StudentRepository();
            Graderepository graderepository = new Graderepository();
            List<Student>? students = new List<Student>();
            List<Grade>? grades = new List<Grade>();

            grades = graderepository.GetAllGrades();
            students = studentrepository.GetAllStudents();
            MessageBox.Show($"{grades.Count}");
            MessageBox.Show($"{students.Count}");

        }
    }
}
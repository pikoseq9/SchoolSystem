using Microsoft.Data.Sqlite;
using SchoolSystem.Model;
using SchoolSystem.Repositories;
using SchoolSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices.Marshalling;
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
            Remarkrepository remarkrepository = new Remarkrepository();
            ObservableCollection<Student>? students = new ObservableCollection<Student>();
            ObservableCollection<Grade>? grades = new ObservableCollection<Grade>();
            ObservableCollection<Remark>? remarks = new ObservableCollection<Remark>();

            grades = graderepository.GetAllGrades();
            students = studentrepository.GetAllStudentsByClassId(1);
            remarks = remarkrepository.GetAllRemarksByStudentId(1);
            MessageBox.Show(students[0].Name + students[0].SurName + students[0].DateOfBirth);
            
            //MessageBox.Show($"{grades.Count}");
            //MessageBox.Show($"{students.Count}");

        }
    }
}
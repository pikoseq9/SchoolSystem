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
            DataContext = new MainViewModel();

            HashAllStudentPasswords();
            HashAllTeacherPasswords();
        }

        public static void HashAllStudentPasswords()
        {
            var repo = new StudentRepository();
            var students = repo.GetAllStudents();

            if (students.Count == 0)
            {
                MessageBox.Show("Brak uczniów w bazie.");
                return;
            }

            int updatedCount = 0;

            using (var connection = new SqliteConnection("Data Source=szkola.db"))
            {
                connection.Open();

                foreach (var student in students)
                {
                    var plainPassword = student.Password;

                    // Pomijamy brak hasła lub już wyglądające na hash
                    if (string.IsNullOrWhiteSpace(plainPassword) || plainPassword.Length > 20)
                        continue;

                    var hashedPassword = PasswordHelper.HashPassword(plainPassword);

                    string updateQuery = "UPDATE Uczniowie SET Haslo = @hashed WHERE ID_Uczen = @id";
                    using (var command = new SqliteCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@hashed", hashedPassword);
                        command.Parameters.AddWithValue("@id", student.Id);

                        int rows = command.ExecuteNonQuery();
                        if (rows > 0)
                            updatedCount++;
                    }
                }
            }

            MessageBox.Show($"Zakończono. Zahashowano {updatedCount} hasł(a/u).");
        }

        public static void HashAllTeacherPasswords()
        {
            var repo = new TeacherRepository();
            var teachers = repo.GetAllTeachers();

            if (teachers.Count == 0)
            {
                MessageBox.Show("Brak nauczycieli w bazie.");
                return;
            }

            int updatedCount = 0;

            using (var connection = new SqliteConnection("Data Source=szkola.db"))
            {
                connection.Open();

                foreach (var teacher in teachers)
                {
                    var plainPassword = teacher.Password;

                    // Pomijamy brak hasła lub już wyglądające na hash
                    if (string.IsNullOrWhiteSpace(plainPassword) || plainPassword.Length > 20)
                        continue;

                    var hashedPassword = PasswordHelper.HashPassword(plainPassword);

                    string updateQuery = "UPDATE Nauczyciele SET Haslo = @hashed WHERE ID_Nauczyciel = @id";
                    using (var command = new SqliteCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@hashed", hashedPassword);
                        command.Parameters.AddWithValue("@id", teacher.Id);

                        int rows = command.ExecuteNonQuery();
                        if (rows > 0)
                            updatedCount++;
                    }
                }
            }

            MessageBox.Show($"Zakończono. Zahashowano {updatedCount} hasł(a/u) nauczycieli.");
        }

    }
}
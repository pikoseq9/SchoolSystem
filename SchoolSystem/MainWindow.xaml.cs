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

            // Wywołaj metodę testową po inicjalizacji komponentów okna
            TestDatabaseConnectionAndDataLoad();
        }

        private void TestDatabaseConnectionAndDataLoad()
        {
            StudentRepository repository = new StudentRepository();
            List<Student> students = new List<Student>();

            try
            {
                students = repository.GetAllStudents();

                if (students.Count > 0)
                {
                    // Jeśli udało się pobrać dane, wyświetl komunikat i kilka danych
                    MessageBox.Show($"Pomyślnie załadowano {students.Count} uczniów z bazy danych.", "Sukces!", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Możesz też wypisać dane do konsoli Visual Studio (Output Window)
                    Console.WriteLine("\n--- Załadowani uczniowie ---");
                    foreach (var student in students)
                    {
                        Console.WriteLine($"ID: {student.Id}, Imię: {student.Name}, Nazwisko: {student.SurName}, KlasaID: {student.ClassID}, PESEL: {student.PESEL}");
                    }
                    Console.WriteLine("---------------------------\n");

                    // Przykład: wyświetl imię i nazwisko pierwszego studenta (jeśli istnieje)
                    MessageBox.Show($"Pierwszy uczeń: {students[0].Name} {students[0].SurName}", "Dane ucznia", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Połączono z bazą danych, ale nie znaleziono żadnych uczniów w tabeli 'Uczniowie'. Upewnij się, że tabela zawiera dane.", "Brak danych", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                // Jeśli wystąpi błąd, wyświetl go
                MessageBox.Show($"Wystąpił błąd podczas próby połączenia z bazą danych lub ładowania danych:\n\n{ex.Message}", "Błąd Bazy Danych", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine($"DEBUG: Pełny wyjątek: {ex.ToString()}"); // Wypisz pełny stos wywołań dla debugowania
            }
        }
    }
}
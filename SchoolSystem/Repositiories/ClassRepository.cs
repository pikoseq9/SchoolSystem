using System;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Data.Sqlite;
using SchoolSystem.Model;

namespace SchoolSystem.Repositories
{
    public class ClassRepository
    {
        private string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "szkola.db");

        public ObservableCollection<Class> GetAllClasses()
        {
            ObservableCollection<Class> classes = new ObservableCollection<Class>();

            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT ID_Klasa, Kod, Wychowawca_ID FROM Klasy";

                    using (SqliteCommand command = new SqliteCommand(query, connection))
                    {
                        using (SqliteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                classes.Add(new Class(
                                    code: reader.GetString(reader.GetOrdinal("Kod")),
                                    classTeacherID: reader.GetInt32(reader.GetOrdinal("Wychowawca_ID")),
                                    id: reader.GetInt32(reader.GetOrdinal("ID_Klasa"))
                                ));
                            }
                        }
                    }
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine($"Błąd bazy danych: {ex.Message}");
                    throw new Exception("Nie udało się pobrać danych klas z bazy.", ex);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił nieoczekiwany błąd: {ex.Message}");
                    throw new Exception("Wystąpił nieoczekiwany błąd podczas pobierania danych klas.", ex);
                }
            }

            return classes;
        }
    }
}

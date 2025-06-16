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

                    string query = @"
                SELECT 
                    k.ID_Klasa, 
                    k.Kod, 
                    k.Wychowawca_ID, 
                    n.Imie || ' ' || n.Nazwisko AS Nauczyciel
                FROM Klasy k
                LEFT JOIN Nauczyciele n ON k.Wychowawca_ID = n.ID_Nauczyciel";

                    using (SqliteCommand command = new SqliteCommand(query, connection))
                    {
                        using (SqliteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                classes.Add(new Class(
                                    code: reader.GetString(reader.GetOrdinal("Kod")),
                                    classTeacherID: reader.GetInt32(reader.GetOrdinal("Wychowawca_ID")),
                                    id: reader.GetInt32(reader.GetOrdinal("ID_Klasa")),
                                    teacherFullName: reader.IsDBNull(reader.GetOrdinal("Nauczyciel"))
                                                     ? "Brak danych"
                                                     : reader.GetString(reader.GetOrdinal("Nauczyciel"))
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

        public void AddClass(Class newClass)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                string query = @"INSERT INTO Klasy (Kod, Wychowawca_ID)
                         VALUES (@Code, @TeacherId)";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Code", newClass.Code ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@TeacherId", newClass.ClassTeacherID);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateClass(Class updatedClass)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                string query = @"UPDATE Klasy
                         SET Kod = @Code,
                             ID_Wychowawca = @TeacherId
                         WHERE ID_Klasa = @Id";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Code", (object)updatedClass.Code ?? DBNull.Value);
                    command.Parameters.AddWithValue("@TeacherId", updatedClass.ClassTeacherID);
                    command.Parameters.AddWithValue("@Id", updatedClass.Id);

                    command.ExecuteNonQuery();
                }
            }
        }


        public void EditClass(Class updatedClass)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                string query = @"UPDATE Klasy
                         SET Kod = @Code,
                             Wychowawca_ID = @TeacherId
                         WHERE ID_Klasa = @Id";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Code", updatedClass.Code ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@TeacherId", updatedClass.ClassTeacherID);
                    command.Parameters.AddWithValue("@Id", updatedClass.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteClass(int classId)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                string query = "DELETE FROM Klasy WHERE ID_Klasa = @Id";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", classId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

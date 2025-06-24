using Microsoft.Data.Sqlite;
using SchoolSystem.Model;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace SchoolSystem.Repositories
{
    public class ClassRepository
    {
        private readonly string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "szkola.db");

        public ObservableCollection<Class> GetAllClasses()
        {
            var classes = new ObservableCollection<Class>();

            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
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

                    using (var command = new SqliteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
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
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] {ex.Message}");
                    throw new Exception("Błąd podczas pobierania wszystkich klas.", ex);
                }
            }

            return classes;
        }

        public ObservableCollection<Class> GetClassByClassId(int classId)
        {
            var classes = new ObservableCollection<Class>();

            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
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
                        LEFT JOIN Nauczyciele n ON k.Wychowawca_ID = n.ID_Nauczyciel
                        WHERE k.ID_Klasa = @Id";

                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", classId);

                        using (var reader = command.ExecuteReader())
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
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] {ex.Message}");
                    throw new Exception("Błąd podczas pobierania klasy po ID.", ex);
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
        public bool ClassCodeExists(string code)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Klasy WHERE Kod = @Code";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Code", code);
                    long count = (long)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public bool CanDeleteClass(int classId)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Uczniowie WHERE Klasa_ID = @ClassId";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClassId", classId);
                    long count = (long)command.ExecuteScalar();
                    return count == 0; // można usunąć tylko jeśli brak uczniów
                }
            }
        }

        public bool DeleteClass(int classId)
        {
            if (!CanDeleteClass(classId))
                return false;

            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                string query = "DELETE FROM Klasy WHERE ID_Klasa = @Id";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", classId);
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }
    }
}

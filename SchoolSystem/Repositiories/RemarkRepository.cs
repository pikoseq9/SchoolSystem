using Microsoft.Data.Sqlite;
using SchoolSystem.Model;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace SchoolSystem.Repositories
{
    public class RemarkRepository
    {
        private readonly string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "szkola.db");
        private readonly TeacherRepository _teacherRepository;

        public RemarkRepository(TeacherRepository? teacherRepository = null)
        {
            _teacherRepository = teacherRepository ?? new TeacherRepository();
        }

        /// <summary>
        /// Pobiera uwagi z bazy danych z użyciem JOIN (szybko, bez zewnętrznego repozytorium)
        /// </summary>
        public ObservableCollection<Remark> GetRemarksWithTeacherName(int studentId)
        {
            var remarks = new ObservableCollection<Remark>();

            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT u.ID_Uwaga, u.Tresc, n.Imie || ' ' || n.Nazwisko AS Wystawiajacy
                FROM Uwagi u
                JOIN Nauczyciele n ON u.Wystawiajacy_ID = n.ID_Nauczyciel
                WHERE u.Uczen_ID = @StudentId";

            command.Parameters.AddWithValue("@StudentId", studentId);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                remarks.Add(new Remark
                {
                    Id = reader.GetInt32(0),
                    Value = reader.GetString(1),
                    TeacherName = reader.GetString(2)
                });
            }

            return remarks;
        }

        /// <summary>
        /// Alternatywna wersja: Pobiera uwagi z repozytorium nauczyciela (gdy potrzebujesz jego klasy lub danych kontaktowych itp.)
        /// </summary>
        public ObservableCollection<Remark> GetAllRemarksByStudentId(int studentId)
        {
            var remarks = new ObservableCollection<Remark>();

            using var connection = new SqliteConnection($"Data Source={dbPath}");
            try
            {
                connection.Open();

                string query = "SELECT ID_Uwaga, Uczen_ID, Wystawiajacy_ID, Tresc FROM Uwagi WHERE Uczen_ID = @Id";
                using var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", studentId);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int teacherId = reader.GetInt32(reader.GetOrdinal("Wystawiajacy_ID"));
                    string teacherFullName = "Nieznany Nauczyciel";

                    var teacher = _teacherRepository.GetTeacherById(teacherId);
                    if (teacher != null)
                        teacherFullName = $"{teacher.Name} {teacher.SurName}";

                    remarks.Add(new Remark(
                        id: reader.GetInt32(reader.GetOrdinal("ID_Uwaga")),
                        studentID: reader.GetInt32(reader.GetOrdinal("Uczen_ID")),
                        teacherID: teacherId,
                        value: reader.GetString(reader.GetOrdinal("Tresc")),
                        teacherFullName: teacherFullName
                    ));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Błąd podczas pobierania uwag: {ex.Message}");
                throw;
            }

            return remarks;
        }

        public Remark? GetRemarkById(int remarkId)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            try
            {
                connection.Open();
                string query = "SELECT ID_Uwaga, Uczen_ID, Wystawiajacy_ID, Tresc FROM Uwagi WHERE ID_Uwaga = @Id";

                using var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", remarkId);

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    int teacherId = reader.GetInt32(reader.GetOrdinal("Wystawiajacy_ID"));
                    string teacherFullName = "Nieznany Nauczyciel";

                    var teacher = _teacherRepository.GetTeacherById(teacherId);
                    if (teacher != null)
                        teacherFullName = $"{teacher.Name} {teacher.SurName}";

                    return new Remark(
                        id: reader.GetInt32(reader.GetOrdinal("ID_Uwaga")),
                        studentID: reader.GetInt32(reader.GetOrdinal("Uczen_ID")),
                        teacherID: teacherId,
                        value: reader.GetString(reader.GetOrdinal("Tresc")),
                        teacherFullName: teacherFullName
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetRemarkById: {ex.Message}");
            }

            return null;
        }

        public void InsertRemark(Remark remark)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Uwagi (Uczen_ID, Wystawiajacy_ID, Tresc)
                VALUES (@UczenID, @TeacherID, @Tresc)";

            command.Parameters.AddWithValue("@UczenID", remark.StudentID);
            command.Parameters.AddWithValue("@TeacherID", remark.TeacherID);
            command.Parameters.AddWithValue("@Tresc", remark.Value);

            command.ExecuteNonQuery();
        }

        public void DeleteRemark(int remarkId)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Uwagi WHERE ID_Uwaga = @Id";
            command.Parameters.AddWithValue("@Id", remarkId);
            command.ExecuteNonQuery();
        }
    }
}

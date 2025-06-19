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

        public ObservableCollection<Remark> GetRemarksWithTeacherName(int studentId)
        {
            var remarks = new ObservableCollection<Remark>();

            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT u.ID_Uwaga, u.Tresc, n.Imie || ' ' || n.Nazwisko AS Wystawiajacy
                    FROM Uwagi u
                    JOIN Nauczyciele n ON u.Wystawiajacy_ID = n.ID_Nauczyciel
                    WHERE u.Uczen_ID = @StudentId";
                command.Parameters.AddWithValue("@StudentId", studentId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        remarks.Add(new Remark
                        {
                            Id = reader.GetInt32(0),
                            Value = reader.GetString(1),
                            TeacherName = reader.GetString(2)
                        });
                    }
                }
            }

            return remarks;
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

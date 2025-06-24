using Microsoft.Data.Sqlite;
using SchoolSystem.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace SchoolSystem.Repositories
{
    public class SubjectRepository
    {
        private readonly string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "szkola.db");

        /// <summary>
        /// Dla prostego użycia w logice biznesowej
        /// </summary>
        public List<Subject> GetAllSubjectsAsList()
        {
            var subjects = new List<Subject>();

            using var connection = new SqliteConnection($"Data Source={dbPath}");
            try
            {
                connection.Open();

                string query = "SELECT ID_Przedmiot, Nazwa FROM Przedmioty";
                using var command = new SqliteCommand(query, connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    subjects.Add(new Subject(
                        id: reader.GetInt32(reader.GetOrdinal("ID_Przedmiot")),
                        name: reader.IsDBNull(reader.GetOrdinal("Nazwa")) ? null : reader.GetString(reader.GetOrdinal("Nazwa"))
                    ));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetAllSubjectsAsList: {ex.Message}");
                throw new Exception("Nie udało się pobrać listy przedmiotów.", ex);
            }

            return subjects;
        }

        /// <summary>
        /// Dla powiązań z interfejsem (np. ComboBox w MVVM)
        /// </summary>
        public ObservableCollection<Subject> GetAllSubjects()
        {
            var list = new ObservableCollection<Subject>();

            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT ID_Przedmiot, Nazwa FROM Przedmioty";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Subject
                {
                    Id = reader.GetInt32(0),
                    Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1)
                });
            }

            return list;
        }

        public Subject? GetSubjectById(int subjectId)
        {
            Subject? subject = null;

            using var connection = new SqliteConnection($"Data Source={dbPath}");
            try
            {
                connection.Open();

                string query = "SELECT ID_Przedmiot, Nazwa FROM Przedmioty WHERE ID_Przedmiot = @Id";
                using var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", subjectId);

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    subject = new Subject(
                        id: reader.GetInt32(reader.GetOrdinal("ID_Przedmiot")),
                        name: reader.IsDBNull(reader.GetOrdinal("Nazwa")) ? null : reader.GetString(reader.GetOrdinal("Nazwa"))
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetSubjectById({subjectId}): {ex.Message}");
                throw new Exception($"Nie udało się pobrać przedmiotu o ID {subjectId}.", ex);
            }

            return subject;
        }

        public void AddSubject(Subject newSubject)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            string query = @"INSERT INTO Przedmioty (Nazwa)
                             VALUES (@Name)";

            using var command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Name", newSubject.Name ?? (object)DBNull.Value);
            command.ExecuteNonQuery();
        }

        public void UpdateSubject(Subject updatedSubject)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            string query = @"UPDATE Przedmioty
                             SET Nazwa = @Name
                             WHERE ID_Przedmiot = @Id";

            using var command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Name", updatedSubject.Name ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Id", updatedSubject.Id);
            command.ExecuteNonQuery();
        }

        public bool CanDeleteSubject(int subjectId)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            // Przykład - zakładam, że tabela Lekcje ma kolumnę Przedmiot_ID
            string query = "SELECT COUNT(*) FROM Lekcje WHERE Przedmiot_ID = @SubjectId";

            using var command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@SubjectId", subjectId);

            long count = (long)command.ExecuteScalar();

            return count == 0; // można usunąć, jeśli brak powiązań
        }

        public bool SubjectNameExistsExceptId(string name, int id)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            string query = "SELECT COUNT(*) FROM Przedmioty WHERE Nazwa = @Name AND ID_Przedmiot != @Id";

            using var command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Id", id);

            long count = (long)command.ExecuteScalar();

            return count > 0;
        }

        public bool SubjectNameExists(string name)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            string query = "SELECT COUNT(*) FROM Przedmioty WHERE Nazwa = @Name"; // zmień na właściwą nazwę tabeli i kolumny

            using var command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Name", name);

            long count = (long)command.ExecuteScalar();

            return count > 0;
        }

        public bool DeleteSubject(int subjectId)
        {
            if (!CanDeleteSubject(subjectId))
                return false;

            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            string query = "DELETE FROM Przedmioty WHERE ID_Przedmiot = @Id";

            using var command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Id", subjectId);
            command.ExecuteNonQuery();

            return true;
        }
    }
}
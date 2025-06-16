using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;
using SchoolSystem.Model;

namespace SchoolSystem.Repositories
{
    public class SubjectRepository
    {
        private readonly string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "szkola.db");

        public List<Subject> GetAllSubjects()
        {
            List<Subject> subjects = new List<Subject>();

            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT ID_Przedmiot, Nazwa FROM Przedmioty";

                    using (var command = new SqliteCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            subjects.Add(new Subject(
                                id: reader.GetInt32(reader.GetOrdinal("ID_Przedmiot")),
                                name: reader.IsDBNull(reader.GetOrdinal("Nazwa")) ? null : reader.GetString(reader.GetOrdinal("Nazwa"))
                            ));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd przy pobieraniu przedmiotów: {ex.Message}");
                    throw new Exception("Nie udało się pobrać listy przedmiotów.", ex);
                }
            }

            return subjects;
        }

        public Subject? GetSubjectById(int subjectId)
        {
            Subject? subject = null;

            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT ID_Przedmiot, Nazwa FROM Przedmioty WHERE ID_Przedmiot = @Id";

                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", subjectId);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                subject = new Subject(
                                    id: reader.GetInt32(reader.GetOrdinal("ID_Przedmiot")),
                                    name: reader.IsDBNull(reader.GetOrdinal("Nazwa")) ? null : reader.GetString(reader.GetOrdinal("Nazwa"))
                                );
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd przy pobieraniu przedmiotu o ID {subjectId}: {ex.Message}");
                    throw new Exception($"Nie udało się pobrać przedmiotu o ID {subjectId}.", ex);
                }
            }

            return subject;
        }
    }
}

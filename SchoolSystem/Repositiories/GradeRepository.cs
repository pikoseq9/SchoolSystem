using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Data.Sqlite;
using SchoolSystem.Model;

namespace SchoolSystem.Repositories
{
    public class Graderepository
    {
        private string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "szkola.db");

        public ObservableCollection<Grade> GetAllGrades()
        {
            ObservableCollection<Grade> grades = new ObservableCollection<Grade>();

            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT ID_Ocena, Uczen_ID, Przedmiot_ID, Data, Ocena, Kategoria, Waga FROM Oceny";

                    using (SqliteCommand command = new SqliteCommand(query, connection))
                    {
                        using (SqliteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                grades.Add(new Grade(
                                    id: reader.GetInt32(reader.GetOrdinal("ID_Ocena")),
                                    studentID: reader.GetInt32(reader.GetOrdinal("Uczen_ID")),
                                    subjectID: reader.GetInt32(reader.GetOrdinal("Przedmiot_ID")),
                                    date: reader.GetDateTime(reader.GetOrdinal("Data")),
                                    value: reader.GetDecimal(reader.GetOrdinal("Ocena")),
                                    category: reader.GetString(reader.GetOrdinal("Kategoria")),
                                    weight: reader.GetInt32(reader.GetOrdinal("Waga"))
                                ));
                            }
                        }
                    }
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine($"Błąd bazy danych: {ex.Message}");
                    throw new Exception("Nie udało się pobrać danych ocen z bazy.", ex);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił nieoczekiwany błąd: {ex.Message}");
                    throw new Exception("Wystąpił nieoczekiwany błąd podczas pobierania danych ocen.", ex);
                }
            }
            return grades;
        }

        public ObservableCollection<Grade>? GetAllGradesByStudentId(int studentId)
        {
            ObservableCollection<Grade> grades = new ObservableCollection<Grade>();

            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT ID_Ocena, Uczen_ID, Przedmiot_ID, Data, Ocena, Kategoria, Waga FROM Oceny WHERE Uczen_ID = @Id";

                    using (SqliteCommand command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", studentId);
                        using (SqliteDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                grades.Add(new Grade(
                                    id: reader.GetInt32(reader.GetOrdinal("ID_Ocena")),
                                    studentID: reader.GetInt32(reader.GetOrdinal("Uczen_ID")),
                                    subjectID: reader.GetInt32(reader.GetOrdinal("Przedmiot_ID")),
                                    date: reader.GetDateTime(reader.GetOrdinal("Data")),
                                    value: reader.GetDecimal(reader.GetOrdinal("Ocena")),
                                    category: reader.GetString(reader.GetOrdinal("Kategoria")),
                                    weight: reader.GetInt32(reader.GetOrdinal("Waga"))
                                ));
                            }
                        }
                    }
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine($"Błąd bazy danych: {ex.Message}");
                    throw new Exception("Nie udało się pobrać danych ocen z bazy.", ex);
                }
                catch (Exception ex)
                {
                    // Obsługa innych, ogólnych błędów
                    Console.WriteLine($"Wystąpił nieoczekiwany błąd: {ex.Message}");
                    throw new Exception("Wystąpił nieoczekiwany błąd podczas pobierania danych ocen.", ex);
                }
            }
            return grades;
        }

        public ObservableCollection<Grade> GetGradesWithSubjectNameByStudentId(int studentId)
        {
            ObservableCollection<Grade> grades = new ObservableCollection<Grade>();

            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                string query = @"
            SELECT 
                o.ID_Ocena,
                o.Uczen_ID,
                o.Przedmiot_ID,
                o.Data,
                o.Ocena,
                o.Kategoria,
                o.Waga,
                p.Nazwa AS SubjectName
            FROM 
                Oceny o
            JOIN 
                Przedmioty p ON o.Przedmiot_ID = p.ID_Przedmiot
            WHERE 
                o.Uczen_ID = @id";

                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", studentId);

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var grade = new Grade(
                                id: reader.GetInt32(0),
                                studentID: reader.GetInt32(1),
                                subjectID: reader.GetInt32(2),
                                date: DateTime.Parse(reader.GetString(3)),
                                value: (decimal)reader.GetDouble(4),
                                category: reader.GetString(5),
                                weight: reader.GetInt32(6)
                            );

                            grade.SubjectName = reader.GetString(7);

                            grades.Add(grade);
                        }
                    }
                }
            }

            return grades;
        }


        public Grade? GetGradeById(int gradeId)
        {
            Grade? grade = null;

            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT ID_Ocena, Uczen_ID, Przedmiot_ID, Data, Ocena, Kategoria, Waga FROM Oceny WHERE ID_Ocena = @Id";

                    using (SqliteCommand command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", gradeId);

                        using (SqliteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                grade = new Grade(
                                    id: reader.GetInt32(reader.GetOrdinal("ID_Ocena")),
                                    studentID: reader.GetInt32(reader.GetOrdinal("Uczen_ID")),
                                    subjectID: reader.GetInt32(reader.GetOrdinal("Przedmiot_ID")),
                                    date: reader.GetDateTime(reader.GetOrdinal("Data")),
                                    value: reader.GetDecimal(reader.GetOrdinal("Waga")),
                                    category: reader.IsDBNull(reader.GetOrdinal("Kategoria")) ? null : reader.GetString(reader.GetOrdinal("Kategoria")),
                                    weight: reader.GetInt32(reader.GetOrdinal("Waga"))
                                );
                            }
                        }
                    }
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine($"Błąd bazy danych podczas pobierania oceny o ID {gradeId}: {ex.Message}");
                    throw new Exception($"Nie udało się pobrać oceny o ID {gradeId}.", ex);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił nieoczekiwany błąd podczas pobierania oceny o ID {gradeId}: {ex.Message}");
                    throw new Exception($"Wystąpił nieoczekiwany błąd podczas pobierania oceny o ID {gradeId}.", ex);
                }
            }
            return grade;
        }


        public void InsertGrade(Grade grade)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
        INSERT INTO Oceny (Uczen_ID, Przedmiot_ID, Data, Ocena, Kategoria, Waga)
        VALUES (@Uczen_ID, @Przedmiot_ID, @Data, @Ocena, @Kategoria, @Waga);";

            command.Parameters.AddWithValue("@Uczen_ID", grade.StudentID);
            command.Parameters.AddWithValue("@Przedmiot_ID", grade.SubjectID);
            command.Parameters.AddWithValue("@Data", grade.Date.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@Ocena", grade.Value);
            command.Parameters.AddWithValue("@Kategoria", grade.Category ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Waga", grade.Weight);

            command.ExecuteNonQuery();
        }

        public void DeleteGrade(int id)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Oceny WHERE ID_Ocena = @Id";
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
        }



    }
}

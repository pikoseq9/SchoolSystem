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
                    throw new Exception("Nie udało się pobrać danych uczniów z bazy.", ex);
                }
                catch (Exception ex)
                {
                    // Obsługa innych, ogólnych błędów
                    Console.WriteLine($"Wystąpił nieoczekiwany błąd: {ex.Message}");
                    throw new Exception("Wystąpił nieoczekiwany błąd podczas pobierania danych uczniów.", ex);
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
                    throw new Exception("Nie udało się pobrać danych uczniów z bazy.", ex);
                }
                catch (Exception ex)
                {
                    // Obsługa innych, ogólnych błędów
                    Console.WriteLine($"Wystąpił nieoczekiwany błąd: {ex.Message}");
                    throw new Exception("Wystąpił nieoczekiwany błąd podczas pobierania danych uczniów.", ex);
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
    }
}

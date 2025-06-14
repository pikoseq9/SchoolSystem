using Microsoft.Data.Sqlite;
using SchoolSystem.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace SchoolSystem.Repositories
{
    public class StudentRepository
    {
        private string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "szkola.db");


        public ObservableCollection<Student> GetAllStudents()
        {
            ObservableCollection<Student> students = new ObservableCollection<Student>();

            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                try
                {
                    connection.Open();

                    string query = @"
                    SELECT 
                        u.ID_Uczen, u.Klasa_ID, u.Imie, u.Nazwisko, 
                        u.Data_Urodzenia, u.Plec, u.PESEL, u.Login, u.Haslo,
                        k.Kod
                    FROM Uczniowie u
                    LEFT JOIN Klasy k ON u.Klasa_ID = k.ID_Klasa";

                    using (SqliteCommand command = new SqliteCommand(query, connection))
                    {
                        using (SqliteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var student = new Student(
                                    id: reader.GetInt32(reader.GetOrdinal("ID_Uczen")),
                                    classID: reader.GetInt32(reader.GetOrdinal("Klasa_ID")),
                                    name: reader.IsDBNull(reader.GetOrdinal("Imie")) ? null : reader.GetString(reader.GetOrdinal("Imie")),
                                    surName: reader.IsDBNull(reader.GetOrdinal("Nazwisko")) ? null : reader.GetString(reader.GetOrdinal("Nazwisko")),
                                    dateOfBirth: reader.IsDBNull(reader.GetOrdinal("Data_Urodzenia")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Data_Urodzenia")),
                                    gender: reader.IsDBNull(reader.GetOrdinal("Plec")) ? null : reader.GetString(reader.GetOrdinal("Plec")),
                                    pesel: reader.IsDBNull(reader.GetOrdinal("PESEL")) ? null : reader.GetString(reader.GetOrdinal("PESEL")),
                                    login: reader.IsDBNull(reader.GetOrdinal("Login")) ? null : reader.GetString(reader.GetOrdinal("Login")),
                                    password: reader.IsDBNull(reader.GetOrdinal("Haslo")) ? null : reader.GetString(reader.GetOrdinal("Haslo"))
                                );

                                student.ClassCode = reader.IsDBNull(reader.GetOrdinal("Kod")) ? null : reader.GetString(reader.GetOrdinal("Kod"));

                                students.Add(student);
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
                    Console.WriteLine($"Wystąpił nieoczekiwany błąd: {ex.Message}");
                    throw new Exception("Wystąpił nieoczekiwany błąd podczas pobierania danych uczniów.", ex);
                }
            }

            return students;
        }

        public Student? GetStudentById(int studentId)
        {
            Student? student = null;

            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT ID_Uczen, Klasa_ID, Imie, Nazwisko, Data_Urodzenia, Plec, PESEL, Login, Haslo FROM Uczniowie WHERE ID_Uczen = @Id";

                    using (SqliteCommand command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", studentId);

                        using (SqliteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                student = new Student(
                                    id: reader.GetInt32(reader.GetOrdinal("ID_Uczen")),
                                    classID: reader.GetInt32(reader.GetOrdinal("Klasa_ID")),
                                    name: reader.IsDBNull(reader.GetOrdinal("Imie")) ? null : reader.GetString(reader.GetOrdinal("Imie")),
                                    surName: reader.IsDBNull(reader.GetOrdinal("Nazwisko")) ? null : reader.GetString(reader.GetOrdinal("Nazwisko")),
                                    dateOfBirth: reader.IsDBNull(reader.GetOrdinal("Data_Urodzenia")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Data_Urodzenia")),
                                    gender: reader.IsDBNull(reader.GetOrdinal("Plec")) ? null : reader.GetString(reader.GetOrdinal("Plec")),
                                    pesel: reader.IsDBNull(reader.GetOrdinal("PESEL")) ? null : reader.GetString(reader.GetOrdinal("PESEL")),
                                    login: reader.IsDBNull(reader.GetOrdinal("Login")) ? null : reader.GetString(reader.GetOrdinal("Login")),
                                    password: reader.IsDBNull(reader.GetOrdinal("Haslo")) ? null : reader.GetString(reader.GetOrdinal("Haslo"))
                                );
                            }
                        }
                    }
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine($"Błąd bazy danych podczas pobierania studenta o ID {studentId}: {ex.Message}");
                    throw new Exception($"Nie udało się pobrać studenta o ID {studentId}.", ex);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił nieoczekiwany błąd podczas pobierania studenta o ID {studentId}: {ex.Message}");
                    throw new Exception($"Wystąpił nieoczekiwany błąd podczas pobierania studenta o ID {studentId}.", ex);
                }
            }
            return student;
        }

        public Student GetStudentByLogin(string login)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                string query = "SELECT ID_Uczen, Klasa_ID, Imie, Nazwisko, Data_Urodzenia, Plec, PESEL, Login, Haslo " +
                               "FROM Uczniowie WHERE Login = @login";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@login", login);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Student(
                                id: reader.GetInt32(reader.GetOrdinal("ID_Uczen")),
                                classID: reader.GetInt32(reader.GetOrdinal("Klasa_ID")),
                                name: reader.IsDBNull(reader.GetOrdinal("Imie")) ? null : reader.GetString(reader.GetOrdinal("Imie")),
                                surName: reader.IsDBNull(reader.GetOrdinal("Nazwisko")) ? null : reader.GetString(reader.GetOrdinal("Nazwisko")),
                                dateOfBirth: reader.IsDBNull(reader.GetOrdinal("Data_Urodzenia")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Data_Urodzenia")),
                                gender: reader.IsDBNull(reader.GetOrdinal("Plec")) ? null : reader.GetString(reader.GetOrdinal("Plec")),
                                pesel: reader.IsDBNull(reader.GetOrdinal("PESEL")) ? null : reader.GetString(reader.GetOrdinal("PESEL")),
                                login: reader.IsDBNull(reader.GetOrdinal("Login")) ? null : reader.GetString(reader.GetOrdinal("Login")),
                                password: reader.IsDBNull(reader.GetOrdinal("Haslo")) ? null : reader.GetString(reader.GetOrdinal("Haslo"))
                            );
                        }
                    }
                }
            }

            return null;
        }

    }
}

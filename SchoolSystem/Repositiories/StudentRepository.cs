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

        public void UpdateStudent(Student student)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                string query = @"UPDATE Uczniowie
                         SET Klasa_ID = @ClassID,
                             Imie = @Name,
                             Nazwisko = @SurName,
                             Data_Urodzenia = @DateOfBirth,
                             Plec = @Gender,
                             PESEL = @Pesel,
                             Login = @Login,
                             Haslo = @Password
                         WHERE ID_Uczen = @Id";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClassID", student.ClassID);
                    command.Parameters.AddWithValue("@Name", (object)student.Name ?? DBNull.Value);
                    command.Parameters.AddWithValue("@SurName", (object)student.SurName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DateOfBirth", (object)student.DateOfBirth ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Gender", (object)student.Gender ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Pesel", (object)student.PESEL ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Login", (object)student.Login ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Password", (object)student.Password ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Id", student.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public bool StudentExistsByPESEL(string pesel)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                using (var pragma = new SqliteCommand("PRAGMA foreign_keys = ON;", connection))
                    pragma.ExecuteNonQuery();

                string query = "SELECT COUNT(*) FROM Uczniowie WHERE PESEL = @PESEL";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PESEL", pesel);
                    long count = (long)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public bool StudentExistsByLogin(string login)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                using (var pragma = new SqliteCommand("PRAGMA foreign_keys = ON;", connection))
                    pragma.ExecuteNonQuery();

                string query = "SELECT COUNT(*) FROM Uczniowie WHERE Login = @Login";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Login", login);
                    long count = (long)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public void DeleteStudent(int studentId)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                string query = "DELETE FROM Uczniowie WHERE ID_Uczen = @Id";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", studentId);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqliteException ex)
                    {
                        if (ex.SqliteErrorCode == 19) 
                        {
                            throw new InvalidOperationException("Nie można usunąć ucznia, ponieważ jest powiązany z innymi danymi (np. oceny, frekwencja).", ex);
                        }
                        else
                        {
                            throw; 
                        }
                    }
                }
            }
        }

        public void AddStudent(Student student)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                string query = @"INSERT INTO Uczniowie 
                        (Klasa_ID, Imie, Nazwisko, Data_Urodzenia, Plec, PESEL, Login, Haslo) 
                         VALUES (@ClassID, @Name, @SurName, @DateOfBirth, @Gender, @Pesel, @Login, @Password)";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClassID", student.ClassID);
                    command.Parameters.AddWithValue("@Name", (object)student.Name ?? DBNull.Value);
                    command.Parameters.AddWithValue("@SurName", (object)student.SurName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DateOfBirth", (object)student.DateOfBirth ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Gender", (object)student.Gender ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Pesel", (object)student.PESEL ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Login", (object)student.Login ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Password", (object)student.Password ?? DBNull.Value);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;
using SchoolSystem.Model;

namespace SchoolSystem.Repositories
{
    public class TeacherRepository
    {
        private string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "szkola.db");


        public List<Teacher> GetAllTeachers()
        {
            List<Teacher> teachers = new List<Teacher>();

            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT ID_Nauczyciel, Imie, Nazwisko, Data_Urodzenia, Plec, Numer_tel, Login, Haslo FROM Nauczyciele";

                    using (SqliteCommand command = new SqliteCommand(query, connection))
                    {
                        using (SqliteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                teachers.Add(new Teacher(
                                    id: reader.GetInt32(reader.GetOrdinal("ID_Nauczyciel")),
                                    name: reader.IsDBNull(reader.GetOrdinal("Imie")) ? null : reader.GetString(reader.GetOrdinal("Imie")),
                                    surName: reader.IsDBNull(reader.GetOrdinal("Nazwisko")) ? null : reader.GetString(reader.GetOrdinal("Nazwisko")),
                                    dateOfBirth: reader.IsDBNull(reader.GetOrdinal("Data_Urodzenia")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Data_Urodzenia")),
                                    gender: reader.IsDBNull(reader.GetOrdinal("Plec")) ? null : reader.GetString(reader.GetOrdinal("Plec")),
                                    phoneNumber: reader.IsDBNull(reader.GetOrdinal("Numer_tel")) ? null : reader.GetString(reader.GetOrdinal("Numer_tel")),
                                    login: reader.IsDBNull(reader.GetOrdinal("Login")) ? null : reader.GetString(reader.GetOrdinal("Login")),
                                    password: reader.IsDBNull(reader.GetOrdinal("Haslo")) ? null : reader.GetString(reader.GetOrdinal("Haslo"))
                                ));
                            }
                        }
                    }
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine($"Błąd bazy danych: {ex.Message}");
                    throw new Exception("Nie udało się pobrać danych nauczycieli z bazy.", ex);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił nieoczekiwany błąd: {ex.Message}");
                    throw new Exception("Wystąpił nieoczekiwany błąd podczas pobierania danych nauczycieli.", ex);
                }
            }
            return teachers;
        }

        public Teacher? GetTeacherById(int teacherId)
        {
            Teacher? teacher = null;

            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT ID_Nauczyciel, Imie, Nazwisko, Data_Urodzenia, Plec, Numer_tel, Login, Haslo FROM Nauczyciele WHERE ID_Nauczyciel = @Id";

                    using (SqliteCommand command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", teacherId);

                        using (SqliteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                teacher = new Teacher(
                                    id: reader.GetInt32(reader.GetOrdinal("ID_Nauczyciel")),
                                    name: reader.IsDBNull(reader.GetOrdinal("Imie")) ? null : reader.GetString(reader.GetOrdinal("Imie")),
                                    surName: reader.IsDBNull(reader.GetOrdinal("Nazwisko")) ? null : reader.GetString(reader.GetOrdinal("Nazwisko")),
                                    dateOfBirth: reader.IsDBNull(reader.GetOrdinal("Data_Urodzenia")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Data_Urodzenia")),
                                    gender: reader.IsDBNull(reader.GetOrdinal("Plec")) ? null : reader.GetString(reader.GetOrdinal("Plec")),
                                    phoneNumber: reader.IsDBNull(reader.GetOrdinal("Numer_tel")) ? null : reader.GetString(reader.GetOrdinal("Numer_tel")),
                                    login: reader.IsDBNull(reader.GetOrdinal("Login")) ? null : reader.GetString(reader.GetOrdinal("Login")),
                                    password: reader.IsDBNull(reader.GetOrdinal("Haslo")) ? null : reader.GetString(reader.GetOrdinal("Haslo"))
                                );
                            }
                        }
                    }
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine($"Błąd bazy danych podczas pobierania nauczycieli o ID {teacherId}: {ex.Message}");
                    throw new Exception($"Nie udało się pobrać studenta o ID {teacherId}.", ex);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił nieoczekiwany błąd podczas pobierania nauczyciela ID {teacherId}: {ex.Message}");
                    throw new Exception($"Wystąpił nieoczekiwany błąd podczas pobierania nauczycieli o ID {teacherId}.", ex);
                }
            }
            return teacher;
        }

        public Teacher GetTeacherByLogin(string login)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                string query = "SELECT ID_Nauczyciel, Imie, Nazwisko, Data_Urodzenia, Plec, Numer_tel, Login, Haslo " +
                               "FROM Nauczyciele WHERE Login = @login";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@login", login);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Teacher(
                                id: reader.GetInt32(reader.GetOrdinal("ID_Nauczyciel")),
                                name: reader.IsDBNull(reader.GetOrdinal("Imie")) ? null : reader.GetString(reader.GetOrdinal("Imie")),
                                surName: reader.IsDBNull(reader.GetOrdinal("Nazwisko")) ? null : reader.GetString(reader.GetOrdinal("Nazwisko")),
                                dateOfBirth: reader.IsDBNull(reader.GetOrdinal("Data_Urodzenia")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Data_Urodzenia")),
                                gender: reader.IsDBNull(reader.GetOrdinal("Plec")) ? null : reader.GetString(reader.GetOrdinal("Plec")),
                                phoneNumber: reader.IsDBNull(reader.GetOrdinal("Numer_tel")) ? null : reader.GetString(reader.GetOrdinal("Numer_tel")),
                                login: reader.IsDBNull(reader.GetOrdinal("Login")) ? null : reader.GetString(reader.GetOrdinal("Login")),
                                password: reader.IsDBNull(reader.GetOrdinal("Haslo")) ? null : reader.GetString(reader.GetOrdinal("Haslo"))
                            );
                        }
                    }
                }
            }

            return null;
        }

        public void UpdateTeacher(Teacher teacher)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                string query = @"UPDATE Nauczyciele
                         SET Imie = @Name,
                             Nazwisko = @SurName,
                             Data_Urodzenia = @DateOfBirth,
                             Plec = @Gender,
                             Numer_tel = @PhoneNumber,
                             Login = @Login,
                             Haslo = @Password
                         WHERE ID_Nauczyciel = @Id";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", (object)teacher.Name ?? DBNull.Value);
                    command.Parameters.AddWithValue("@SurName", (object)teacher.SurName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DateOfBirth", (object)teacher.DateOfBirth ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Gender", (object)teacher.Gender ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PhoneNumber", (object)teacher.PhoneNumber ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Login", (object)teacher.Login ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Password", (object)teacher.Password ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Id", teacher.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteTeacher(int teacherId)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                string query = "DELETE FROM Nauczyciele WHERE ID_Nauczyciel = @Id";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", teacherId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddTeacher(Teacher teacher)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                string query = @"INSERT INTO Nauczyciele 
                        (Imie, Nazwisko, Data_Urodzenia, Plec, Numer_tel, Login, Haslo) 
                         VALUES (@Name, @SurName, @DateOfBirth, @Gender, @PhoneNumber, @Login, @Password)";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", (object)teacher.Name ?? DBNull.Value);
                    command.Parameters.AddWithValue("@SurName", (object)teacher.SurName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DateOfBirth", (object)teacher.DateOfBirth ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Gender", (object)teacher.Gender ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PhoneNumber", (object)teacher.PhoneNumber ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Login", (object)teacher.Login ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Password", (object)teacher.Password ?? DBNull.Value);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;
using SchoolSystem.Model;

namespace SchoolSystem.Repositories
{
    public class TeacherRepository
    {
        private string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "szkola.db");

        public Teacher GetTeacherByLogin(string login, string password)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                string query = "SELECT ID_Nauczyciel, Imie, Nazwisko, Data_Urodzenia, Plec, Numer_tel, Login, Haslo " +
                               "FROM Nauczyciele WHERE Login = @login AND Haslo = @password";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@password", password);

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
    }
}
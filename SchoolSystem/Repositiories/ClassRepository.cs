using Microsoft.Data.Sqlite;
using SchoolSystem.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Text;

using System.Threading.Tasks;

namespace SchoolSystem.Repositiories
{
    internal class ClassRepository
    {


        private string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "szkola.db");

        public ObservableCollection<Class>? GetClassByClassId(int studentId)
        {
            ObservableCollection<Class> classes = new ObservableCollection<Class>();

            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT ID_Klasa, Wychowawca_ID, Kod WHERE ID_Klasa = @Id";

                    using (SqliteCommand command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", studentId);
                        using (SqliteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                classes.Add(new Class(
                                    id: reader.GetInt32(reader.GetOrdinal("ID_Ocena")),
                                    classTeacherID: reader.GetInt32(reader.GetOrdinal("Wychowawca_ID")),
                                    code: reader.GetString(reader.GetOrdinal("Kod"))
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
                    Console.WriteLine($"Wystąpił nieoczekiwany błąd: {ex.Message}");
                    throw new Exception("Wystąpił nieoczekiwany błąd podczas pobierania danych uczniów.", ex);
                }
            }
            return classes;
        }
    }
}

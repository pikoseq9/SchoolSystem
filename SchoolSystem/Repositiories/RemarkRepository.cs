using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Data.Sqlite;
using SchoolSystem.Model;

namespace SchoolSystem.Repositories
{
    public class RemarkRepository
    {
        private string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "szkola.db");
        private readonly TeacherRepository _teacherRepository;
        public RemarkRepository(TeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository ?? throw new ArgumentNullException(nameof(teacherRepository));
        }

        public ObservableCollection<Remark>? GetAllRemarksByStudentId(int studentId)
        {
            ObservableCollection<Remark> remarks = new ObservableCollection<Remark>();

            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT ID_Uwaga, Uczen_ID, Wystawiajacy_ID, Tresc FROM Uwagi WHERE Uczen_ID = @Id";

                    using (SqliteCommand command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", studentId);
                        using (SqliteDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {

                                int teacherId = reader.GetInt32(reader.GetOrdinal("Wystawiajacy_ID"));
                                string teacherFullName = "Nieznany Nauczyciel"; // Default value

                                // Use the injected TeacherRepository to get the teacher's name
                                Teacher teacher = _teacherRepository.GetTeacherById(teacherId);
                                if (teacher != null)
                                {
                                    teacherFullName = $"{teacher.Name} {teacher.SurName}";
                                }

                                remarks.Add(new Remark(
                                    id: reader.GetInt32(reader.GetOrdinal("ID_Uwaga")),
                                    studentID: reader.GetInt32(reader.GetOrdinal("Uczen_ID")),
                                    teacherID: reader.GetInt32(reader.GetOrdinal("Wystawiajacy_ID")),
                                    value: reader.GetString(reader.GetOrdinal("Tresc")), 
                                    teacherFullName: teacherFullName
                                ));
                            }
                        }
                    }
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine($"Błąd bazy danych: {ex.Message}");
                    throw new Exception("Nie udało się pobrać danych uwag z bazy.", ex);
                }
                catch (Exception ex)
                {
                    // Obsługa innych, ogólnych błędów
                    Console.WriteLine($"Wystąpił nieoczekiwany błąd: {ex.Message}");
                    throw new Exception("Wystąpił nieoczekiwany błąd podczas pobierania danych uwag.", ex);
                }
            }
            return remarks;
        }

        public Remark? GetRemarkById(int remarkId)
        {
            Remark? remark = null;

            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT ID_Uwaga, Uczen_ID, Wystawiajacy_ID, Tresc FROM Uwagi WHERE ID_Uwaga = @Id";

                    using (SqliteCommand command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", remarkId);

                        using (SqliteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int teacherId = reader.GetInt32(reader.GetOrdinal("Wystawiajacy_ID"));
                                string teacherFullName = "Nieznany Nauczyciel"; // Default value

                                // Use the injected TeacherRepository to get the teacher's name
                                Teacher teacher = _teacherRepository.GetTeacherById(teacherId);
                                if (teacher != null)
                                {
                                    teacherFullName = $"{teacher.Name} {teacher.SurName}";
                                }
                                remark = new Remark(
                                    id: reader.GetInt32(reader.GetOrdinal("ID_Uwaga")),
                                    studentID: reader.GetInt32(reader.GetOrdinal("Uczen_ID")),
                                    teacherID: reader.GetInt32(reader.GetOrdinal("Wystawiajacy_ID")),
                                    value: reader.GetString(reader.GetOrdinal("Tresc")), 
                                    teacherFullName: teacherFullName
                                );
                            }
                        }
                    }
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine($"Błąd bazy danych podczas pobierania uwagi o ID {remarkId}: {ex.Message}");
                    throw new Exception($"Nie udało się pobrać uwagi o ID {remarkId}.", ex);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił nieoczekiwany błąd podczas pobierania uwagi o ID {remarkId}: {ex.Message}");
                    throw new Exception($"Wystąpił nieoczekiwany błąd podczas pobierania uwagi o ID {remarkId}.", ex);
                }
            }   
            return remark;
        }
    }
}

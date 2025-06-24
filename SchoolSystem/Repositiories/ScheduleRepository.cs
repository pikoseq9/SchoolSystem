using System;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Data.Sqlite;
using SchoolSystem.Model;

namespace SchoolSystem.Repositories
{
    public class ScheduleRepository
    {
        private string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "szkola.db");

        public ObservableCollection<Lesson> GetAllLessons()
        {
            ObservableCollection<Lesson> lessons = new ObservableCollection<Lesson>();

            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT ID_Lekcja, Sala_ID, Przedmiot_ID, Klasa_ID, Prowadzacy_ID, Dzien_Tygodnia, Godzina_Rozpoczecia, Czas_Trwania FROM Lekcje";

                    using (SqliteCommand command = new SqliteCommand(query, connection))
                    {
                        using (SqliteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lessons.Add(new Lesson(
                                    id: reader.GetInt32(reader.GetOrdinal("ID_Lekcja")),
                                    roomID: reader.GetInt32(reader.GetOrdinal("Sala_ID")),
                                    subjectID: reader.GetInt32(reader.GetOrdinal("Przedmiot_ID")),
                                    classID: reader.GetInt32(reader.GetOrdinal("Klasa_ID")),
                                    teacherID: reader.GetInt32(reader.GetOrdinal("Prowadzacy_ID")),
                                    dayOfWeek: reader.GetString(reader.GetOrdinal("Dzien_Tygodnia")),
                                    startTime: reader.GetString(reader.GetOrdinal("Godzina_Rozpoczecia")),
                                    duration: reader.GetInt32(reader.GetOrdinal("Czas_Trwania"))
                                ));
                            }
                        }
                    }
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine($"Błąd bazy danych: {ex.Message}");
                    throw new Exception("Nie udało się pobrać danych lekcji z bazy.", ex);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił nieoczekiwany błąd: {ex.Message}");
                    throw new Exception("Wystąpił nieoczekiwany błąd podczas pobierania danych lekcji.", ex);
                }
            }
            return lessons;
        }

        public ObservableCollection<Lesson>? GetLessonsByClassId(int classId)
        {
            ObservableCollection<Lesson> lessons = new ObservableCollection<Lesson>();

            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT ID_Lekcja, Sala_ID, Przedmiot_ID, Klasa_ID, Prowadzacy_ID, Dzien_Tygodnia, Godzina_Rozpoczecia, Czas_Trwania FROM Lekcje WHERE Klasa_ID = @ClassId";

                    using (SqliteCommand command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ClassId", classId);

                        using (SqliteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lessons.Add(new Lesson(
                                    id: reader.GetInt32(reader.GetOrdinal("ID_Lekcja")),
                                    roomID: reader.GetInt32(reader.GetOrdinal("Sala_ID")),
                                    subjectID: reader.GetInt32(reader.GetOrdinal("Przedmiot_ID")),
                                    classID: reader.GetInt32(reader.GetOrdinal("Klasa_ID")),
                                    teacherID: reader.GetInt32(reader.GetOrdinal("Prowadzacy_ID")),
                                    dayOfWeek: reader.GetString(reader.GetOrdinal("Dzien_Tygodnia")),
                                    startTime: reader.GetString(reader.GetOrdinal("Godzina_Rozpoczecia")),
                                    duration: reader.GetInt32(reader.GetOrdinal("Czas_Trwania"))
                                ));
                            }
                        }
                    }
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine($"Błąd bazy danych: {ex.Message}");
                    throw new Exception("Nie udało się pobrać danych lekcji z bazy.", ex);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił nieoczekiwany błąd: {ex.Message}");
                    throw new Exception("Wystąpił nieoczekiwany błąd podczas pobierania danych lekcji.", ex);
                }
            }
            return lessons;
        }

        public Lesson? GetLessonById(int lessonId)
        {
            Lesson? lesson = null;

            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT ID_Lekcja, Sala_ID, Przedmiot_ID, Klasa_ID, Prowadzacy_ID, Dzien_Tygodnia, Godzina_Rozpoczecia, Czas_Trwania FROM Lekcje WHERE ID_Lekcja = @Id";

                    using (SqliteCommand command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", lessonId);

                        using (SqliteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lesson = new Lesson(
                                    id: reader.GetInt32(reader.GetOrdinal("ID_Lekcja")),
                                    roomID: reader.GetInt32(reader.GetOrdinal("Sala_ID")),
                                    subjectID: reader.GetInt32(reader.GetOrdinal("Przedmiot_ID")),
                                    classID: reader.GetInt32(reader.GetOrdinal("Klasa_ID")),
                                    teacherID: reader.GetInt32(reader.GetOrdinal("Prowadzacy_ID")),
                                    dayOfWeek: reader.GetString(reader.GetOrdinal("Dzien_Tygodnia")),
                                    startTime: reader.GetString(reader.GetOrdinal("Godzina_Rozpoczecia")),
                                    duration: reader.GetInt32(reader.GetOrdinal("Czas_Trwania"))
                                );
                            }
                        }
                    }
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine($"Błąd bazy danych podczas pobierania lekcji o ID {lessonId}: {ex.Message}");
                    throw new Exception($"Nie udało się pobrać lekcji o ID {lessonId}.", ex);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił nieoczekiwany błąd podczas pobierania lekcji o ID {lessonId}: {ex.Message}");
                    throw new Exception($"Wystąpił nieoczekiwany błąd podczas pobierania lekcji o ID {lessonId}.", ex);
                }
            }
            return lesson;
        }

        public void AddLesson(Lesson lesson)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
        INSERT INTO Lekcje (Sala_ID, Przedmiot_ID, Klasa_ID, Prowadzacy_ID, Dzien_Tygodnia, Godzina_Rozpoczecia, Czas_Trwania)
        VALUES (@RoomID, @SubjectID, @ClassID, @TeacherID, @Day, @StartTime, @Duration)";
            command.Parameters.AddWithValue("@RoomID", lesson.RoomID);
            command.Parameters.AddWithValue("@SubjectID", lesson.SubjectID);
            command.Parameters.AddWithValue("@ClassID", lesson.ClassID);
            command.Parameters.AddWithValue("@TeacherID", lesson.TeacherID);
            command.Parameters.AddWithValue("@Day", lesson.DayOfWeek);
            command.Parameters.AddWithValue("@StartTime", lesson.StartTime);
            command.Parameters.AddWithValue("@Duration", lesson.Duration);

            command.ExecuteNonQuery();
        }



        public void DeleteLessonById(int id)
        {
            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                try
                {
                    connection.Open();

                    string query = "DELETE FROM Lekcje WHERE ID_Lekcja = @Id";

                    using (SqliteCommand command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        command.ExecuteNonQuery();
                    }
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine($"Błąd podczas usuwania lekcji: {ex.Message}");
                    throw new Exception("Nie udało się usunąć lekcji z bazy.", ex);
                }
            }
        }

        public bool IsLessonOverlapping(int classId, string dayOfWeek, string startTime, int duration)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            string query = @"
        SELECT Godzina_Rozpoczecia, Czas_Trwania
        FROM Lekcje
        WHERE Klasa_ID = @ClassId AND Dzien_Tygodnia = @DayOfWeek";

            using var command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@ClassId", classId);
            command.Parameters.AddWithValue("@DayOfWeek", dayOfWeek);

            using var reader = command.ExecuteReader();
            var newStart = TimeSpan.Parse(startTime);
            var newEnd = newStart.Add(TimeSpan.FromMinutes(duration));

            while (reader.Read())
            {
                var existingStart = TimeSpan.Parse(reader.GetString(0));
                var existingEnd = existingStart.Add(TimeSpan.FromMinutes(reader.GetInt32(1)));

                if (newStart < existingEnd && existingStart < newEnd)
                {
                    return true;
                }
            }

            return false;
        }


    }

    

}

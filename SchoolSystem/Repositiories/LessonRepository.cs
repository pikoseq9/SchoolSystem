using Microsoft.Data.Sqlite;
using SchoolSystem.Model;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace SchoolSystem.Repositories
{
    public class LessonRepository
    {
        private readonly string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "szkola.db");

        public ObservableCollection<Lesson> GetLessonsByTeacherId(int teacherId)
        {
            ObservableCollection<Lesson> lessons = new();

            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            string query = @"
                SELECT l.ID_Lekcja, l.Sala_ID, l.Przedmiot_ID, l.Klasa_ID, l.Prowadzacy_ID, 
                       l.Dzien_Tygodnia, l.Godzina_Rozpoczecia, l.Czas_Trwania,
                       k.Kod AS KlasaNazwa, p.Nazwa AS PrzedmiotNazwa
                FROM Lekcje l
                JOIN Klasy k ON l.Klasa_ID = k.ID_Klasa
                JOIN Przedmioty p ON l.Przedmiot_ID = p.ID_Przedmiot
                WHERE k.Wychowawca_ID = @TeacherId";

            using var command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@TeacherId", teacherId);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                lessons.Add(new Lesson
                {
                    Id = reader.GetInt32(0),
                    RoomID = reader.GetInt32(1),
                    SubjectID = reader.GetInt32(2),
                    ClassID = reader.GetInt32(3),
                    TeacherID = reader.GetInt32(4),
                    DayOfWeek = reader.GetString(5),
                    StartTime = reader.GetString(6),
                    Duration = reader.GetInt32(7),
                    ClassName = reader.GetString(8),
                    SubjectName = reader.GetString(9)
                });
            }

            return lessons;
        }
    }
}

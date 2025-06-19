using Microsoft.Data.Sqlite;
using SchoolSystem.Model;
using System;
using System.Collections.ObjectModel;
using System.IO;

public class SubjectRepository
{
    private string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "szkola.db");

    public ObservableCollection<Subject> GetAllSubjects()
    {
        var list = new ObservableCollection<Subject>();

        using var connection = new SqliteConnection($"Data Source={dbPath}");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT ID_Przedmiot, Nazwa FROM Przedmioty";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new Subject
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            });
        }

        return list;
    }
}

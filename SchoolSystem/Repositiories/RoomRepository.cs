using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;
using SchoolSystem.Model;

namespace SchoolSystem.Repositories
{
    public class RoomRepository
    {
        private string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "szkola.db");

        public List<Room> GetAllRooms()
        {
            List<Room> rooms = new List<Room>();

            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT ID_Sala, Nr_Sali FROM Sale";

                    using (SqliteCommand command = new SqliteCommand(query, connection))
                    {
                        using (SqliteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                rooms.Add(new Room(
                                    id: reader.GetInt32(reader.GetOrdinal("ID_Sala")),
                                    number: reader.IsDBNull(reader.GetOrdinal("Nr_Sali")) ? null : reader.GetString(reader.GetOrdinal("Nr_Sali"))
                                ));
                            }
                        }
                    }
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine($"Błąd bazy danych: {ex.Message}");
                    throw new Exception("Nie udało się pobrać danych sal z bazy.", ex);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił nieoczekiwany błąd: {ex.Message}");
                    throw new Exception("Wystąpił błąd podczas pobierania danych sal.", ex);
                }
            }

            return rooms;
        }

        public Room? GetRoomById(int roomId)
        {
            Room? room = null;

            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT ID_Sala, Nr_Sali FROM Sale WHERE ID_Sala = @Id";

                    using (SqliteCommand command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", roomId);

                        using (SqliteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                room = new Room(
                                    id: reader.GetInt32(reader.GetOrdinal("ID_Sala")),
                                    number: reader.IsDBNull(reader.GetOrdinal("Nr_Sali")) ? null : reader.GetString(reader.GetOrdinal("Nr_Sali"))
                                );
                            }
                        }
                    }
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine($"Błąd bazy danych podczas pobierania sali o ID {roomId}: {ex.Message}");
                    throw new Exception($"Nie udało się pobrać sali o ID {roomId}.", ex);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił nieoczekiwany błąd podczas pobierania sali o ID {roomId}: {ex.Message}");
                    throw new Exception($"Wystąpił nieoczekiwany błąd podczas pobierania sali o ID {roomId}.", ex);
                }
            }

            return room;
        }
    }
}

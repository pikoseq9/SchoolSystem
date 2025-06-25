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

        public void AddRoom(Room newRoom)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            string query = @"INSERT INTO Sale (Nr_Sali)
                             VALUES (@Number)";

            using var command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Number", newRoom.Number ?? (object)DBNull.Value);
            command.ExecuteNonQuery();
        }

        public void UpdateRoom(Room updatedRoom)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            string query = @"UPDATE Sale
                             SET Nr_Sali = @Number
                             WHERE ID_Sala = @Id";

            using var command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Number", updatedRoom.Number ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Id", updatedRoom.ID);
            command.ExecuteNonQuery();
        }

        public bool CanDeleteRoom(int roomId)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            string query = "SELECT COUNT(*) FROM Lekcje WHERE Sala_ID = @RoomId";

            using var command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@RoomId", roomId);

            long count = (long)command.ExecuteScalar();

            return count == 0;
        }

        public bool RoomNumberExists(string number)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            string query = "SELECT COUNT(*) FROM Sale WHERE Nr_Sali = @Number";

            using var command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Number", number);

            long count = (long)command.ExecuteScalar();

            return count > 0;
        }


        public bool DeleteRoom(int roomId)
        {
            if (!CanDeleteRoom(roomId))
                return false;

            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            string query = "DELETE FROM Sale WHERE ID_Sala = @Id";

            using var command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Id", roomId);
            command.ExecuteNonQuery();

            return true;
        }
    }
}

using UnityEngine;
using System;
using System.Data;
using System.Data.Common;
using Npgsql;

public class DatabaseManager : MonoBehaviour
{
    void Start()
    {
        string connectionString = "Server=myServerAddress;Port=myPort;Database=myDatabase;User Id=myUsername;Password=myPassword;";
        string sql = "SELECT * FROM public.\"character\"";

        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int characterId = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string description = reader.GetString(2);
                        int silverValue = reader.GetInt32(3);
                        int value = reader.GetInt32(4);
                        DateTime dateAdded = reader.GetDateTime(5);

                        Debug.Log("Character ID: " + characterId);
                        Debug.Log("Name: " + name);
                        Debug.Log("Description: " + description);
                        Debug.Log("Silver Value: " + silverValue);
                        Debug.Log("Value: " + value);
                        Debug.Log("Date Added: " + dateAdded);
                    }
                }
            }

            connection.Close();
        }
    }
}
using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;
using Npgsql;
using System.Collections.Generic;

namespace DataServiceLayer.Services
{
    public class ActorService : IActorService
    {
        private readonly string _connectionString;

        public ActorService(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<CoActor> GetCoActors(string actorName, int pageNumber = 1, int pageSize = 10)
        {
            var coActors = new List<CoActor>();

            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand
            (@"SELECT * 
            FROM find_co_players(@actorName)
            ORDER BY frequency DESC
            LIMIT @pageSize OFFSET @offset", conn);
            cmd.Parameters.AddWithValue("actorName", actorName);
            cmd.Parameters.AddWithValue("pageSize", pageSize);
            cmd.Parameters.AddWithValue("offset", (pageNumber - 1) * pageSize);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                coActors.Add(new CoActor
                {
                    Id = reader["co_id"].ToString()!,
                    CoActorName = reader["co_actor_name"].ToString()!,
                    Frequency = (long)reader["frequency"]
                });
            }

            return coActors;
        }
    }
}

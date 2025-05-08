using Microsoft.Data.SqlClient;
using TravelAgency.Models.DTOs;
using TravelAgency.Models.Entities;


namespace TravelAgency.Repositories
{
    public class TripRepository : ITripRepository
    {
        private readonly string _connectionString;

        public TripRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<TripResponse>> GetTripsAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            
            var command = new SqlCommand(@"
                SELECT t.IdTrip, t.Name, t.Description, t.DateFrom, t.DateTo, t.MaxPeople, c.Name
                FROM Trip t
                JOIN Country_Trip ct ON t.IdTrip = ct.IdTrip
                JOIN Country c ON ct.IdCountry = c.IdCountry
                ORDER BY t.DateFrom DESC", connection);

            var trips = new Dictionary<int, TripResponse>();
            using var reader = await command.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                var tripId = reader.GetInt32(0);
                if (!trips.ContainsKey(tripId))
                {
                    trips[tripId] = new TripResponse
                    {
                        IdTrip = tripId,
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        DateFrom = reader.GetDateTime(3),
                        DateTo = reader.GetDateTime(4),
                        MaxPeople = reader.GetInt32(5),
                        Countries = new List<string>()
                    };
                }
                trips[tripId].Countries.Add(reader.GetString(6));
            }
            
            return trips.Values;
        }

        public async Task<Trip?> GetTripAsync(int tripId, SqlTransaction? transaction = null)
        {
            var connection = transaction?.Connection ?? new SqlConnection(_connectionString);
            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();
            
            var command = new SqlCommand("SELECT * FROM Trip WHERE IdTrip = @Id", connection, transaction);
            command.Parameters.AddWithValue("@Id", tripId);
            
            using var reader = await command.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return null;
            
            return new Trip
            {
                IdTrip = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2),
                DateFrom = reader.GetDateTime(3),
                DateTo = reader.GetDateTime(4),
                MaxPeople = reader.GetInt32(5)
            };
        }

        public async Task<SqlTransaction> BeginTransactionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection.BeginTransaction();
        }

        public async Task AssignClientAsync(int clientId, int tripId, DateTime date, SqlTransaction? transaction = null)
        {
            var connection = transaction?.Connection ?? new SqlConnection(_connectionString);
            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();
            
            var command = new SqlCommand(
                "INSERT INTO Client_Trip (IdClient, IdTrip, RegisteredAt) VALUES (@ClientId, @TripId, @Date)",
                connection, transaction);
            
            command.Parameters.AddWithValue("@ClientId", clientId);
            command.Parameters.AddWithValue("@TripId", tripId);
            command.Parameters.AddWithValue("@Date", int.Parse(date.ToString("yyyyMMdd")));
            
            await command.ExecuteNonQueryAsync();
        }

        public async Task<bool> RemoveClientAsync(int clientId, int tripId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            
            var command = new SqlCommand(
                "DELETE FROM Client_Trip WHERE IdClient = @ClientId AND IdTrip = @TripId", 
                connection);
            
            command.Parameters.AddWithValue("@ClientId", clientId);
            command.Parameters.AddWithValue("@TripId", tripId);
            
            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<int> GetParticipantsCountAsync(int tripId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            
            var command = new SqlCommand(
                "SELECT COUNT(*) FROM Client_Trip WHERE IdTrip = @TripId", 
                connection);
            
            command.Parameters.AddWithValue("@TripId", tripId);
            return (int)await command.ExecuteScalarAsync();
        }

        public async Task<bool> IsClientRegisteredAsync(int clientId, int tripId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            
            var command = new SqlCommand(
                "SELECT 1 FROM Client_Trip WHERE IdClient = @ClientId AND IdTrip = @TripId", 
                connection);
            
            command.Parameters.AddWithValue("@ClientId", clientId);
            command.Parameters.AddWithValue("@TripId", tripId);
            return await command.ExecuteScalarAsync() != null;
        }
        public async Task<IEnumerable<ClientTripResponse>> GetClientTripsAsync(int clientId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(@"
        SELECT 
            t.IdTrip,
            t.Name,
            t.Description,
            t.DateFrom,
            t.DateTo,
            t.MaxPeople,
            ct.RegisteredAt,
            ct.PaymentDate,
            c.Name AS CountryName
        FROM Client_Trip ct
        JOIN Trip t ON ct.IdTrip = t.IdTrip
        JOIN Country_Trip ctr ON t.IdTrip = ctr.IdTrip
        JOIN Country c ON ctr.IdCountry = c.IdCountry
        WHERE ct.IdClient = @ClientId
        ORDER BY t.DateFrom DESC", connection);

            command.Parameters.AddWithValue("@ClientId", clientId);

            var trips = new Dictionary<int, ClientTripResponse>();
            using var reader = await command.ExecuteReaderAsync();
    
            while (await reader.ReadAsync())
            {
                var tripId = reader.GetInt32(0);
                if (!trips.ContainsKey(tripId))
                {
                    trips[tripId] = new ClientTripResponse
                    {
                        IdTrip = tripId,
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        DateFrom = reader.GetDateTime(3),
                        DateTo = reader.GetDateTime(4),
                        MaxPeople = reader.GetInt32(5),
                        RegisteredAt = reader.GetInt32(6),
                        PaymentDate = reader.IsDBNull(7) ? null : (int?)reader.GetInt32(7),
                        Countries = new List<string>()
                    };
                }
                trips[tripId].Countries.Add(reader.GetString(8));
            }
    
            return trips.Values;
        }
    }
    
}
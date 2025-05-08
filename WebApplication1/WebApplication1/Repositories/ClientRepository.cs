using Microsoft.Data.SqlClient;
using TravelAgency.Models.DTOs;
// Na początku każdego pliku
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TravelAgency.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly string _connectionString;

        public ClientRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<bool> ExistsAsync(int clientId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            
            var command = new SqlCommand("SELECT 1 FROM Client WHERE IdClient = @Id", connection);
            command.Parameters.AddWithValue("@Id", clientId);
            
            return (await command.ExecuteScalarAsync()) != null;
        }

        public async Task<int> CreateClientAsync(ClientRequest request)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            
            var command = new SqlCommand(@"
                INSERT INTO Client (FirstName, LastName, Email, Telephone, Pesel) 
                OUTPUT INSERTED.IdClient
                VALUES (@FirstName, @LastName, @Email, @Telephone, @Pesel)", connection);

            command.Parameters.AddWithValue("@FirstName", request.FirstName);
            command.Parameters.AddWithValue("@LastName", request.LastName);
            command.Parameters.AddWithValue("@Email", request.Email);
            command.Parameters.AddWithValue("@Telephone", request.Telephone ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Pesel", request.Pesel ?? (object)DBNull.Value);

            return (int)await command.ExecuteScalarAsync();
        }
    }
}
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelAgency.Models.DTOs;


namespace TravelAgency.Repositories
{
    public interface IClientRepository
    {
        Task<bool> ExistsAsync(int clientId);
        Task<int> CreateClientAsync(ClientRequest request);
    }
}
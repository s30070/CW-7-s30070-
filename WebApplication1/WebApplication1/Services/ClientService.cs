using TravelAgency.Models.DTOs;
using TravelAgency.Repositories;
// Na początku każdego pliku
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TravelAgency.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly ITripRepository _tripRepository;

        public ClientService(IClientRepository clientRepository, ITripRepository tripRepository)
        {
            _clientRepository = clientRepository;
            _tripRepository = tripRepository;
        }

        public async Task<ServiceResult<IEnumerable<ClientTripResponse>>> GetClientTripsAsync(int clientId)
        {
            if (!await _clientRepository.ExistsAsync(clientId))
                return ServiceResult.Fail<IEnumerable<ClientTripResponse>>("Client not found", 404);

            var trips = await _tripRepository.GetClientTripsAsync(clientId);
            return ServiceResult.Ok(trips);
        }

        public async Task<ServiceResult<int>> CreateClientAsync(ClientRequest request)
        {
            // Walidacja danych
            if (string.IsNullOrEmpty(request.Pesel) || !IsValidPesel(request.Pesel))
                return ServiceResult.Fail<int>("Invalid PESEL format", 400);

            var clientId = await _clientRepository.CreateClientAsync(request);
            return ServiceResult.Ok(clientId);
        }

        public async Task<ServiceResult> AssignToTripAsync(int clientId, int tripId)
        {
            using var transaction = await _tripRepository.BeginTransactionAsync();
            try
            {
                if (!await _clientRepository.ExistsAsync(clientId))
                    return ServiceResult.Fail("Client not found", 404);

                var trip = await _tripRepository.GetTripAsync(tripId);
                if (trip == null)
                    return ServiceResult.Fail("Trip not found", 404);

                if (await _tripRepository.IsClientRegisteredAsync(clientId, tripId))
                    return ServiceResult.Fail("Client already registered", 400);

                if (await _tripRepository.GetParticipantsCountAsync(tripId) >= trip.MaxPeople)
                    return ServiceResult.Fail("Trip is full", 400);

                await _tripRepository.AssignClientAsync(clientId, tripId, DateTime.Now);
                await transaction.CommitAsync();
                return ServiceResult.Ok();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<ServiceResult> RemoveFromTripAsync(int clientId, int tripId)
        {
            var success = await _tripRepository.RemoveClientAsync(clientId, tripId);
            return success ? ServiceResult.Ok() : ServiceResult.Fail("Registration not found", 404);
        }

        private bool IsValidPesel(string pesel) => pesel.Length == 11 && pesel.All(char.IsDigit);
    }
}
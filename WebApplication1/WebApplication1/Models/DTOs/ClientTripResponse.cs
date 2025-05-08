// Na początku każdego pliku
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace TravelAgency.Models.DTOs
{
    public class ClientTripResponse : TripResponse
    {
        public int RegisteredAt { get; set; }
        public int? PaymentDate { get; set; }
    }
}
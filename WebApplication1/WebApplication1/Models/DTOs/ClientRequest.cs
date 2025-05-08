// Na początku każdego pliku
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TravelAgency.Models.DTOs
{
    public class ClientRequest
    {
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required][EmailAddress] public string Email { get; set; }
        public string Telephone { get; set; }
        public string Pesel { get; set; }
    }
}
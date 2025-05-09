﻿using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace TravelAgency.Models.DTOs
{
    public class TripResponse
    {
        public int IdTrip { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int MaxPeople { get; set; }
        public List<string> Countries { get; set; }
    }
}
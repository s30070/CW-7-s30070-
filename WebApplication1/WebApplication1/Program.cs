using TravelAgency.Repositories;
using TravelAgency.Services;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<ITripRepository, TripRepository>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<ITripService, TripService>(); // Dodana rejestracja

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
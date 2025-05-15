using Microsoft.Data.SqlClient;
using Tutorial8.Models;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Repositories;

public class TripRepository : ITripRepository
{
    private readonly string _connectionString;

    public TripRepository(IConfiguration connectionString)
    {
        _connectionString = connectionString.GetConnectionString("Lecture7Db");
    }

    public async Task<IEnumerable<ClientTrip>> GetTripsByClientIdAsync(int id, CancellationToken cancellationToken)
    {
        var trips = new List<ClientTrip>();

        await using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            
            var query = @"SELECT 
                            o.FirstName,
                            o.LastName, o.IdClient,
                            ct.RegisteredAt, ct.PaymentDate, t.IdTrip, t.Description,t.DateFrom,
                            t.DateTo, t.Name AS TripName, c.Name AS CountryName FROM Client o 
                            JOIN Client_Trip ct ON ct.IdClient = o.IdClient 
                            JOIN Trip t On ct.IdTrip = t.IdTrip
                            JOIN Country_Trip ctr ON ctr.IdTrip = t.IdTrip
                            JOIN Country c ON c.IdCountry = ctr.IdCountry WHERE o.IdClient = @IdClient";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@IdClient", id);

                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    var tripsDict = new Dictionary<int, ClientTrip>();
                    
                    while (await reader.ReadAsync(cancellationToken)) //iteruje
                    {
                        var tripId = reader.GetInt32(reader.GetOrdinal("IdTrip")); //pobiera id danej wycieczki
                        
                        if (!tripsDict.ContainsKey(tripId)) //tworzenie obiektu
                        {
                            var client = new Client
                            {
                                IdClient = reader.GetInt32(reader.GetOrdinal("IdClient")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName"))
                            };
                            var trip = new Trip
                            {
                                IdTrip = tripId,
                                Name = reader.GetString(reader.GetOrdinal("TripName")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                DateFrom = reader.GetDateTime(reader.GetOrdinal("DateFrom")),
                                DateTo = reader.GetDateTime(reader.GetOrdinal("DateTo")),
                                Countries = new List<Country>()
                            };

                            var clientTrip = new ClientTrip
                            {
                                Client = client,
                                Trip = trip,
                                RegisteredAt = reader.GetInt32(reader.GetOrdinal("RegisteredAt")),
                                PaymentDate = reader.IsDBNull(reader.GetOrdinal("PaymentDate"))
                                    ? (int?)null
                                    : reader.GetInt32(reader.GetOrdinal("PaymentDate"))
                            };

                            tripsDict[tripId] = clientTrip;
                        }
                        
                        if (!reader.IsDBNull(reader.GetOrdinal("CountryName")))
                        {
                            var country = new Country
                            {
                                Name = reader.GetString(reader.GetOrdinal("CountryName"))
                            };

                            tripsDict[tripId].Trip.Countries.Add(country);
                        }
                    }
                    trips = tripsDict.Values.ToList();
                }
                //trips = tripsDict.Values.ToList();
            }
            return trips;
        }
        //return trips;
    }

    public async Task<IEnumerable<Trip>> GetAllTripsAsync(CancellationToken cancellationToken)
    {
        var trips = new List<Trip>();

        await using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);

            var query = @"SELECT 
                t.IdTrip,
                t.Name AS TripName,
                t.Description,
                t.DateFrom,
                t.DateTo,
                t.MaxPeople,
                c.Name AS CountryName
                FROM Trip t
                JOIN Country_Trip ct ON ct.IdTrip = t.IdTrip
                JOIN Country c ON c.IdCountry = ct.IdCountry
                ORDER BY t.IdTrip;
                ";


            using (var command = new SqlCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    var tripsDict = new Dictionary<int, Trip>();
                    
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        var tripId = reader.GetInt32(reader.GetOrdinal("IdTrip"));

                        if (!tripsDict.ContainsKey(tripId))
                        {
                            var trip = new Trip
                            {
                                IdTrip = reader.GetInt32(reader.GetOrdinal("IdTrip")),
                                Name = reader.GetString(reader.GetOrdinal("TripName")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                DateFrom = reader.GetDateTime(reader.GetOrdinal("DateFrom")),
                                DateTo = reader.GetDateTime(reader.GetOrdinal("DateTo")),
                                MaxPeople = reader.GetInt32(reader.GetOrdinal("MaxPeople")),
                                Countries = new List<Country>()
                            };
                            
                            tripsDict[tripId] = trip;
                        }
                        
                        var country = new Country
                        {
                            Name = reader.GetString(reader.GetOrdinal("CountryName"))
                        };
                        tripsDict[tripId].Countries.Add(country);
                    }
                    trips = tripsDict.Values.ToList();
                }
            }
            return trips;
        }
        
    }

    public async Task<bool> TripExistsAsync(int IdTrip, CancellationToken cancellationToken)
    {
        string sql = @"SELECT 1 FROM Trip WHERE IdTrip = @IdTrip";
        
        await using var con = new SqlConnection(_connectionString);
        await con.OpenAsync(cancellationToken);
        
        await using var com = new SqlCommand(sql, con);
        com.Parameters.AddWithValue("@IdTrip", IdTrip);
        
        return await com.ExecuteScalarAsync(cancellationToken) is not null;
    }

    public async Task<bool> IsTripFullAsync(int IdTrip, CancellationToken cancellationToken)
    {
        string sql = @"SELECT COUNT(*) FROM Client_Trip WHERE IdTrip = @IdTrip;
                    SELECT MaxPeople From Trip WHERE IdTrip = @IdTrip;";
        
        await using var con = new SqlConnection(_connectionString);
        await con.OpenAsync(cancellationToken);
        
        await using var com = new SqlCommand(sql, con);
        com.Parameters.AddWithValue("@IdTrip", IdTrip);
        
        var reader = await com.ExecuteReaderAsync(cancellationToken);

        int registered = 0;
        int max = 0;
        
        if (await reader.ReadAsync(cancellationToken))
            registered = reader.GetInt32(0);

        if (await reader.NextResultAsync(cancellationToken) && await reader.ReadAsync(cancellationToken))
            max = reader.GetInt32(0);

        return registered >= max;
    }

}


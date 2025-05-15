using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Tutorial8.Models;

namespace Tutorial8.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly string _connectionString;

    public ClientRepository(IConfiguration connectionString)
    {
        _connectionString = connectionString.GetConnectionString("Lecture7Db");
    }

    public async Task<int> AddClientAsync(Client client, CancellationToken cancellationToken)
    {
        await using var con = new SqlConnection(_connectionString);
        await using var com = new SqlCommand();
        com.Connection = con;
        
        string sql = @"
        INSERT INTO Client(FirstName, LastName, Email, Telephone ,Pesel)
        Values (@FirstName, @LastName, @Email, @Telephone ,@Pesel);
        SELECT SCOPE_IDENTITY();
        ";
        
        com.CommandText = sql;
        
        com.Parameters.AddWithValue("@FirstName", client.FirstName);
        com.Parameters.AddWithValue("@LastName", client.LastName);
        com.Parameters.AddWithValue("@Email", client.Email);
        com.Parameters.AddWithValue("@Telephone", client.Telephone);
        com.Parameters.AddWithValue("@Pesel", client.Pesel);
        
        await con.OpenAsync();
        
        var idCustomer = Convert.ToInt32(await com.ExecuteScalarAsync(cancellationToken));
        
        return idCustomer;
    }

    public async Task<bool> ClientExistsAsync(int IdClient, CancellationToken cancellationToken)
    {
        string sql = @"SELECT 1 FROM Client WHERE IdClient = @IdClient";
        
        await using var con = new SqlConnection(_connectionString);
        await con.OpenAsync(cancellationToken);
        
        await using var com = new SqlCommand(sql, con);
        com.Parameters.AddWithValue("@IdClient", IdClient);
        
        return await com.ExecuteScalarAsync(cancellationToken) is not null;
    }

    public async Task<bool> IsClientRegisteredAsync(int IdClient, int IdTrip, CancellationToken cancellationToken)
    {
        string sql = @"Select 1 from Client_Trip where IdClient = @IdClient and IdTrip = @IdTrip";
        
        await using var con = new SqlConnection(_connectionString);
        await con.OpenAsync(cancellationToken);
        
        await using var com = new SqlCommand(sql, con);
        com.Parameters.AddWithValue("@IdClient", IdClient);
        com.Parameters.AddWithValue("@IdTrip", IdTrip);
        
        return await com.ExecuteScalarAsync(cancellationToken) is not null;
    }

    public async Task RegisterClientToTripAsync(int IdClient, int IdTrip, CancellationToken cancellationToken)
    {
        string sql = @"INSERT INTO Client_Trip (IdClient, IdTrip, RegisteredAt) VALUES (@IdClient, @IdTrip, @RegisteredAt)";
        await using var con = new SqlConnection(_connectionString);
        await con.OpenAsync(cancellationToken);
        
        await using var com = new SqlCommand(sql, con);
        com.Parameters.AddWithValue("@IdClient", IdClient);
        com.Parameters.AddWithValue("@IdTrip", IdTrip);
        com.Parameters.AddWithValue("@RegisteredAt", DateTime.Now.ToString());
        
        await com.ExecuteNonQueryAsync(cancellationToken);
    }
}
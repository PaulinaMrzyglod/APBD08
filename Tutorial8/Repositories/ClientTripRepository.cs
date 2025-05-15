using Microsoft.Data.SqlClient;

namespace Tutorial8.Repositories;

public class ClientTripRepository : IClientTripRepository
{
    private readonly string _connectionString;

    public ClientTripRepository(IConfiguration connectionString)
    {
        _connectionString = connectionString.GetConnectionString("Lecture7Db");
    }

    public async Task<bool> DeleteClientTripAsync(int IdClient, int IdTrip, CancellationToken cancellationToken)
    {
        string checkQuery = @"SELECT 1 FROM Client_Trip WHERE IdClient = @IdClient AND IdTrip = @IdTrip";
        string deleteQuery = @"DELETE FROM Client_Trip WHERE IdClient = @IdClient AND IdTrip = @IdTrip";
        
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using (var checkCommand = new SqlCommand(checkQuery, connection))
        {
            checkCommand.Parameters.AddWithValue("@IdClient", IdClient);
            checkCommand.Parameters.AddWithValue("@IdTrip", IdTrip);

            if (await checkCommand.ExecuteScalarAsync(cancellationToken) is null)
            {
                return false;
            }
        }

        await using (var deleteCommand = new SqlCommand(deleteQuery, connection))
        {
            deleteCommand.Parameters.AddWithValue("@IdClient", IdClient);
            deleteCommand.Parameters.AddWithValue("@IdTrip", IdTrip);
            await deleteCommand.ExecuteNonQueryAsync(cancellationToken);
        }
        
        return true;
    }
}
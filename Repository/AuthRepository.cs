using Dapper;
using Microsoft.Data.SqlClient;
using RestAPIPractica.Entity;
using System.Data;

namespace RestAPIPractica.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IDbConnection _connection;

        public AuthRepository(ConnectionService connection)
        {
            string connsql = connection.getConnection();
            _connection = new SqlConnection(connsql);
        }

        public async Task<dynamic> Auth(string correo, string password)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Correo", correo);
            parameters.Add("@Password", password);

            
            IEnumerable<dynamic> lista = await _connection.QueryAsync<dynamic>(
                "RestAPIPractica", 
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return lista.ToList();
        }

        public Task<Auth> GetUserFromRefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}

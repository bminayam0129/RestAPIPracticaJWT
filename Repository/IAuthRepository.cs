using RestAPIPractica.Entity;

namespace RestAPIPractica.Repository
{
    public interface IAuthRepository
    {
        public Task<dynamic> Auth(string correo, string password);
        Task<Auth> GetUserFromRefreshToken(string refreshToken);
    }
}

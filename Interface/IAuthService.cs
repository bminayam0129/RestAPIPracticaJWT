
using RestAPIPractica.Entity;

namespace RestAPIPractica.Interface
{
    public interface IAuthService
    {
        Task<Auth> Auth(string correo, string password);
    }
}

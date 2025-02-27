using RestAPIPractica.Entity;
using RestAPIPractica.Interface;

using RestAPIPractica.Repository;

namespace RestAPIPractica.Service
{
    public class AuthService : IAuthService
    {
        private readonly IjwtService _jwtService;
        private readonly IAuthRepository _authRepository;

        public AuthService(IjwtService jwtService, IAuthRepository authRepository)
        {
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
            _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
        }

        public async Task<Auth> Auth(string correo, string password)
        {
            var user = await _authRepository.Auth(correo, password);
            if (user != null)
            {
                user.Token = _jwtService.generateToken(user.Email);
            }
            else
            {
                user = new Auth();  // Devolver un objeto vacío si no se encuentra usuario
            }
            return user;
        }

    }
}

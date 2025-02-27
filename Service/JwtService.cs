using Microsoft.IdentityModel.Tokens;
using RestAPIPractica.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestAPIPractica.Service
{
    public class JwtService : IjwtService
    {
        private readonly IConfiguration _configuration;

        // Acceder a las variables de entorno
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Método para generar token 
        public string generateToken(string user)
        {
            if (string.IsNullOrEmpty(user))
            {
                throw new ArgumentNullException(nameof(user), "El usuario no puede ser nulo o vacío.");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user)
            };

            // Obtener la clave secreta y validar que no sea null
            var secretKey = _configuration["Jwt:SecretKey"] ?? throw new ArgumentNullException("Jwt:SecretKey", "La clave secreta no está configurada.");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["Jwt:ExpirationHours"] ?? "1")), // Valor por defecto 1 hora si está null
                signingCredentials: credentials
            );

            string jwt = new JwtSecurityTokenHandler().WriteToken(token);
            Console.WriteLine(jwt);

            return jwt;
        }
    }
}

using Microsoft.Extensions.Configuration;

namespace RestAPIPractica
{
    public class ConnectionService
    {
        private readonly IConfiguration _configuration;

        // Constructor que inyecta IConfiguration
        public ConnectionService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Método que devuelve la cadena de conexión desde la configuración
        public string getConnection()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            return connectionString;
        }
    }
}

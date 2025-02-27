using Microsoft.EntityFrameworkCore;
using RestAPIPractica.RestAPI.Models;

namespace RestAPIPractica.RestAPI.Data
{
    public class RestAPIContext: DbContext
    {
        public RestAPIContext(DbContextOptions<RestAPIContext> options) : base(options) 
        {
        }
        
        public DbSet<Usuario> Usuarios { get; set; }
    }
}

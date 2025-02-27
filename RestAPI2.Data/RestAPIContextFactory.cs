using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using RestAPIPractica.RestAPI.Data;

public class RestAPIContextFactory : IDesignTimeDbContextFactory<RestAPIContext>
{
    public RestAPIContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<RestAPIContext>();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        optionsBuilder.UseSqlServer(configuration.GetConnectionString("PracticaRestAPI"));

        return new RestAPIContext(optionsBuilder.Options);
    }
}
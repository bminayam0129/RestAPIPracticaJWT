using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RestAPIPractica.Interface;
using RestAPIPractica.Repository;
using RestAPIPractica.RestAPI.Data;
using RestAPIPractica.Service;
using System.Text;

namespace RestAPIPractica
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            Console.WriteLine("SecretKey: " + builder.Configuration["Jwt:SecretKey"]);

            // Configurar DbContext
            builder.Services.AddDbContext<RestAPIContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("PracticaRestAPI")));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSingleton<ConnectionService>();
            builder.Services.AddScoped<IjwtService, JwtService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();

            // Configurar Swagger con autenticación JWT
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RestAPIPractica", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Ingrese el token JWT en el formato 'Bearer {token}'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            // Validar la clave secreta
            string? secretKey = builder.Configuration["Jwt:SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("La clave secreta 'Jwt:SecretKey' no está configurada en appsettings.json.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            // Configurar autenticación JWT
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.Zero
                };
            });

            var app = builder.Build();

            // Configurar el pipeline HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}

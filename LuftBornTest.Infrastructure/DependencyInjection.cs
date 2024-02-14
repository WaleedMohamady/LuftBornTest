using LuftBornTest.Application.Interfaces;
using LuftBornTest.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace LuftBornTest.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {

        #region Cors
        services.AddCors(options =>
        {
            options.AddPolicy(name: "CorsPolicy",
                policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
        });
        #endregion

        #region Controllers and Swagger
        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        #endregion

        #region Context
        var connectionString = configuration.GetConnectionString("LuftBornTestDb");
        services
            .AddDbContext<ILuftBornTestDbContext, LuftBornTestDbContext>(options => options.UseSqlServer(connectionString));
        #endregion

        #region ASP Identity
        services.AddIdentity<User, IdentityRole>(options =>
        {
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 5;

            options.User.RequireUniqueEmail = true;

            options.Lockout.MaxFailedAccessAttempts = 3;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
        })
            .AddEntityFrameworkStores<LuftBornTestDbContext>();
        #endregion

        #region Authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = "default";
            options.DefaultChallengeScheme = "default";
        }).
            AddJwtBearer("default", options =>
            {
                var secretKey = configuration.GetValue<string>("SecretKey");
                var secretKeyInBytes = Encoding.ASCII.GetBytes(secretKey);
                var key = new SymmetricSecurityKey(secretKeyInBytes);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = key
                };
            });
        #endregion

        #region Authorization

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AllowAdminAndManager", policy =>
                policy
                .RequireClaim(ClaimTypes.Role, "Administrator", "Manager"));
        });

        #endregion

        return services;
    }
}

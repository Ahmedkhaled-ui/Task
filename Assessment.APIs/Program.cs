using Application.DTOS.Auth;
using Application.Interfaces;
using Assessment.APIs.Middlewares;
using Infrastructure.Data;
using Infrastructure.DependencyInjections;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Scalar.AspNetCore;
using System.Text;
using System.Text.Json.Serialization;

namespace Assessment.APIs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ==========================================
            // 1. Register Services (DI Container)
            // ==========================================

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // بيخلي الـ Enums تظهر كلمات (Pending, High) بدل أرقام
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            builder.Services.AddInfrastructureServices(builder.Configuration);

            // Identity Configuration
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // OpenAPI / Scalar Configuration for .NET 9
            builder.Services.AddOpenApi(options =>
            {
                options.AddSchemaTransformer((schema, context, cancellationToken) =>
                {
                    if (context.JsonTypeInfo.Type.IsEnum)
                    {
                        schema.Type = "string";
                        schema.Format = null;
                        schema.Enum.Clear();

                        foreach (var enumName in Enum.GetNames(context.JsonTypeInfo.Type))
                        {
                            schema.Enum.Add(new OpenApiString(enumName));
                        }
                    }
                    return Task.CompletedTask;
                });
            });

            // JWT Authentication Configuration
            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                var jwt = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = jwt.audience,
                    ValidIssuer = jwt.issure,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key))
                };
            });

            // ==========================================
            // 2. Build App & Configure HTTP Pipeline (Middlewares)
            // ==========================================

            var app = builder.Build();

            app.UseCustomExceptionMiddleware();           
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization(); 

            app.MapControllers();

            app.Run();
        }
    }
}
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SchoolHub.Common.Data;
using SchoolHub.Common.Models.Entities.Users;
using SchoolHub.Common.Models.Mappings;
using SchoolHub.Common.Repositories.Implementation;
using SchoolHub.Common.Repositories.Interfaces;
using SchoolHub.Common.Services.Implementation;
using SchoolHub.Common.Services.Interfaces;
using System.Text;

namespace SchoolHub.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                  policy =>
                  {
                      policy.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                  });
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SchoolHub.Api", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Example: Bearer {Token JWT here} ",
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
                         new string[] {}
                    }
                });
            });

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddScoped<IClassGroupRepository, ClassGroupRepository>();
            builder.Services.AddScoped<ITennantRepository, TennantRepository>();
            builder.Services.AddScoped<ITokenRepository, TokenRepository>();

            builder.Services.AddScoped<ISeedService, SeedService>();

            builder.Services.AddSingleton(RT.Comb.Provider.Sql);
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddIdentityCore<User>()
               .AddRoles<Role>()
               .AddTokenProvider<DataProtectorTokenProvider<User>>("SchoolApi")
               .AddEntityFrameworkStores<AppDbContext>()
               .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       AuthenticationType = "Jwt",
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = builder.Configuration["Jwt:Issuer"],
                       ValidAudience = builder.Configuration["Jwt:Audience"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                   };
               });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            using (var scope = app.Services.CreateScope())
            {
                var seedUserService = scope.ServiceProvider.GetRequiredService<ISeedService>();
                seedUserService.Seed();
            }

            app.MapControllers();

            app.Run();
        }
    }
}

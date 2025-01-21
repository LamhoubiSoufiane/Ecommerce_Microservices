using AuthentificationService.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using AuthentificationService.Interfaces;
using AuthentificationService.Services;
using Microsoft.Extensions.Caching.StackExchangeRedis;


namespace AuthentificationService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("Redis");
                options.InstanceName = "AuthService_"; // Préfixe pour les clés Redis
            });
            services.AddScoped<ITokenBlacklistService, TokenBlacklistService>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            services.AddDbContext<EcommerceAuthDb>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            
            services.AddIdentity<IdentityUser, IdentityRole>(options => {
                // Configuration du mot de passe
                options.Password.RequireDigit = true; // Requiert au moins un chiffre
                options.Password.RequireLowercase = true; // Requiert au moins une minuscule
                options.Password.RequireUppercase = true; // Requiert au moins une majuscule
                options.Password.RequireNonAlphanumeric = false; // Ne requiert pas de caractère spécial
                options.Password.RequiredLength = 6; // Longueur minimale de 6 caractères
                options.Password.RequiredUniqueChars = 1; // Au moins 1 caractère unique

                // Configuration de l'utilisateur
                options.User.RequireUniqueEmail = true; // Email doit�tre unique
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"; // Caractères autorisés pour le nom d'utilisateur

                // Configuration de la connexion
                options.SignIn.RequireConfirmedAccount = false; // Pas besoin de confirmer le compte
                options.SignIn.RequireConfirmedEmail = false; // Pas besoin de confirmer l'email
                options.SignIn.RequireConfirmedPhoneNumber = false; // Pas besoin de confirmer le téléphone
            })
            .AddEntityFrameworkStores<EcommerceAuthDb>()
            .AddDefaultTokenProviders();

            // Configuration de l'authentification
            var jwtSettings = Configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });
           
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthentificationService", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
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
                        Array.Empty<string>()
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthentificationService v1"));
            }

            app.UseRouting();

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Initialiser la base de données
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<EcommerceAuthDb>();
                    context.Database.Migrate();

                    // Créer le rôle User s'il n'existe pas
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    if (!roleManager.RoleExistsAsync("User").Result)
                    {
                        var role = new IdentityRole("User");
                        roleManager.CreateAsync(role).Wait();
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Startup>>();
                    logger.LogError(ex, "Une erreur s'est produite lors de l'initialisation de la base de données.");
                }
            }
        }
    }
}

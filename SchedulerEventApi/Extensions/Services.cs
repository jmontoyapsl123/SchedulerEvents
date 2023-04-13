using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using ScedulerEventDomain.Services.Implementations;
using ScedulerEventDomain.Services.Interfaces;
using SchedulerEventApi.Validations;
using SchedulerEventCommon.Utilities;
using SchedulerEventDomain.Services.Implementations;
using SchedulerEventDomain.Services.Interfaces;
using SchedulerEventRepositories.Repositories.Implementations;
using SchedulerEventRepositories.Repositories.Interfaces;

namespace SchedulerEventApi.Extensions
{
    public static class ServicesExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining(typeof(LoginValidator));

            services.AddScoped<IDeveloperRepository, DeveloperRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IEventInvitationRepository, EventInvitationRepository>();

            services.AddTransient<IDeveloperService, DeveloperService>();
            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<IPasswordUtility, PasswordUtility>();
            services.AddTransient<IEventService, EventService>();
            services.AddTransient<IWeatherstackService, WeatherstackService>();
            services.AddTransient<IEventInvitationService, EventInvitationService>();
            services.AddTransient<ISendEmailService, SendEmailService>();
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            });
        }

        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerExtension>();
        }

        public static void ConfigureAuthentication(this IServiceCollection services, string token)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                    };
                });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        @$"JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. 
                        {Environment.NewLine}Example: 'Bearer XXXXXXXXXXXXXXXXXXXX'",
                    Name = HeaderNames.Authorization,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[]{}
                    }
                });
            });
        }
    }
}
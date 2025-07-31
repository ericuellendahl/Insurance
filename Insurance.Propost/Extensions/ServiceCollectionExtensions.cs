using FluentValidation;
using Insurance.Propost.Adapter.Repositories;
using Insurance.Propost.Application.DTOs;
using Insurance.Propost.Application.Interfaces;
using Insurance.Propost.Application.Services;
using Insurance.Propost.Application.UseCase;
using Insurance.Propost.Application.Validatons;
using Insurance.Propost.Domain.Ports;
using Insurance.Propost.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Insurance.Propost.Extensions
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPropostPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("PostgreConnection");

            services.AddDbContext<PropostDbContext>(options =>
                options.UseNpgsql(connectionString, npgsqlOptions =>
                    npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "public"))
                       .UseSnakeCaseNamingConvention());

            services.AddScoped<IPropostRepository, PropostRepository>();

            return services;
        }

        public static IServiceCollection AddPropostApplication(this IServiceCollection services)
        {
            // Use Cases
            services.AddScoped<CreatePropostUseCase>();
            services.AddScoped<ChangePropostStatusUseCase>();

            // Application Services
            services.AddScoped<IPropostService, PropostService>();

            return services;
        }

        public static IServiceCollection AddPropostValidation(this IServiceCollection services)
        {
            services.AddScoped<IValidator<ChangePropostStatusRequest>, ChangePropostStatusRequestValidator>();
            services.AddScoped<IValidator<CreatePropostRequest>, CreatePropostRequestValidator>();

            return services;
        }
    }
}

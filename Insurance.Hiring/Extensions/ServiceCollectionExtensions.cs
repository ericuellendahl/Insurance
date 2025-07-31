using FluentValidation;
using Insurance.Hiring.Adapter.ExternalServices;
using Insurance.Hiring.Adapter.Repositories;
using Insurance.Hiring.Application.DTOs;
using Insurance.Hiring.Application.Interfaces;
using Insurance.Hiring.Application.Services;
using Insurance.Hiring.Application.UseCase;
using Insurance.Hiring.Application.Validations;
using Insurance.Hiring.Domain.Ports;
using Insurance.Hiring.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Insurance.Hiring.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPropostPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("PostgreConnection");

            services.AddDbContext<ContractDbContext>(options =>
                options.UseNpgsql(connectionString, npgsqlOptions =>
                    npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "public"))
                       .UseSnakeCaseNamingConvention());

            services.AddScoped<IContractRepository, ContractRepository>();

            return services;
        }

        public static IServiceCollection AddPropostApplication(this IServiceCollection services)
        {
            services.AddScoped<CreateContractUseCase>();
            services.AddScoped<GetContractPropostUseCase>();
            services.AddScoped<IContractService, ContractService>();
            services.AddScoped<IPropostServiceClient, PropostServiceClient>();

            return services;
        }

        public static IServiceCollection AddContractValidation(this IServiceCollection services)
        {
            services.AddScoped<IValidator<ContractPropostRequest>, ContractProposalRequestValidator>();

            return services;
        }
    }
}

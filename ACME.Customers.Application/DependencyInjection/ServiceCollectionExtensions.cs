using ACME.Customers.Application.Interfaces;
using ACME.Customers.Application.Mapping;
using ACME.Customers.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ACME.Customers.Application.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<ISalesRepService, SalesRepService>();

            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            services.AddValidatorsFromAssembly(typeof(MappingProfile).Assembly);

            return services;
        }
    }
}
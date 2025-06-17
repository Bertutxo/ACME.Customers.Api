using ACME.Customers.Core.Interfaces;
using ACME.Customers.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ACME.Customers.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<ISalesRepRepository, SalesRepRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
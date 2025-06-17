using ACME.Customers.Core.Interfaces;
using ACME.Customers.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ACME.Customers.Infrastructure.DependencyInjection
{
    /// <summary>
    /// Proporciona extensiones para <see cref="IServiceCollection"/>,
    /// registrando los servicios de infraestructura (repositorios y Unit of Work).
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registra los componentes de infraestructura en el contenedor de servicios:
        /// <list type="bullet">
        ///   <item><description><see cref="IClientRepository"/> -> <see cref="ClientRepository"/></description></item>
        ///   <item><description><see cref="ISalesRepRepository"/> -> <see cref="SalesRepRepository"/></description></item>
        ///   <item><description><see cref="IUnitOfWork"/> -> <see cref="UnitOfWork"/></description></item>
        /// </list>
        /// </summary>
        /// <param name="services">Colección de servicios de la aplicación.</param>
        /// <returns>La misma instancia de <see cref="IServiceCollection"/>, permitiendo encadenar llamadas.</returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Repositorio de clientes
            services.AddScoped<IClientRepository, ClientRepository>();

            // Repositorio de comerciales
            services.AddScoped<ISalesRepRepository, SalesRepRepository>();

            // Unidad de trabajo para coordinar transacciones
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
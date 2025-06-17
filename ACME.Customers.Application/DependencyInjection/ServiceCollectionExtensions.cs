using ACME.Customers.Application.Interfaces;
using ACME.Customers.Application.Mapping;
using ACME.Customers.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ACME.Customers.Application.DependencyInjection
{
    /// <summary>
    /// Proporciona métodos de extensión para <see cref="IServiceCollection"/>,
    /// registrando los servicios de la capa de aplicación, perfiles de mapeo y validadores.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registra en el contenedor de servicios:
        /// <list type="bullet">
        ///   <item>
        ///     <description>
        ///       <see cref="IClientService"/> con implementación <see cref="ClientService"/>
        ///     </description>
        ///   </item>
        ///   <item>
        ///     <description>
        ///       <see cref="ISalesRepService"/> con implementación <see cref="SalesRepService"/>
        ///     </description>
        ///   </item>
        ///   <item>
        ///     <description>
        ///       Mapas de AutoMapper cargados desde <see cref="MappingProfile"/>
        ///     </description>
        ///   </item>
        ///   <item>
        ///     <description>
        ///       Validadores de FluentValidation registrados desde el ensamblado
        ///     </description>
        ///   </item>
        /// </list>
        /// </summary>
        /// <param name="services">Colección de servicios a la que se añadirán los componentes.</param>
        /// <returns>
        /// La misma instancia de <see cref="IServiceCollection"/>, permitiendo encadenar
        /// llamadas de extensión.
        /// </returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Servicios de aplicación (casos de uso)
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<ISalesRepService, SalesRepService>();

            // Configuración de AutoMapper: carga perfiles del ensamblado de mapeo
            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            // Registro de validadores FluentValidation desde el mismo ensamblado
            services.AddValidatorsFromAssembly(typeof(MappingProfile).Assembly);

            return services;
        }
    }
}
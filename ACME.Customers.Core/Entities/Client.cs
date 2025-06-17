// Client.cs
using System;

namespace ACME.Customers.Core.Entities
{
    /// <summary>
    /// Entidad que representa a un cliente visitado por un comercial de ACME.
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Identificador único del cliente.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nombre de la empresa o cliente.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Email de contacto del cliente.
        /// </summary>
        public string ContactEmail { get; set; } = string.Empty;

        /// <summary>
        /// Fecha en la que se realizó la visita.
        /// </summary>
        public DateTime VisitDate { get; set; }

        /// <summary>
        /// Identificador del comercial responsable de la visita.
        /// </summary>
        public Guid SalesRepId { get; set; }

        /// <summary>
        /// Navegación al comercial responsable.
        /// </summary>
        public SalesRep SalesRep { get; set; } = null!;

        /// <summary>
        /// Notas adicionales de la visita.
        /// </summary>
        public string? Notes { get; set; }
    }
}
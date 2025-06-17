// SalesRep.cs
using System;
using System.Collections.Generic;

namespace ACME.Customers.Core.Entities
{
    /// <summary>
    /// Entidad que representa a un comercial de ACME.
    /// </summary>
    public class SalesRep
    {
        /// <summary>
        /// Identificador único del comercial.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nombre completo del comercial.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Email de contacto del comercial.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Teléfono de contacto (opcional).
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// Lista de clientes visitados por este comercial.
        /// </summary>
        public ICollection<Client> Clients { get; set; } = new List<Client>();
    }
}
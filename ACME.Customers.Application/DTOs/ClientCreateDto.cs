namespace ACME.Customers.Application.DTOs
{
    /// <summary>
    /// DTO para la creación de un nuevo Cliente.
    /// </summary>
    public class ClientCreateDto
    {
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
        /// Identificador del comercial responsable.
        /// </summary>
        public Guid SalesRepId { get; set; }

        /// <summary>
        /// Notas o comentarios adicionales de la visita.
        /// </summary>
        public string? Notes { get; set; }
    }
}
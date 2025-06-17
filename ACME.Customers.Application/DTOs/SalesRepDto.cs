namespace ACME.Customers.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object para exponer la información de un Comercial (Sales Rep).
    /// </summary>
    public class SalesRepDto
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
    }
}
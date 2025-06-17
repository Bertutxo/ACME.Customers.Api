namespace ACME.Customers.Application.DTOs
{
    /// <summary>
    /// DTO para la actualización de un Comercial existente.
    /// </summary>
    public class SalesRepUpdateDto
    {
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
using ACME.Customers.Application.DTOs;

namespace ACME.Customers.Application.Interfaces
{
    /// <summary>
    /// Servicios de aplicación para gestionar Clientes y sus visitas.
    /// </summary>
    public interface IClientService
    {
        /// <summary>
        /// Obtiene todos los clientes registrados.
        /// </summary>
        /// <returns>Lista de ClientDto.</returns>
        Task<IEnumerable<ClientDto>> GetAllAsync();

        /// <summary>
        /// Obtiene un cliente por su Id.
        /// </summary>
        /// <param name="id">Identificador del cliente.</param>
        /// <returns>ClientDto si existe o null.</returns>
        Task<ClientDto?> GetByIdAsync(Guid id);

        /// <summary>
        /// Crea un nuevo cliente con los datos proporcionados.
        /// </summary>
        /// <param name="dto">Datos para creación de cliente.</param>
        /// <returns>El Id del cliente recién creado.</returns>
        Task<Guid> CreateAsync(ClientCreateDto dto);

        /// <summary>
        /// Actualiza un cliente existente.
        /// </summary>
        /// <param name="id">Identificador del cliente a actualizar.</param>
        /// <param name="dto">Datos de actualización.</param>
        /// <returns>True si se actualizó correctamente, false si no existe.</returns>
        Task<bool> UpdateAsync(Guid id, ClientUpdateDto dto);

        /// <summary>
        /// Elimina un cliente por su Id.
        /// </summary>
        /// <param name="id">Identificador del cliente a eliminar.</param>
        /// <returns>True si se eliminó correctamente, false si no existe.</returns>
        Task<bool> DeleteAsync(Guid id);
    }
}
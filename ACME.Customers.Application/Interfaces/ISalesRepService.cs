using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ACME.Customers.Application.DTOs;

namespace ACME.Customers.Application.Interfaces
{
    /// <summary>
    /// Servicios de aplicación para gestionar Comerciales (Sales Reps).
    /// </summary>
    public interface ISalesRepService
    {
        /// <summary>
        /// Obtiene todos los comerciales registrados.
        /// </summary>
        /// <returns>Lista de SalesRepDto.</returns>
        Task<IEnumerable<SalesRepDto>> GetAllAsync();

        /// <summary>
        /// Obtiene un comercial por su Id.
        /// </summary>
        /// <param name="id">Identificador del comercial.</param>
        /// <returns>SalesRepDto si existe o null.</returns>
        Task<SalesRepDto?> GetByIdAsync(Guid id);

        /// <summary>
        /// Crea un nuevo comercial con los datos proporcionados.
        /// </summary>
        /// <param name="dto">Datos para creación del comercial.</param>
        /// <returns>El Id del comercial recién creado.</returns>
        Task<Guid> CreateAsync(SalesRepCreateDto dto);

        /// <summary>
        /// Actualiza un comercial existente.
        /// </summary>
        /// <param name="id">Identificador del comercial a actualizar.</param>
        /// <param name="dto">Datos de actualización.</param>
        /// <returns>True si se actualizó correctamente, false si no existe.</returns>
        Task<bool> UpdateAsync(Guid id, SalesRepUpdateDto dto);

        /// <summary>
        /// Elimina un comercial por su Id.
        /// </summary>
        /// <param name="id">Identificador del comercial a eliminar.</param>
        /// <returns>True si se eliminó correctamente, false si no existe.</returns>
        Task<bool> DeleteAsync(Guid id);
    }
}
using ACME.Customers.Core.Entities;

namespace ACME.Customers.Core.Interfaces
{
    /// <summary>
    /// Repositorio para la entidad <see cref="SalesRep"/>.
    /// </summary>
    public interface ISalesRepRepository
    {
        /// <summary>
        /// Obtiene todos los comerciales.
        /// </summary>
        /// <returns>Lista de <see cref="SalesRep"/>.</returns>
        Task<IEnumerable<SalesRep>> GetAllAsync();

        /// <summary>
        /// Obtiene un comercial por su identificador.
        /// </summary>
        /// <param name="id">Identificador del comercial.</param>
        /// <returns><see cref="SalesRep"/> si existe; <c>null</c> en caso contrario.</returns>
        Task<SalesRep?> GetByIdAsync(Guid id);

        /// <summary>
        /// Agrega un nuevo comercial.
        /// </summary>
        /// <param name="salesRep">Instancia de <see cref="SalesRep"/> a agregar.</param>
        /// <returns>Una tarea asincrónica.</returns>
        Task AddAsync(SalesRep salesRep);

        /// <summary>
        /// Actualiza un comercial existente.
        /// </summary>
        /// <param name="salesRep">Instancia de <see cref="SalesRep"/> con datos actualizados.</param>
        void Update(SalesRep salesRep);

        /// <summary>
        /// Elimina un comercial.
        /// </summary>
        /// <param name="salesRep">Instancia de <see cref="SalesRep"/> a eliminar.</param>
        void Delete(SalesRep salesRep);

        /// <summary>
        /// Verifica si un comercial existe por su identificador.
        /// </summary>
        /// <param name="id">Identificador del comercial.</param>
        /// <returns><c>true</c> si existe; <c>false</c> en caso contrario.</returns>
        Task<bool> ExistsAsync(Guid id);
    }
}
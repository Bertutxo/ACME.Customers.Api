using ACME.Customers.Core.Entities;
using ACME.Customers.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ACME.Customers.Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio que implementa las operaciones CRUD para la entidad <see cref="SalesRep"/>,
    /// usando Entity Framework Core para la persistencia de datos.
    /// </summary>
    public class SalesRepRepository : ISalesRepRepository
    {
        private readonly CustomersDbContext _context;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="SalesRepRepository"/>,
        /// inyectando el contexto de base de datos.
        /// </summary>
        /// <param name="context">Instancia de <see cref="CustomersDbContext"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// Se lanza si <paramref name="context"/> es <c>null</c>.
        /// </exception>
        public SalesRepRepository(CustomersDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "El contexto no puede ser null.");
        }

        /// <summary>
        /// Obtiene todos los comerciales registrados, incluyendo
        /// la lista de clientes visitados por cada comercial.
        /// </summary>
        /// <returns>
        /// Una colección de <see cref="SalesRep"/> con su propiedad
        /// <see cref="SalesRep.Clients"/> cargada de manera ansiosa.
        /// </returns>
        public async Task<IEnumerable<SalesRep>> GetAllAsync()
        {
            return await _context.SalesReps
                .Include(r => r.Clients) // Carga de la relación uno a muchos
                .ToListAsync();
        }

        /// <summary>
        /// Busca un comercial por su identificador único.
        /// </summary>
        /// <param name="id">GUID que identifica al comercial.</param>
        /// <returns>
        /// La instancia de <see cref="SalesRep"/>, o <c>null</c> si no existe.
        /// </returns>
        public async Task<SalesRep?> GetByIdAsync(Guid id)
        {
            return await _context.SalesReps
                .Include(r => r.Clients)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <summary>
        /// Agrega un nuevo comercial al contexto para su posterior persistencia.
        /// </summary>
        /// <param name="salesRep">Instancia de <see cref="SalesRep"/> a agregar.</param>
        /// <exception cref="ArgumentNullException">
        /// Se lanza si <paramref name="salesRep"/> es <c>null</c>.
        /// </exception>
        public async Task AddAsync(SalesRep salesRep)
        {
            if (salesRep is null)
                throw new ArgumentNullException(nameof(salesRep), "El comercial no puede ser null.");

            await _context.SalesReps.AddAsync(salesRep);
        }

        /// <summary>
        /// Marca un comercial existente como modificado para su actualización.
        /// </summary>
        /// <param name="salesRep">Instancia de <see cref="SalesRep"/> con datos actualizados.</param>
        /// <exception cref="ArgumentNullException">
        /// Se lanza si <paramref name="salesRep"/> es <c>null</c>.
        /// </exception>
        public void Update(SalesRep salesRep)
        {
            if (salesRep is null)
                throw new ArgumentNullException(nameof(salesRep), "El comercial a actualizar no puede ser null.");

            _context.SalesReps.Update(salesRep);
        }

        /// <summary>
        /// Elimina un comercial del contexto para que sea borrado de la base de datos.
        /// </summary>
        /// <param name="salesRep">Instancia de <see cref="SalesRep"/> a eliminar.</param>
        /// <exception cref="ArgumentNullException">
        /// Se lanza si <paramref name="salesRep"/> es <c>null</c>.
        /// </exception>
        public void Delete(SalesRep salesRep)
        {
            if (salesRep is null)
                throw new ArgumentNullException(nameof(salesRep), "El comercial a eliminar no puede ser null.");

            _context.SalesReps.Remove(salesRep);
        }

        /// <summary>
        /// Comprueba si existe un comercial con el identificador dado.
        /// </summary>
        /// <param name="id">GUID del comercial a verificar.</param>
        /// <returns>
        /// <c>true</c> si existe; <c>false</c> si no.
        /// </returns>
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.SalesReps.AnyAsync(r => r.Id == id);
        }
    }
}
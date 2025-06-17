using ACME.Customers.Core.Entities;
using ACME.Customers.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ACME.Customers.Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio que implementa las operaciones CRUD para la entidad <see cref="Client"/>,
    /// usando Entity Framework Core para interactuar con la base de datos.
    /// </summary>
    public class ClientRepository : IClientRepository
    {
        private readonly CustomersDbContext _context;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ClientRepository"/>,
        /// inyectando el contexto de base de datos.
        /// </summary>
        /// <param name="context">Instancia del <see cref="CustomersDbContext"/>.</param>
        public ClientRepository(CustomersDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Recupera todos los clientes de la base de datos, incluyendo la información
        /// del comercial responsable (<see cref="SalesRep"/>).
        /// </summary>
        /// <returns>Lista de <see cref="Client"/> con su <see cref="SalesRep"/> cargado.</returns>
        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await _context.Clients
                .Include(c => c.SalesRep) // Carga ansiosa de la relación
                .ToListAsync();
        }

        /// <summary>
        /// Busca un cliente por su identificador único.
        /// </summary>
        /// <param name="id">GUID que identifica al cliente.</param>
        /// <returns>
        /// La instancia de <see cref="Client"/>, o <c>null</c> si no se encuentra.
        /// </returns>
        public async Task<Client?> GetByIdAsync(Guid id)
        {
            return await _context.Clients
                .Include(c => c.SalesRep)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        /// <summary>
        /// Agrega un nuevo cliente al contexto. No se persiste hasta llamar a SaveChanges.
        /// </summary>
        /// <param name="client">Entidad <see cref="Client"/> a agregar.</param>
        /// <exception cref="ArgumentNullException">
        /// Si el parámetro <paramref name="client"/> es <c>null</c>.
        /// </exception>
        public async Task AddAsync(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client), "El cliente no puede ser null.");

            await _context.Clients.AddAsync(client);
        }

        /// <summary>
        /// Marca un cliente existente como modificado para su posterior actualización en la BBDD.
        /// </summary>
        /// <param name="client">Entidad <see cref="Client"/> con valores actualizados.</param>
        /// <exception cref="ArgumentNullException">
        /// Si el parámetro <paramref name="client"/> es <c>null</c>.
        /// </exception>
        public void Update(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client), "El cliente a actualizar no puede ser null.");

            _context.Clients.Update(client);
        }

        /// <summary>
        /// Elimina un cliente del contexto para que sea borrado de la base de datos.
        /// </summary>
        /// <param name="client">Entidad <see cref="Client"/> a eliminar.</param>
        /// <exception cref="ArgumentNullException">
        /// Si el parámetro <paramref name="client"/> es <c>null</c>.
        /// </exception>
        public void Delete(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client), "El cliente a eliminar no puede ser null.");

            _context.Clients.Remove(client);
        }

        /// <summary>
        /// Determina si existe un cliente con el identificador indicado.
        /// </summary>
        /// <param name="id">GUID del cliente a verificar.</param>
        /// <returns>
        /// <c>true</c> si existe al menos un registro con ese ID; <c>false</c> en caso contrario.
        /// </returns>
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Clients.AnyAsync(c => c.Id == id);
        }
    }
}
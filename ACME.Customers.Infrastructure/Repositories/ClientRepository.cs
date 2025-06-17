using ACME.Customers.Core.Entities;
using ACME.Customers.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ACME.Customers.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación de <see cref="IClientRepository"/> usando Entity Framework Core.
    /// </summary>
    public class ClientRepository : IClientRepository
    {
        private readonly CustomersDbContext _context;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ClientRepository"/>.
        /// </summary>
        /// <param name="context">Contexto de base de datos de clientes.</param>
        public ClientRepository(CustomersDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await _context.Clients
                .Include(c => c.SalesRep)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Client?> GetByIdAsync(Guid id)
        {
            return await _context.Clients
                .Include(c => c.SalesRep)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        /// <inheritdoc />
        public async Task AddAsync(Client client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            await _context.Clients.AddAsync(client);
        }

        /// <inheritdoc />
        public void Update(Client client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            _context.Clients.Update(client);
        }

        /// <inheritdoc />
        public void Delete(Client client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            _context.Clients.Remove(client);
        }

        /// <inheritdoc />
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Clients.AnyAsync(c => c.Id == id);
        }
    }
}
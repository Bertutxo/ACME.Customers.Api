using ACME.Customers.Core.Entities;
using ACME.Customers.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ACME.Customers.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación de <see cref="ISalesRepRepository"/> usando Entity Framework Core.
    /// </summary>
    public class SalesRepRepository : ISalesRepRepository
    {
        private readonly CustomersDbContext _context;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="SalesRepRepository"/>.
        /// </summary>
        /// <param name="context">Contexto de base de datos de clientes.</param>
        public SalesRepRepository(CustomersDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<SalesRep>> GetAllAsync()
        {
            return await _context.SalesReps
                .Include(r => r.Clients)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<SalesRep?> GetByIdAsync(Guid id)
        {
            return await _context.SalesReps
                .Include(r => r.Clients)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <inheritdoc />
        public async Task AddAsync(SalesRep salesRep)
        {
            if (salesRep is null)
                throw new ArgumentNullException(nameof(salesRep));

            await _context.SalesReps.AddAsync(salesRep);
        }

        /// <inheritdoc />
        public void Update(SalesRep salesRep)
        {
            if (salesRep is null)
                throw new ArgumentNullException(nameof(salesRep));

            _context.SalesReps.Update(salesRep);
        }

        /// <inheritdoc />
        public void Delete(SalesRep salesRep)
        {
            if (salesRep is null)
                throw new ArgumentNullException(nameof(salesRep));

            _context.SalesReps.Remove(salesRep);
        }

        /// <inheritdoc />
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.SalesReps.AnyAsync(r => r.Id == id);
        }
    }
}
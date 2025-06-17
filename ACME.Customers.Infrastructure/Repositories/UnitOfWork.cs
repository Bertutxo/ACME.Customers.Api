using ACME.Customers.Core.Interfaces;

namespace ACME.Customers.Infrastructure.Repositories
{
    /// <summary>
    /// Implementa el patrón Unit of Work para coordinar las transacciones
    /// y asegurar la persistencia de cambios en el contexto.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CustomersDbContext _context;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="UnitOfWork"/>.
        /// </summary>
        /// <param name="context">
        /// Contexto de base de datos utilizado por la unidad de trabajo.
        /// </param>
        public UnitOfWork(CustomersDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc />
        public async Task<int> CompleteAsync()
        {
            // Guarda todos los cambios pendientes en el contexto
            return await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // Libera los recursos del contexto
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
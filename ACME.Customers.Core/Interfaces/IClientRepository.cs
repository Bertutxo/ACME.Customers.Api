using ACME.Customers.Core.Entities;

namespace ACME.Customers.Core.Interfaces
{
    /// <summary>
    /// Repositorio para la entidad Client.
    /// </summary>
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetAllAsync();
        Task<Client?> GetByIdAsync(Guid id);
        Task AddAsync(Client client);
        void Update(Client client);
        void Delete(Client client);
        Task<bool> ExistsAsync(Guid id);
    }
}
using ACME.Customers.Application.DTOs;
using ACME.Customers.Application.Interfaces;
using ACME.Customers.Core.Entities;
using ACME.Customers.Core.Interfaces;
using AutoMapper;

namespace ACME.Customers.Application.Services
{
    /// <summary>
    /// Implementación de <see cref="IClientService"/> para gestionar Clientes.
    /// </summary>
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly ISalesRepRepository _salesRepRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ClientService"/>.
        /// </summary>
        public ClientService(
            IClientRepository clientRepository,
            ISalesRepRepository salesRepRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _clientRepository = clientRepository ?? throw new ArgumentNullException(nameof(clientRepository));
            _salesRepRepository = salesRepRepository ?? throw new ArgumentNullException(nameof(salesRepRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ClientDto>> GetAllAsync()
        {
            var clients = await _clientRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ClientDto>>(clients);
        }

        /// <inheritdoc />
        public async Task<ClientDto?> GetByIdAsync(Guid id)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            return client == null ? null : _mapper.Map<ClientDto>(client);
        }

        /// <inheritdoc />
        public async Task<Guid> CreateAsync(ClientCreateDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // Verificar que el SalesRep existe
            if (!await _salesRepRepository.ExistsAsync(dto.SalesRepId))
            {
                throw new KeyNotFoundException($"SalesRep con ID '{dto.SalesRepId}' no encontrado.");
            }

            var client = _mapper.Map<Client>(dto);
            client.Id = Guid.NewGuid();

            await _clientRepository.AddAsync(client);
            await _unitOfWork.CompleteAsync();

            return client.Id;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(Guid id, ClientUpdateDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (!await _clientRepository.ExistsAsync(id))
                return false;

            // Verificar que el SalesRep existe si ha cambiado
            if (!await _salesRepRepository.ExistsAsync(dto.SalesRepId))
            {
                throw new KeyNotFoundException($"SalesRep con ID '{dto.SalesRepId}' no encontrado.");
            }

            var client = await _clientRepository.GetByIdAsync(id);
            if (client == null)
                return false;

            client.Name = dto.Name;
            client.ContactEmail = dto.ContactEmail;
            client.VisitDate = dto.VisitDate;
            client.SalesRepId = dto.SalesRepId;
            client.Notes = dto.Notes;

            _clientRepository.Update(client);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(Guid id)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            if (client == null)
                return false;

            _clientRepository.Delete(client);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
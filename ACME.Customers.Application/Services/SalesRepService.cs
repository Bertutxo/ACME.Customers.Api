using ACME.Customers.Application.DTOs;
using ACME.Customers.Application.Interfaces;
using ACME.Customers.Core.Entities;
using ACME.Customers.Core.Interfaces;
using AutoMapper;

namespace ACME.Customers.Application.Services
{
    /// <summary>
    /// Implementación de <see cref="ISalesRepService"/> para gestionar Comerciales.
    /// </summary>
    public class SalesRepService : ISalesRepService
    {
        private readonly ISalesRepRepository _salesRepRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="SalesRepService"/>.
        /// </summary>
        public SalesRepService(
            ISalesRepRepository salesRepRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _salesRepRepository = salesRepRepository ?? throw new ArgumentNullException(nameof(salesRepRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc />
        public async Task<IEnumerable<SalesRepDto>> GetAllAsync()
        {
            var reps = await _salesRepRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<SalesRepDto>>(reps);
        }

        /// <inheritdoc />
        public async Task<SalesRepDto?> GetByIdAsync(Guid id)
        {
            var rep = await _salesRepRepository.GetByIdAsync(id);
            return rep == null ? null : _mapper.Map<SalesRepDto>(rep);
        }

        /// <inheritdoc />
        public async Task<Guid> CreateAsync(SalesRepCreateDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var rep = _mapper.Map<SalesRep>(dto);
            rep.Id = Guid.NewGuid();

            await _salesRepRepository.AddAsync(rep);
            await _unitOfWork.CompleteAsync();

            return rep.Id;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(Guid id, SalesRepUpdateDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (!await _salesRepRepository.ExistsAsync(id))
                return false;

            var rep = await _salesRepRepository.GetByIdAsync(id);
            if (rep == null)
                return false;

            rep.Name = dto.Name;
            rep.Email = dto.Email;
            rep.Phone = dto.Phone;

            _salesRepRepository.Update(rep);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(Guid id)
        {
            var rep = await _salesRepRepository.GetByIdAsync(id);
            if (rep == null)
                return false;

            // Verificamos si hay clientes asociados:
            // Asegurarse de que rep.Clients esté poblado (en el repositorio GetByIdAsync incluir Clients).
            if (rep.Clients != null && rep.Clients.Any())
            {
                var nombres = rep.Clients.Select(c => c.Name).ToList();
                var lista = string.Join(", ", nombres);
                // Lanzar excepción con mensaje detallado
                throw new InvalidOperationException(
                    $"No se puede eliminar el comercial porque tiene {nombres.Count} cliente(s) asociado(s): {lista}.");
            }

            _salesRepRepository.Delete(rep);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
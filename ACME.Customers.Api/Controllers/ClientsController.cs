using ACME.Customers.Application.DTOs;
using ACME.Customers.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ACME.Customers.Api.Controllers
{
    /// <summary>
    /// Controlador API para gestionar operaciones CRUD de clientes.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        /// <summary>
        /// Crea una nueva instancia de <see cref="ClientsController"/>.
        /// </summary>
        /// <param name="clientService">
        /// Servicio de aplicación para gestión de clientes.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Se lanza si <paramref name="clientService"/> es <c>null</c>.
        /// </exception>
        public ClientsController(IClientService clientService)
        {
            _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
        }

        /// <summary>
        /// Obtiene la lista de todos los clientes.
        /// </summary>
        /// <returns>
        /// <see cref="ActionResult{IEnumerable{ClientDto}}"/> con la colección de clientes.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetAll()
        {
            var clients = await _clientService.GetAllAsync();
            return Ok(clients);
        }

        /// <summary>
        /// Obtiene un cliente por su identificador.
        /// </summary>
        /// <param name="id">GUID que identifica al cliente.</param>
        /// <returns>
        /// <see cref="ActionResult{ClientDto}"/> con el cliente, o 404 si no se encuentra.
        /// </returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ClientDto>> GetById(Guid id)
        {
            var client = await _clientService.GetByIdAsync(id);
            if (client == null)
                return NotFound();

            return Ok(client);
        }

        /// <summary>
        /// Crea un nuevo cliente.
        /// </summary>
        /// <param name="dto">DTO con los datos del cliente a crear.</param>
        /// <returns>
        /// <see cref="IActionResult"/> con status 201 y ruta al recurso creado,
        /// o 400 si falla la validación o el <c>salesRepId</c> no existe.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClientCreateDto dto)
        {
            try
            {
                var newId = await _clientService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = newId }, null);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza un cliente existente.
        /// </summary>
        /// <param name="id">GUID que identifica al cliente.</param>
        /// <param name="dto">DTO con los datos actualizados.</param>
        /// <returns>
        /// <see cref="IActionResult"/> con status 204 si se actualiza,
        /// 404 si no existe, o 400 si falla validación.
        /// </returns>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ClientUpdateDto dto)
        {
            try
            {
                var updated = await _clientService.UpdateAsync(id, dto);
                if (!updated)
                    return NotFound();

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un cliente por su identificador.
        /// </summary>
        /// <param name="id">GUID que identifica al cliente.</param>
        /// <returns>
        /// <see cref="IActionResult"/> con status 204 si se elimina, o 404 si no existe.
        /// </returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _clientService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
using ACME.Customers.Application.DTOs;
using ACME.Customers.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ACME.Customers.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
        }

        // GET: api/Clients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetAll()
        {
            var clients = await _clientService.GetAllAsync();
            return Ok(clients);
        }

        // GET: api/Clients/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ClientDto>> GetById(Guid id)
        {
            var client = await _clientService.GetByIdAsync(id);
            if (client == null)
                return NotFound();

            return Ok(client);
        }

        // POST: api/Clients
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

        // PUT: api/Clients/{id}
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

        // DELETE: api/Clients/{id}
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
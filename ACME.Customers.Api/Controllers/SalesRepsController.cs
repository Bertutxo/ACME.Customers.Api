using ACME.Customers.Application.DTOs;
using ACME.Customers.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ACME.Customers.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesRepsController : ControllerBase
    {
        private readonly ISalesRepService _service;

        public SalesRepsController(ISalesRepService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        // GET: api/SalesReps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesRepDto>>> GetAll()
        {
            var reps = await _service.GetAllAsync();
            return Ok(reps);
        }

        // GET: api/SalesReps/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SalesRepDto>> GetById(Guid id)
        {
            var rep = await _service.GetByIdAsync(id);
            if (rep == null) return NotFound();
            return Ok(rep);
        }

        // POST: api/SalesReps
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SalesRepCreateDto dto)
        {
            try
            {
                var newId = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = newId }, null);
            }
            catch (Exception ex)
            {
                // Aquí podrías diferenciar tipos de excepción (por ej. validación)
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/SalesReps/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] SalesRepUpdateDto dto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, dto);
                if (!updated) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/SalesReps/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
using ACME.Customers.Application.DTOs;
using ACME.Customers.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ACME.Customers.Api.Controllers
{
    /// <summary>
    /// Controlador API para gestionar operaciones CRUD de comerciales (<see cref="SalesRepDto"/>).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SalesRepsController : ControllerBase
    {
        private readonly ISalesRepService _service;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="SalesRepsController"/>.
        /// </summary>
        /// <param name="service">Servicio de aplicación para gestión de comerciales.</param>
        /// <exception cref="ArgumentNullException">Si <paramref name="service"/> es <c>null</c>.</exception>
        public SalesRepsController(ISalesRepService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service), "El servicio de comerciales no puede ser null.");
        }

        /// <summary>
        /// Recupera todos los comerciales registrados.
        /// </summary>
        /// <returns>
        /// <see cref="ActionResult{IEnumerable{SalesRepDto}}"/> con la lista de comerciales.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesRepDto>>> GetAll()
        {
            var reps = await _service.GetAllAsync();
            return Ok(reps);
        }

        /// <summary>
        /// Obtiene un comercial por su <paramref name="id"/>.
        /// </summary>
        /// <param name="id">GUID que identifica al comercial.</param>
        /// <returns>
        /// <see cref="ActionResult{SalesRepDto}"/> con el comercial, o 404 si no se encuentra.
        /// </returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SalesRepDto>> GetById(Guid id)
        {
            var rep = await _service.GetByIdAsync(id);
            if (rep == null)
                return NotFound();

            return Ok(rep);
        }

        /// <summary>
        /// Crea un nuevo comercial.
        /// </summary>
        /// <param name="dto">DTO con los datos del comercial a crear.</param>
        /// <returns>
        /// <see cref="IActionResult"/> con status 201 y cabecera Location a <see cref="GetById"/>,
        /// o 400 si ocurre algún error (p. ej., validación).
        /// </returns>
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
                // Devuelve BadRequest con el mensaje de error
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza un comercial existente.
        /// </summary>
        /// <param name="id">GUID que identifica al comercial a actualizar.</param>
        /// <param name="dto">DTO con los datos actualizados.</param>
        /// <returns>
        /// <see cref="IActionResult"/> con status 204 si la actualización fue exitosa,
        /// 404 si no se encontró, o 400 si ocurrió un error.
        /// </returns>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] SalesRepUpdateDto dto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, dto);
                if (!updated)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un comercial por su <paramref name="id"/>.
        /// </summary>
        /// <param name="id">GUID que identifica al comercial a eliminar.</param>
        /// <returns>
        /// <see cref="IActionResult"/> con status 204 si la eliminación fue exitosa,
        /// o 404 si no se encontró.</returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
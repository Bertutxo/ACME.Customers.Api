using System.Net;
using System.Text;
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
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        #region JSON Endpoints
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
            if (rep == null) return NotFound();
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
        public async Task<IActionResult> Create([FromBody] SalesRepCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var newId = await _service.CreateAsync(dto);
                var resultDto = new SalesRepDto
                {
                    Id = newId,
                    Name = dto.Name,
                    Email = dto.Email,
                    Phone = dto.Phone
                };
                return Ok(new { message = "Comercial creado satisfactoriamente", salesRep = resultDto });
            }
            catch (Exception ex)
            {
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updated = await _service.UpdateAsync(id, dto);
                if (!updated)
                    return NotFound();
                var resultDto = new SalesRepDto
                {
                    Id = id,
                    Name = dto.Name,
                    Email = dto.Email,
                    Phone = dto.Phone
                };
                return Ok(new { message = "Comercial actualizado satisfactoriamente", salesRep = resultDto });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Error interno al actualizar comercial." });
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
            try
            {
                var deleted = await _service.DeleteAsync(id);
                if (!deleted) return NotFound();
                return Ok(new { message = "Comercial eliminado correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Error interno al eliminar comercial." });
            }
        }

        #endregion

        #region HTML fragments for HTMX

        [HttpGet("html")]
        public async Task<ContentResult> GetAllHtml()
        {
            var reps = await _service.GetAllAsync();
            var sb = new StringBuilder();
            sb.AppendLine("<table class='min-w-full border-collapse'>");
            sb.AppendLine("<thead class='bg-gray-100'><tr>");
            sb.AppendLine("  <th class='border p-2 text-left'>Nombre</th>");
            sb.AppendLine("  <th class='border p-2 text-left'>Email</th>");
            sb.AppendLine("  <th class='border p-2 text-left'>Teléfono</th>");
            sb.AppendLine("  <th class='border p-2 text-left'>Acciones</th>");
            sb.AppendLine("</tr></thead>");
            sb.AppendLine("<tbody>");
            foreach (var r in reps)
            {
                var nameEsc = WebUtility.HtmlEncode(r.Name);
                var emailEsc = WebUtility.HtmlEncode(r.Email);
                var phoneEsc = WebUtility.HtmlEncode(r.Phone);
                sb.AppendLine("<tr>");
                sb.AppendLine($"  <td class='border p-2'>{nameEsc}</td>");
                sb.AppendLine($"  <td class='border p-2'>{emailEsc}</td>");
                sb.AppendLine($"  <td class='border p-2'>{phoneEsc}</td>");
                sb.AppendLine("  <td class='border p-2 space-x-2'>");
                // Editar
                sb.AppendLine($"    <button class='px-2 py-1 bg-yellow-500 text-white rounded hover:bg-yellow-600' " +
                              $"hx-get='/api/SalesReps/{r.Id}/edit-html' " +
                              $"hx-target='#main-content' hx-swap='innerHTML'>Editar</button>");
                // Borrar con confirm y manejo de error
                sb.AppendLine($"    <button class='px-2 py-1 bg-red-500 text-white rounded hover:bg-red-600' " +
                              $"hx-delete='/api/SalesReps/{r.Id}' " +
                              $"hx-confirm='¿Eliminar comercial \"{nameEsc}\"?' " +
                              $"hx-on='htmx:responseError: window.Alpine && Alpine.store(\"app\").handleError(event)' " +
                              $"hx-on='htmx:afterRequest: if (event.detail.xhr.status === 204) {{ window.Alpine && Alpine.store(\"app\").showToast(\"Comercial eliminado correctamente\"); Alpine.store(\"app\").showRepsList(); }}'>Borrar</button>");
                sb.AppendLine("  </td>");
                sb.AppendLine("</tr>");
            }
            sb.AppendLine("</tbody></table>");
            return Content(sb.ToString(), "text/html");
        }

        [HttpGet("{id:guid}/edit-html")]
        public async Task<ContentResult> EditHtml(Guid id)
        {
            var rep = await _service.GetByIdAsync(id);
            if (rep == null)
            {
                return Content("<div class='text-red-600'>Comercial no encontrado.</div>", "text/html");
            }
            var sb = new StringBuilder();
            sb.AppendLine("<div class='bg-white p-4 rounded shadow'>");
            sb.AppendLine($"  <h2 class='text-xl font-semibold mb-2'>Editar Comercial: {WebUtility.HtmlEncode(rep.Name)}</h2>");
            sb.AppendLine("  <form " +
                          $"hx-put='/api/SalesReps/{rep.Id}' " +
                          "hx-ext='json-enc' " +
                          "hx-target='#main-content' hx-swap='innerHTML' " +
                          "hx-on='htmx:responseError: window.Alpine && Alpine.store(\"app\").handleError(event)' " +
                          "hx-on='htmx:afterRequest: if (event.detail.xhr.status === 204) { window.Alpine && Alpine.store(\"app\").showToast(\"Comercial actualizado correctamente\"); Alpine.store(\"app\").showRepsList(); }' " +
                          "class='space-y-4' " +
                          "onsubmit='return false;'>");
            sb.AppendLine($"    <div><label class='block mb-1 font-medium'>Nombre</label>" +
                          $"<input name='name' value='{WebUtility.HtmlEncode(rep.Name)}' placeholder='Nombre' class='w-full p-2 border rounded' required /></div>");
            sb.AppendLine($"    <div><label class='block mb-1 font-medium'>Email</label>" +
                          $"<input name='email' type='email' value='{WebUtility.HtmlEncode(rep.Email)}' placeholder='Email' class='w-full p-2 border rounded' required /></div>");
            sb.AppendLine($"    <div><label class='block mb-1 font-medium'>Teléfono</label>" +
                          $"<input name='phone' value='{WebUtility.HtmlEncode(rep.Phone)}' placeholder='Teléfono' class='w-full p-2 border rounded' required /></div>");
            sb.AppendLine("    <div class='flex justify-end space-x-2'>");
            sb.AppendLine("      <button type='button' class='px-4 py-2 bg-gray-400 text-white rounded hover:bg-gray-500' " +
                          "hx-on='click: window.Alpine && Alpine.store(\"app\").showRepsList()'>Cancelar</button>");
            sb.AppendLine("      <button type='submit' class='px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700'>Guardar</button>");
            sb.AppendLine("    </div>");
            sb.AppendLine("</form></div>");
            return Content(sb.ToString(), "text/html");
        }

        #endregion
    }
}
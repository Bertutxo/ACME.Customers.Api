using System.Net;
using System.Text;
using ACME.Customers.Application.DTOs;
using ACME.Customers.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ACME.Customers.Api.Controllers
{
    /// <summary>
    /// Controlador API para gestionar operaciones CRUD de clientes,
    /// y para devolver vistas parciales HTML para HTMX.
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

        #region JSON Endpoints
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
            catch (Exception)
            {
                return StatusCode(500, new { message = "Error interno al crear cliente." });
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
                // Éxito, devolvemos 204 No Content
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Error interno al actualizar cliente." });
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
            try
            {
                var deleted = await _clientService.DeleteAsync(id);
                if (!deleted)
                    return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                // En caso de que el servicio lance excepción por dependencias, etc.
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Error interno al borrar cliente." });
            }
        }

        #endregion

        #region HTML fragments for HTMX

        /// <summary>
        /// Devuelve la tabla completa de clientes como HTML (para inyección HTMX).
        /// </summary>
        [HttpGet("html")]
        public async Task<ContentResult> GetAllHtml()
        {
            var clients = await _clientService.GetAllAsync();
            var sb = new StringBuilder();

            sb.AppendLine("<table class='min-w-full border-collapse'>");
            sb.AppendLine("<thead class='bg-gray-100'><tr>");
            sb.AppendLine("  <th class='border p-2 text-left'>Nombre</th>");
            sb.AppendLine("  <th class='border p-2 text-left'>Email</th>");
            sb.AppendLine("  <th class='border p-2 text-left'>Fecha Visita</th>");
            sb.AppendLine("  <th class='border p-2 text-left'>Acciones</th>");
            sb.AppendLine("</tr></thead>");
            sb.AppendLine("<tbody>");

            foreach (var c in clients)
            {
                var nameEsc = WebUtility.HtmlEncode(c.Name);
                var emailEsc = WebUtility.HtmlEncode(c.ContactEmail);
                var visitEsc = c.VisitDate.ToString("yyyy-MM-dd");
                sb.AppendLine("<tr>");
                sb.AppendLine($"  <td class='border p-2'>{nameEsc}</td>");
                sb.AppendLine($"  <td class='border p-2'>{emailEsc}</td>");
                sb.AppendLine($"  <td class='border p-2'>{visitEsc}</td>");
                sb.AppendLine("  <td class='border p-2 space-x-2'>");
                // Botón Editar: carga formulario de edición
                sb.AppendLine($"    <button class='px-2 py-1 bg-yellow-500 text-white rounded hover:bg-yellow-600' " +
                              $"hx-get='/api/Clients/{c.Id}/edit-html' " +
                              $"hx-target='#main-content' hx-swap='innerHTML'>Editar</button>");
                // Botón Borrar: con confirm, y manejadores HTMX
                sb.AppendLine($"    <button class='px-2 py-1 bg-red-500 text-white rounded hover:bg-red-600' " +
                              $"hx-delete='/api/Clients/{c.Id}' " +
                              $"hx-confirm='¿Eliminar cliente \"{nameEsc}\"?' " +
                              $"@htmx:responseError=\"window.Alpine && Alpine.store('app').handleError($event)\" " +
                              $"@htmx:afterRequest=\"if (event.detail.xhr.status === 204) {{ window.Alpine && Alpine.store('app').showToast('Cliente eliminado correctamente'); Alpine.store('app').showList('clients'); }}\">Borrar</button>");
                sb.AppendLine("  </td>");
                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</tbody></table>");
            return Content(sb.ToString(), "text/html");
        }

        /// <summary>
        /// Devuelve el formulario de edición de un cliente, con campos prellenados, como fragmento HTML para HTMX.
        /// </summary>
        [HttpGet("{id:guid}/edit-html")]
        public async Task<ContentResult> EditHtml(Guid id)
        {
            var client = await _clientService.GetByIdAsync(id);
            if (client == null)
            {
                // Si no existe, devolvemos un pequeño aviso HTML o 404
                var msg = $"<div class='text-red-600'>Cliente no encontrado.</div>";
                return Content(msg, "text/html");
            }

            // Generamos un formulario HTML con los valores prellenados
            var sb = new StringBuilder();
            sb.AppendLine("<div class='bg-white p-4 rounded shadow'>");
            sb.AppendLine($"  <h2 class='text-xl font-semibold mb-2'>Editar Cliente: {WebUtility.HtmlEncode(client.Name)}</h2>");
            sb.AppendLine("  <form " +
                          $"hx-put='/api/Clients/{client.Id}' " +
                          "hx-ext='json-enc' " +
                          "hx-target='#main-content' hx-swap='innerHTML' " +
                          "@htmx:responseError=\"window.Alpine && Alpine.store('app').handleError($event)\" " +
                          "@htmx:afterRequest=\"if (event.detail.xhr.status === 204) { window.Alpine && Alpine.store('app').showToast('Cliente actualizado correctamente'); Alpine.store('app').showList('clients'); }\" " +
                          "@submit.prevent " +
                          "class='space-y-2'>");

            // Campos: name, contactEmail, visitDate, salesRepId, notes
            // **Opcional**: podrías generar un <select> con opciones de SalesReps.
            // Para simplicidad, dejamos input text; si quieres combo, habría que obtener la lista de comerciales.
            sb.AppendLine($"    <input name='name' value='{WebUtility.HtmlEncode(client.Name)}' placeholder='Nombre' class='w-full p-2 border rounded' required />");
            sb.AppendLine($"    <input name='contactEmail' type='email' value='{WebUtility.HtmlEncode(client.ContactEmail)}' placeholder='Email' class='w-full p-2 border rounded' required />");
            sb.AppendLine($"    <input name='visitDate' type='date' value='{client.VisitDate:yyyy-MM-dd}' class='w-full p-2 border rounded' required />");
            sb.AppendLine($"    <input name='salesRepId' value='{client.SalesRepId}' placeholder='SalesRep GUID' class='w-full p-2 border rounded' required />");
            sb.AppendLine($"    <textarea name='notes' placeholder='Notas' class='w-full p-2 border rounded'>{WebUtility.HtmlEncode(client.Notes ?? "")}</textarea>");
            sb.AppendLine("    <div class='space-x-2'>");
            sb.AppendLine("      <button type='submit' class='px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700'>Guardar</button>");
            sb.AppendLine("      <button type='button' class='px-4 py-2 bg-gray-400 text-white rounded hover:bg-gray-500' @click=\"$store.app.showList('clients')\">Cancelar</button>");
            sb.AppendLine("    </div>");
            sb.AppendLine("  </form>");
            sb.AppendLine("</div>");

            return Content(sb.ToString(), "text/html");
        }

        #endregion
    }
}
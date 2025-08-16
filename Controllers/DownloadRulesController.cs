using Fiscalapi.Abstractions;
using Fiscalapi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FiscalApi.Samples.AspNet.Controllers
{
    /// <summary>
    /// NOTA IMPORTANTE: Las rutas (paths) definidas en esta aplicación de ejemplo no necesariamente
    /// coinciden con las rutas reales de la API de FiscalApi. Los paths de esta aplicación son
    /// únicamente para propósitos de demostración y ejemplo.
    /// 
    /// Para consultar las rutas y endpoints oficiales de FiscalApi, consulte la documentación 
    /// oficial en: https://docs.fiscalapi.com/
    /// </summary>
    [Route("api/v4/[controller]")]
    [ApiController]
    public class DownloadRulesController : ControllerBase
    {
        private readonly IFiscalApiClient _fiscalApiClient;

        public DownloadRulesController(IFiscalApiClient fiscalApiClient)
        {
            _fiscalApiClient = fiscalApiClient;
        }

        

        /// <summary>
        /// Obtener lista paginada de reglas de descarga masiva.
        /// </summary>
        /// <remarks>
        /// Utiliza paginación con página 1 y 10 elementos por página por defecto.
        /// </remarks>
        [HttpGet]
        public async Task<IActionResult> GetRulesPaginated()
        {
            // Página 1, 10 elementos por página (hardcodeado según patrón del ProductsController)
            var apiResponse = await _fiscalApiClient.DownloadRules.GetListAsync(1, 10);

            if (apiResponse.Succeeded)
                return Ok(apiResponse.Data);

            return BadRequest(apiResponse);
        }

        /// <summary>
        /// Crear una nueva regla de descarga masiva con datos predefinidos.
        /// </summary>
        /// <remarks>
        /// Crea una regla para descargar CFDI recibidos y vigentes con configuración por defecto.
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> CreateRule()
        {
            // Regla hardcodeada según el ejemplo de WinForms
            var request = new DownloadRule
            {
                PersonId = "b0c1cf6c-153a-464e-99df-5741f45d6695", // Persona que recibió los CFDI
                Description = "Regla descarga demo ...",
                SatQueryTypeId = "CFDI",
                DownloadTypeId = "Recibidos",
                SatInvoiceStatusId = "Vigente",
            };

            var apiResponse = await _fiscalApiClient.DownloadRules.CreateAsync(request);

            if (apiResponse.Succeeded)
                return Ok(apiResponse.Data);

            return BadRequest(apiResponse);
        }

        /// <summary>
        /// Obtener una regla de descarga masiva específica por su ID.
        /// </summary>
        /// <remarks>
        /// Permite consultar los detalles completos de una regla configurada.
        /// </remarks>
        /// <param name="id">ID único de la regla a consultar</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRuleById(string id)
        {
            var apiResponse = await _fiscalApiClient.DownloadRules.GetByIdAsync(id);

            if (apiResponse.Succeeded)
                return Ok(apiResponse.Data);

            return BadRequest(apiResponse);
        }

        /// <summary>
        /// Actualizar una regla de descarga masiva existente.
        /// </summary>
        /// <remarks>
        /// Modifica la descripción de una regla específica con datos predefinidos.
        /// </remarks>
        /// <param name="id">ID único de la regla a actualizar</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRule(string id)
        {
            // Actualización hardcodeada según el ejemplo de WinForms
            var request = new DownloadRule
            {
                Id = id,
                Description = "Regla descarga actualizada",
            };

            var apiResponse = await _fiscalApiClient.DownloadRules.UpdateAsync(id, request);

            if (apiResponse.Succeeded)
                return Ok(apiResponse.Data);

            return BadRequest(apiResponse);
        }

        /// <summary>
        /// Eliminar una regla de descarga masiva por su ID.
        /// </summary>
        /// <remarks>
        /// Borra permanentemente la regla especificada del sistema.
        /// </remarks>
        /// <param name="id">ID único de la regla a eliminar</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRule(string id)
        {
            var apiResponse = await _fiscalApiClient.DownloadRules.DeleteAsync(id);

            if (apiResponse.Succeeded)
                return Ok(apiResponse.Data);

            return BadRequest(apiResponse);
        }

        /// <summary>
        /// Crear una regla de descarga masiva de prueba.
        /// </summary>
        /// <remarks>
        /// Genera automáticamente una regla con configuración de testing predeterminada.
        /// </remarks>
        [HttpPost("test")]
        public async Task<IActionResult> CreateTestRule()
        {
            var apiResponse = await _fiscalApiClient.DownloadRules.CreateTestRuleAsync();

            if (apiResponse.Succeeded)
                return Ok(apiResponse.Data);

            return BadRequest(apiResponse);
        }
    }
}
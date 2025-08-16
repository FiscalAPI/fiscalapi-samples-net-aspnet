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
    [Route("api/[controller]")]
    [ApiController]
    public class ApiKeysController : ControllerBase
    {
        private readonly IFiscalApiClient _fiscalApiClient;

        public ApiKeysController(IFiscalApiClient fiscalApiClient)
        {
            _fiscalApiClient = fiscalApiClient;
        }

        /// <summary>
        /// Obtener lista paginada de apikeys (hardcodeado: page=1, pageSize=2).
        /// Equivale a ObtenerPagedListApikeys_Click en WinForms
        /// </summary>
        [HttpGet("obtener-lista-paginada")]
        public async Task<IActionResult> ObtenerListaPaginada()
        {
            // pageNumber=1, pageSize=2
            var apiResponse = await _fiscalApiClient.ApiKeys.GetListAsync(1, 2);
            if (apiResponse.Succeeded)
                return Ok(apiResponse.Data);
            return BadRequest(apiResponse);
        }

        /// <summary>
        /// Obtener apikey por ID (sin hardcodear).
        /// Equivale a ObtenerApikeyByID_Click, pero recibe el id en la ruta.
        /// </summary>
        /// <param name="id">Id de la apikey</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var apiResponse = await _fiscalApiClient.ApiKeys.GetByIdAsync(id);

            if (apiResponse.Succeeded)
                return Ok(apiResponse.Data);

            return BadRequest(apiResponse);
        }

        /// <summary>
        /// Crear una apikey con datos fijos (PersonId hardcodeado).
        /// Equivale a CrearApikey_Click en WinForms
        /// </summary>
        [HttpPost("crear-apikey/{personId}")]
        public async Task<IActionResult> CrearApikey(string personId)
        {
            // request model con PersonId hardcodeado (igual que en WinForms)
            var request = new ApiKey
            {
                PersonId = personId,
            };

            var apiResponse = await _fiscalApiClient.ApiKeys.CreateAsync(request);

            if (apiResponse.Succeeded)
                return Ok(apiResponse.Data);

            return BadRequest(apiResponse);
        }

        /// <summary>
        /// Revocar (borrar) apikey por ID 
        /// Equivale a RevocaApikey_Click en WinForms, pero recibiendo el id en la ruta.
        /// </summary>
        /// <param name="id">Id de la apikey a revocar</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Revoke(string id)
        {
            var apiResponse = await _fiscalApiClient.ApiKeys.DeleteAsync(id);

            if (apiResponse.Succeeded)
                return Ok(apiResponse);

            return BadRequest(apiResponse);
        }

        /// <summary>
        /// Actualizar apikey con datos fijos (ID y descripción).
        /// Equivale a UpdateApiKey_Click en WinForms
        /// </summary>
        [HttpPut("actualizar-apikey/{id}")]
        public async Task<IActionResult> UpdateApiKey(string id)
        {
            var request = new ApiKey
            {
                Id = id,
                Description = "Api-key server 001",
                ApiKeyStatus = ApiKeyStatus.Enabled,
            };

            var apiResponse = await _fiscalApiClient.ApiKeys.UpdateAsync(id, request);

            if (apiResponse.Succeeded)
                return Ok(apiResponse.Data);

            return BadRequest(apiResponse);
        }
    }
}
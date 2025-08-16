using Fiscalapi.Abstractions;
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
    public class DownloadCatalogsController : ControllerBase
    {
        private readonly IFiscalApiClient _fiscalApiClient;

        public DownloadCatalogsController(IFiscalApiClient fiscalApiClient)
        {
            _fiscalApiClient = fiscalApiClient;
        }

        /// <summary>
        /// Obtener todos los catálogos de descarga masiva disponibles.
        /// </summary>
        /// <remarks>
        /// Retorna una lista de nombres de catálogos que pueden ser consultados.
        /// </remarks>
        [HttpGet]
        public async Task<IActionResult> GetAllCatalogs()
        {
            var apiResponse = await _fiscalApiClient.DownloadCatalogs.GetListAsync();

            if (apiResponse.Succeeded)
                return Ok(apiResponse.Data);

            return BadRequest(apiResponse);
        }

        /// <summary>
        /// Obtener los registros de un catálogo específico de descarga masiva por su nombre.
        /// </summary>
        /// <remarks>
        /// Permite consultar el contenido detallado de catálogos como 'SatInvoiceStatuses', 'SatProductCodes', etc.
        /// </remarks>
        /// <param name="catalogName">Nombre del catálogo a consultar (ej: SatInvoiceStatuses)</param>
        [HttpGet("{catalogName}")]
        public async Task<IActionResult> GetCatalogByName(string catalogName)
        {
            var apiResponse = await _fiscalApiClient.DownloadCatalogs.GetRecordByNameAsync(catalogName);

            if (apiResponse.Succeeded)
                return Ok(apiResponse.Data);

            return BadRequest(apiResponse);
        }
    }
}
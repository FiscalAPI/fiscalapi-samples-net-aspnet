using Fiscalapi.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace FiscalApi.Samples.AspNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogsController : ControllerBase
    {
        private readonly IFiscalApiClient _fiscalApiClient;

        public CatalogsController(IFiscalApiClient fiscalApiClient)
        {
            _fiscalApiClient = fiscalApiClient;
        }

        /// <summary>
        /// Obtener todos los catálogos disponibles.
        /// Equivale a: ObtenerCatalogosDisponibles_Click
        /// </summary>
        [HttpGet("disponibles")]
        public async Task<IActionResult> ObtenerCatalogosDisponibles()
        {
            var apiResponse = await _fiscalApiClient.Catalogs.GetListAsync();

            if (apiResponse.Succeeded)
                return Ok(apiResponse.Data);

            return BadRequest(apiResponse);
        }

        /// <summary>
        /// Obtener un registro de un catálogo por nombre del catalogo y id del registro en ese catalogo.
        /// Equivale a: ObtenerCatalogRecordPorId_Click
        /// </summary>
        [HttpGet("record")]
        public async Task<IActionResult> ObtenerCatalogRecordPorId()
        {
            // Hardcodeado: "SatProductCodes", "84111500"
            var catalogName = "SatProductCodes";
            var recordId = "84111500";
            var apiResponse = await _fiscalApiClient.Catalogs.GetRecordByIdAsync(catalogName, recordId);

            if (apiResponse.Succeeded)
                return Ok(apiResponse.Data);

            return BadRequest(apiResponse);
        }

        /// <summary>
        /// Buscar 'inter' en el catálogo 'SatUnitMeasurements' (página 1, 10 registros).
        /// Equivale a: BuscarCatalogo_Click
        /// </summary>
        [HttpGet("buscar-catalogo")]
        public async Task<IActionResult> BuscarCatalogo()
        {
            // Hardcodeado: "SatUnitMeasurements", "inter", page=1, size=10
            var catalogName = "SatUnitMeasurements";
            var searchText = "inter";
            var pageNumber = 1;
            var pageSize = 10;

            var apiResponse =
                await _fiscalApiClient.Catalogs.SearchCatalogAsync(catalogName, searchText, pageNumber, pageSize);

            if (apiResponse.Succeeded)
                return Ok(apiResponse.Data);

            return BadRequest(apiResponse);
        }

        /// <summary>
        /// Buscar 'serv' en el catálogo 'SatProductCodes' (página 1, 10 registros).
        /// Equivale a: BuscarCodigoProductoServicio_Click
        /// </summary>
        [HttpGet("buscar-codigo-producto-servicio")]
        public async Task<IActionResult> BuscarCodigoProductoServicio()
        {
            // Hardcodeado: "SatProductCodes", "serv", page=1, size=10
            var catalogName = "SatProductCodes";
            var searchText = "serv";
            var pageNumber = 1;
            var pageSize = 10;

            var apiResponse =
                await _fiscalApiClient.Catalogs.SearchCatalogAsync(catalogName, searchText, pageNumber, pageSize);

            if (apiResponse.Succeeded)
                return Ok(apiResponse.Data);

            return BadRequest(apiResponse);
        }

        /// <summary>
        /// Buscar 'inter' en el catálogo 'SatUnitMeasurements' (página 1, 10 registros).
        /// Equivale a: BuscarCodigoUnidad_Click
        /// </summary>
        [HttpGet("buscar-codigo-unidad")]
        public async Task<IActionResult> BuscarCodigoUnidad()
        {
            // Hardcodeado: "SatUnitMeasurements", "inter", page=1, size=10
            var catalogName = "SatUnitMeasurements";
            var searchText = "inter";
            var pageNumber = 1;
            var pageSize = 10;

            var apiResponse =
                await _fiscalApiClient.Catalogs.SearchCatalogAsync(catalogName, searchText, pageNumber, pageSize);

            if (apiResponse.Succeeded)
                return Ok(apiResponse.Data);

            return BadRequest(apiResponse);
        }
    }
}
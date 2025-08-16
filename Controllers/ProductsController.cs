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
    public class ProductsController : ControllerBase
    {
        private readonly IFiscalApiClient _fiscalApiClient;

        public ProductsController(IFiscalApiClient fiscalApiClient)
        {
            _fiscalApiClient = fiscalApiClient;
        }

        /// <summary>
        /// Obtener lista paginada de productos (hardcoded page=1, pageSize=2).
        /// Equivale a ObtenerProductosPagedList_Click en WinForms.
        /// </summary>
        [HttpGet("obtener-lista-paginada")]
        public async Task<IActionResult> ObtenerListaPaginada()
        {
            // pageNumber=1, pageSize=50 (hardcodeado)
            var apiResponse = await _fiscalApiClient.Products.GetListAsync(1, 10);

            if (apiResponse.Succeeded)
                return Ok(apiResponse.Data);

            return BadRequest(apiResponse);
        }

        /// <summary>
        /// Crear producto con datos fijos (no recibe body).
        /// Equivale a CrearProducto_Click en WinForms.
        /// </summary>
        [HttpPost("crear-producto")]
        public async Task<IActionResult> CrearProducto()
        {
            // Producto hardcodeado según WinForms
            var request = new Product
            {
                Description = "Consultoría de software",
                UnitPrice = 100, // Precio unitario
                SatUnitMeasurementId = "E48", // 'H87' por default
                SatTaxObjectId = "02", // '02' por default
                SatProductCodeId = "84111500" // '01010101' por default
                // ProductTaxes = [] // IVA 16% por default (si se omite, lo asume backend por defecto, segun doc).
            };

            var apiResponse = await _fiscalApiClient.Products.CreateAsync(request);

            if (apiResponse.Succeeded)
                return Ok(apiResponse.Data);

            return BadRequest(apiResponse);
        }

        /// <summary>
        /// Obtener un producto por Id (NO hardcodeado). 
        /// Equivale a ObtenerProductoById_Click, pero recibiendo el Id en la ruta.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            // Obtener producto por ID (id de la ruta, no hardcodeado).
            var apiResponse = await _fiscalApiClient.Products.GetByIdAsync(id);

            if (apiResponse.Succeeded)
                return Ok(apiResponse);

            return BadRequest(apiResponse);
        }

        /// <summary>
        /// Actualizar un producto con datos fijos (no recibe body).
        /// Equivale a ActualizarProducto_Click en WinForms (Id hardcodeado).
        /// </summary>
        [HttpPut("actualizar-producto/{id}")]
        public async Task<IActionResult> ActualizarProducto(string id)
        {
            var request = new Product
            {
                Id = id,
                Description = "Consultoría de software updated.",
                UnitPrice = 200,
                SatUnitMeasurementId = "E48",
                SatTaxObjectId = "01", // No objeto de impuesto
                SatProductCodeId = "01010101",
                // ProductTaxes = [] // si se omite, se mantienen los impuestos actuales en backend
            };

            var apiResponse = await _fiscalApiClient.Products.UpdateAsync(id, request);

            if (apiResponse.Succeeded)
                return Ok(apiResponse.Data);

            return BadRequest(apiResponse);
        }

        /// <summary>
        /// Actualizar un producto con nuevos impuestos 
        /// Equivale a ActualizarImpuestosProducto_Click en WinForms (Id hardcodeado).
        /// </summary>
        [HttpPut("actualizar-impuestos/{id}")]
        public async Task<IActionResult> ActualizarImpuestosProducto(string id)
        {
            var request = new Product
            {
                Id = id,
                Description = "Consultoría de software updated con impuestos",
                UnitPrice = 100,
                SatUnitMeasurementId = "E48",
                SatTaxObjectId = "02",
                SatProductCodeId = "84111500",
                ProductTaxes =
                    new
                        List<ProductTax> // Al incluir esta lista, se reemplazan los impuestos existentes por esta lista.
                        {
                            //Traslado IVA 16%
                            new ProductTax
                            {
                                Rate = 0.16m,
                                TaxId = "002",
                                TaxFlagId = "T",
                                TaxTypeId = "Tasa",
                            },
                            // Retención de ISR 10%
                            new ProductTax
                            {
                                Rate = 0.10m,
                                TaxId = "001",
                                TaxFlagId = "R",
                                TaxTypeId = "Tasa",
                            },
                            // Retención de IVA 2/3 partes
                            new ProductTax
                            {
                                Rate = 0.10666666666m,
                                TaxId = "002",
                                TaxFlagId = "R",
                                TaxTypeId = "Tasa",
                            }
                        }
            };

            var apiResponse = await _fiscalApiClient.Products.UpdateAsync(id, request);

            if (apiResponse.Succeeded)
                return Ok(apiResponse.Data);

            return BadRequest(apiResponse);
        }

        /// <summary>
        /// Obtener impuestos de un producto (NO hardcodeado el Id).
        /// Equivale a ObtenerImpuestosProducto_Click en WinForms, 
        /// pero recibiendo el Id en la ruta.
        /// </summary>
        [HttpGet("{id}/taxes")]
        public async Task<IActionResult> GetTaxes(string id)
        {
            // Obtener impuestos de un producto por Id de la ruta
            var apiResponse = await _fiscalApiClient.Products.GetTaxesAsync(id);

            if (apiResponse.Succeeded)
                return Ok(apiResponse.Data);

            return BadRequest(apiResponse);
        }

        /// <summary>
        /// Borrar producto por Id (NO hardcodeado).
        /// Equivale a BorrarProducto_Click en WinForms.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            // Borrar producto
            var apiResponse = await _fiscalApiClient.Products.DeleteAsync(id);

            if (apiResponse.Succeeded)
                return Ok(apiResponse.Data);

            return BadRequest(apiResponse);
        }
    }
}
using Fiscalapi.Abstractions;
using Fiscalapi.Common;
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
    public class DownloadRequestsController : ControllerBase
    {
        private readonly IFiscalApiClient _fiscalApiClient;

        public DownloadRequestsController(IFiscalApiClient fiscalApiClient)
        {
            _fiscalApiClient = fiscalApiClient;
        }

        /// <summary>
        /// Obtener lista paginada de solicitudes de descarga masiva.
        /// </summary>
        /// <remarks>
        /// Retorna las solicitudes con paginación (página 1, 10 elementos por defecto).
        /// </remarks>
        [HttpGet]
        public async Task<IActionResult> GetRequestsPaginated()
        {
            // Página 1, 10 elementos por página (hardcodeado según patrón)
            var apiResponse = await _fiscalApiClient.DownloadRequests.GetListAsync(1, 10);

            if (apiResponse.Succeeded)
                return Ok(apiResponse.Data);

            return BadRequest(apiResponse);
        }

        /// <summary>
        /// Crear una nueva solicitud de descarga masiva.
        /// </summary>
        /// <remarks>
        /// Genera una solicitud para descargar facturas de los últimos 5 días usando una regla predefinida.
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> CreateRequest()
        {
            var request = new DownloadRequest
            {
                DownloadRuleId = "89aba371-3f9a-431c-a92d-dcb1e606fcfd",
                DownloadRequestTypeId = "Manual",
                StartDate = DateTime.Now.AddDays(-5),
                EndDate = DateTime.Now,
            };

            var apiResponse = await _fiscalApiClient.DownloadRequests.CreateAsync(request);

            if (apiResponse.Succeeded)
                return Ok(apiResponse.Data);

            return BadRequest(apiResponse);
        }


        /// <summary>
        /// Obtener una solicitud de descarga masiva específica por su ID.
        /// </summary>
        /// <remarks>
        /// Permite consultar los detalles completos de una solicitud procesada.
        /// </remarks>
        /// <param name="id">ID único de la solicitud a consultar</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRequestById(string id)
        {
            var apiResponse = await _fiscalApiClient.DownloadRequests.GetByIdAsync(id);

            if (apiResponse.Succeeded)
                return Ok(apiResponse.Data);

            return BadRequest(apiResponse);
        }

        /// <summary>
        /// Eliminar una solicitud de descarga masiva por su ID.
        /// </summary>
        /// <remarks>
        /// Borra permanentemente la solicitud y todos sus datos asociados del sistema.
        /// </remarks>
        /// <param name="id">ID único de la solicitud a eliminar</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(string id)
        {
            var apiResponse = await _fiscalApiClient.DownloadRequests.DeleteAsync(id);

            if (apiResponse.Succeeded)
                return Ok(apiResponse.Data);

            return BadRequest(apiResponse);
        }

        /// <summary>
        /// Obtener lista paginada de archivos XML descargados asociados a una solicitud específica.
        /// </summary>
        /// <remarks>
        /// Retorna los CFDIs en formato XML que fueron descargados en la solicitud.
        /// </remarks>
        /// <param name="id">ID único de la solicitud de descarga</param>
        [HttpGet("{id}/xmls")]
        public async Task<IActionResult> GetRequestXmls(string id)
        {
            var apiResponse = await _fiscalApiClient.DownloadRequests.GetXmlsAsync(id);

            if (apiResponse.Succeeded)
                return Ok(apiResponse.Data);

            return BadRequest(apiResponse);
        }

        /// <summary>
        /// Obtener lista paginada de elementos de metadatos descargados asociados a una solicitud específica.
        /// </summary>
        /// <remarks>
        /// Retorna información resumida de los CFDIs sin el contenido XML completo.
        /// </remarks>
        /// <param name="id">ID único de la solicitud de descarga</param>
        [HttpGet("{id}/meta-items")]
        public async Task<IActionResult> GetRequestMetadataItems(string id)
        {
            var apiResponse = await _fiscalApiClient.DownloadRequests.GetMetadataItemsAsync(id);

            if (apiResponse.Succeeded)
                return Ok(apiResponse.Data);

            return BadRequest(apiResponse);
        }

        /// <summary>
        /// Descargar el paquete completo (archivo ZIP) de una solicitud de descarga masiva.
        /// </summary>
        /// <remarks>
        /// Guarda el archivo ZIP en disco y retorna confirmación de la operación.
        /// </remarks>
        /// <param name="id">ID único de la solicitud de descarga</param>
        [HttpGet("{id}/package")]
        public async Task<IActionResult> DownloadRequestPackage(string id)
        {
            var apiResponse = await _fiscalApiClient.DownloadRequests.DownloadPackageAsync(id);

            if (apiResponse.Succeeded)
            {
                var fileData = apiResponse.Data.FirstOrDefault();
                if (fileData != null)
                {
                    await WriteFileToDiskAsync(fileData);
                    return Ok(new
                        { message = "Archivo descargado y guardado en disco.", fileName = fileData.FileName });
                }

                return NotFound("No se encontró el archivo del paquete.");
            }

            return BadRequest(apiResponse);
        }

        /// <summary>
        /// Descargar el archivo XML de la solicitud enviada al SAT (para depuración/testing).
        /// </summary>
        /// <remarks>
        /// Guarda el XML en disco y retorna confirmación de la operación.
        /// </remarks>
        /// <param name="id">ID único de la solicitud de descarga</param>
        [HttpGet("{id}/raw-request")]
        public async Task<IActionResult> DownloadSatRequest(string id)
        {
            var apiResponse = await _fiscalApiClient.DownloadRequests.DownloadSatRequestAsync(id);

            if (apiResponse.Succeeded)
            {
                await WriteFileToDiskAsync(apiResponse.Data);
                return Ok(new
                    { message = "Archivo descargado y guardado en disco.", fileName = apiResponse.Data.FileName });
            }

            return BadRequest(apiResponse);
        }

        /// <summary>
        /// Descargar el archivo XML de la respuesta recibida del SAT (para depuración/testing).
        /// </summary>
        /// <remarks>
        /// Guarda el XML en disco y retorna confirmación de la operación.
        /// </remarks>
        /// <param name="id">ID único de la solicitud de descarga</param>
        [HttpGet("{id}/raw-response")]
        public async Task<IActionResult> DownloadSatResponse(string id)
        {
            var apiResponse = await _fiscalApiClient.DownloadRequests.DownloadSatResponseAsync(id);

            if (apiResponse.Succeeded)
            {
                await WriteFileToDiskAsync(apiResponse.Data);
                return Ok(new
                    { message = "Archivo descargado y guardado en disco.", fileName = apiResponse.Data.FileName });
            }

            return BadRequest(apiResponse);
        }

        [HttpGet("search")]
        public async Task<IActionResult> DownloadSatResponse(DateTime createdAt)
        {
            var apiResponse = await _fiscalApiClient.DownloadRequests.SearchAsync(createdAt);

            if (apiResponse.Succeeded)
            {
                return Ok(apiResponse);
            }

            return BadRequest(apiResponse);
        }


        /// <summary>
        /// Método auxiliar para escribir archivos en disco.
        /// Convierte el archivo Base64 a bytes y lo guarda en C:\facturas.
        /// </summary>
        /// <param name="fileResponse">Respuesta del archivo con contenido Base64</param>
        private static async Task WriteFileToDiskAsync(FileResponse fileResponse)
        {
            // Convertir Base64 a bytes
            var fileBytes = Convert.FromBase64String(fileResponse.Base64File);
            var filePath = Path.Combine(@"C:\facturas", fileResponse.FileName);

            // Crear directorio si no existe
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            await System.IO.File.WriteAllBytesAsync(filePath, fileBytes);
        }
    }
}
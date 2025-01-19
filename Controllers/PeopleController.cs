using Fiscalapi.Abstractions;
using Fiscalapi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FiscalApi.Samples.AspNet.Controllers
{
    /// <summary>
    /// Controlador para manejar personas (emisor, receptor, cliente, usuario, etc)
    /// En fiscalapi, una persona es un objeto que representa a un usuario del sistema, puede ser un emisor, receptor, cliente, usuario. Es el mismo objeto y mismo recurso, solo cambia su connotación (role) dependiendo del contexto.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IFiscalApiClient _fiscalApiClient;

        public PeopleController(IFiscalApiClient fiscalApiClient)
        {
            _fiscalApiClient = fiscalApiClient;
        }

        /// <summary>
        /// Obtener lista paginada de personas (emisor, receptor, cliente, usuario, etc)
        /// Equivale a ObtenerListaPaginada_Click en WinForms
        /// </summary>
        [HttpGet("obtener-lista-paginada")]
        public async Task<IActionResult> ObtenerListaPaginada()
        {
            // En WinForms: pageNumber=1, pageSize=2
            var apiResponse = await _fiscalApiClient.Persons.GetListAsync(1, 10);

            if (apiResponse.Succeeded)
            {
                // Retornamos 200 con los datos
                return Ok(apiResponse.Data);
            }
            else
            {
                return BadRequest(apiResponse);
            }
        }

        /// <summary>
        /// Crear persona (emisor, receptor, cliente, usuario, etc) con datos "hardcodeados" no recibe nada en el body.  Puedes recibir los datos del cuerpo de la solicitud asi agregando ([FromBody] Person person) en los parametros del metodo
        /// Equivale a CrearPersona_Click en WinForms
        /// </summary>
        [HttpPost("crear-persona")]
        public async Task<IActionResult> CrearPersona()
        {
            // Mismo ejemplo que en WinForms:
            var request = new Person
            {
                LegalName = "Persona de Prueba",
                Email = "someone4@somewhere.com",
                Password = "YourStrongPassword123!",
            };

            var apiResponse = await _fiscalApiClient.Persons.CreateAsync(request);

            if (apiResponse.Succeeded)
            {
                // Retornamos 200 con la persona creada
                return Ok(apiResponse.Data);
            }
            else
            {
                return BadRequest(apiResponse);
            }
        }

        /// <summary>
        /// Obtener persona (emisor, receptor, cliente, usuario, etc) por ID.
        /// Equivale a ObtenerPersonaPorID_Click en WinForms
        /// </summary>
        [HttpGet("obtener-persona-by-id/{id}")]
        public async Task<IActionResult> ObtenerPersonaPorId(string id)
        {
            var apiResponse = await _fiscalApiClient.Persons.GetByIdAsync(id);

            if (apiResponse.Succeeded)
            {
                return Ok(apiResponse.Data);
            }
            else
            {
                return BadRequest(apiResponse);
            }
        }

        /// <summary>
        /// Actualizar (emisor, receptor, cliente, usuario, etc) con datos "hardcodeados" (no recibe nada en el body) Puedes recibir los datos del cuerpo de la solicitud asi agregando ([FromBody] Person person) en los parametros del metodo
        /// Equivale a ActualizarPersona_Click en WinForms
        /// </summary>
        [HttpPut("actualizar-persona/{id}")]
        public async Task<IActionResult> ActualizarPersona(string id)
        {
            // Mismo ejemplo que en WinForms:
            var request = new Person
            {
                Id = id,
                LegalName = "Personita 2",
                SatTaxRegimeId = "601",
                SatCfdiUseId = "G01",
                Tin = "AAA010101AAA",
                ZipCode = "12345",
                Base64Photo = "base64",
            };

            var apiResponse = await _fiscalApiClient.Persons.UpdateAsync(id, request);

            if (apiResponse.Succeeded)
            {
                return Ok(apiResponse.Data);
            }
            else
            {
                return BadRequest(apiResponse);
            }
        }

        /// <summary>
        /// Borrar persona (emisor, receptor, cliente, usuario, etc) con ID 
        /// Equivale a BorrarPersona_Click en WinForms
        /// </summary>
        [HttpDelete("borrar-persona/{id}")]
        public async Task<IActionResult> BorrarPersona(string id)
        {
            var apiResponse = await _fiscalApiClient.Persons.DeleteAsync(id);

            if (apiResponse.Succeeded)
            {
                return Ok(apiResponse); // Se retorna lo que devuelva la API
            }
            else
            {
                return BadRequest(apiResponse);
            }
        }
    }
}
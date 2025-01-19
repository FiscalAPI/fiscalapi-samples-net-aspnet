using Fiscalapi.Abstractions;
using Fiscalapi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FiscalApi.Samples.AspNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificatesController : ControllerBase
    {
        private readonly IFiscalApiClient _fiscalApiClient;

        public CertificatesController(IFiscalApiClient fiscalApiClient)
        {
            _fiscalApiClient = fiscalApiClient;
        }

        /// <summary>
        /// Listar certificados (TaxFiles) con paginación (page=1, pageSize=2).
        /// Equivale a listarCertificados_Click en WinForms
        /// </summary>
        [HttpGet("listar-certificados")]
        public async Task<IActionResult> ListarCertificados()
        {
            var apiResponse = await _fiscalApiClient.TaxFiles.GetListAsync(1, 10);
            if (apiResponse.Succeeded)
            {
                // Retorna 200 con la lista paginada de TaxFiles
                return Ok(apiResponse);
            }
            else
            {
                // Retorna 400 con la respuesta completa si falló
                return BadRequest(apiResponse);
            }
        }

        /// <summary>
        /// Cargar dos certificados (CSD y clave privada) a una persona (personId)
        /// Equivale a CargarCertificados_Click en WinForms
        /// </summary>
        [HttpPost("cargar-certificados/{personId}")]
        public async Task<IActionResult> CargarCertificados(string personId)
        {
            // Datos de ejemplo tomados de WinForms:
            var certificadoCsd = new TaxFile
            {
                PersonId = personId,
                Base64File =
                    "MIIFsDCCA5igAwIBAgIUMzAwMDEwMDAwMDA1MDAwMDM0MTYwDQYJKoZIhvcNAQELBQAwggErMQ8wDQYDVQQDDAZBQyBVQVQxLjAsBgNVBAoMJVNFUlZJQ0lPIERFIEFETUlOSVNUUkFDSU9OIFRSSUJVVEFSSUExGjAYBgNVBAsMEVNBVC1JRVMgQXV0aG9yaXR5MSgwJgYJKoZIhvcNAQkBFhlvc2Nhci5tYXJ0aW5lekBzYXQuZ29iLm14MR0wGwYDVQQJDBQzcmEgY2VycmFkYSBkZSBjYWxpejEOMAwGA1UEEQwFMDYzNzAxCzAJBgNVBAYTAk1YMRkwFwYDVQQIDBBDSVVEQUQgREUgTUVYSUNPMREwDwYDVQQHDAhDT1lPQUNBTjERMA8GA1UELRMIMi41LjQuNDUxJTAjBgkqhkiG9w0BCQITFnJlc3BvbnNhYmxlOiBBQ0RNQS1TQVQwHhcNMjMwNTE4MTE0MzUxWhcNMjcwNTE4MTE0MzUxWjCB1zEnMCUGA1UEAxMeRVNDVUVMQSBLRU1QRVIgVVJHQVRFIFNBIERFIENWMScwJQYDVQQpEx5FU0NVRUxBIEtFTVBFUiBVUkdBVEUgU0EgREUgQ1YxJzAlBgNVBAoTHkVTQ1VFTEEgS0VNUEVSIFVSR0FURSBTQSBERSBDVjElMCMGA1UELRMcRUtVOTAwMzE3M0M5IC8gVkFEQTgwMDkyN0RKMzEeMBwGA1UEBRMVIC8gVkFEQTgwMDkyN0hTUlNSTDA1MRMwEQYDVQQLEwpTdWN1cnNhbCAxMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAtmecO6n2GS0zL025gbHGQVxznPDICoXzR2uUngz4DqxVUC/w9cE6FxSiXm2ap8Gcjg7wmcZfm85EBaxCx/0J2u5CqnhzIoGCdhBPuhWQnIh5TLgj/X6uNquwZkKChbNe9aeFirU/JbyN7Egia9oKH9KZUsodiM/pWAH00PCtoKJ9OBcSHMq8Rqa3KKoBcfkg1ZrgueffwRLws9yOcRWLb02sDOPzGIm/jEFicVYt2Hw1qdRE5xmTZ7AGG0UHs+unkGjpCVeJ+BEBn0JPLWVvDKHZAQMj6s5Bku35+d/MyATkpOPsGT/VTnsouxekDfikJD1f7A1ZpJbqDpkJnss3vQIDAQABox0wGzAMBgNVHRMBAf8EAjAAMAsGA1UdDwQEAwIGwDANBgkqhkiG9w0BAQsFAAOCAgEAFaUgj5PqgvJigNMgtrdXZnbPfVBbukAbW4OGnUhNrA7SRAAfv2BSGk16PI0nBOr7qF2mItmBnjgEwk+DTv8Zr7w5qp7vleC6dIsZFNJoa6ZndrE/f7KO1CYruLXr5gwEkIyGfJ9NwyIagvHHMszzyHiSZIA850fWtbqtythpAliJ2jF35M5pNS+YTkRB+T6L/c6m00ymN3q9lT1rB03YywxrLreRSFZOSrbwWfg34EJbHfbFXpCSVYdJRfiVdvHnewN0r5fUlPtR9stQHyuqewzdkyb5jTTw02D2cUfL57vlPStBj7SEi3uOWvLrsiDnnCIxRMYJ2UA2ktDKHk+zWnsDmaeleSzonv2CHW42yXYPCvWi88oE1DJNYLNkIjua7MxAnkNZbScNw01A6zbLsZ3y8G6eEYnxSTRfwjd8EP4kdiHNJftm7Z4iRU7HOVh79/lRWB+gd171s3d/mI9kte3MRy6V8MMEMCAnMboGpaooYwgAmwclI2XZCczNWXfhaWe0ZS5PmytD/GDpXzkX0oEgY9K/uYo5V77NdZbGAjmyi8cE2B2ogvyaN2XfIInrZPgEffJ4AB7kFA2mwesdLOCh0BLD9itmCve3A1FGR4+stO2ANUoiI3w3Tv2yQSg4bjeDlJ08lXaaFCLW2peEXMXjQUk7fmpb5MNuOUTW6BE=",
                FileType = FileType.CertificateCsd,
                Password = "12345678a",
                Tin = "EKU9003173C9", // RFC del certificado (Tax Identification Number)
            };

            var clavePrivadaCsd = new TaxFile
            {
                PersonId = personId,
                Base64File =
                    "MIIFDjBABgkqhkiG9w0BBQ0wMzAbBgkqhkiG9w0BBQwwDgQIAgEAAoIBAQACAggAMBQGCCqGSIb3DQMHBAgwggS/AgEAMASCBMh4EHl7aNSCaMDA1VlRoXCZ5UUmqErAbucoZQObOaLUEm+I+QZ7Y8Giupo+F1XWkLvAsdk/uZlJcTfKLJyJbJwsQYbSpLOCLataZ4O5MVnnmMbfG//NKJn9kSMvJQZhSwAwoGLYDm1ESGezrvZabgFJnoQv8Si1nAhVGTk9FkFBesxRzq07dmZYwFCnFSX4xt2fDHs1PMpQbeq83aL/PzLCce3kxbYSB5kQlzGtUYayiYXcu0cVRu228VwBLCD+2wTDDoCmRXtPesgrLKUR4WWWb5N2AqAU1mNDC+UEYsENAerOFXWnmwrcTAu5qyZ7GsBMTpipW4Dbou2yqQ0lpA/aB06n1kz1aL6mNqGPaJ+OqoFuc8Ugdhadd+MmjHfFzoI20SZ3b2geCsUMNCsAd6oXMsZdWm8lzjqCGWHFeol0ik/xHMQvuQkkeCsQ28PBxdnUgf7ZGer+TN+2ZLd2kvTBOk6pIVgy5yC6cZ+o1Tloql9hYGa6rT3xcMbXlW+9e5jM2MWXZliVW3ZhaPjptJFDbIfWxJPjz4QvKyJk0zok4muv13Iiwj2bCyefUTRz6psqI4cGaYm9JpscKO2RCJN8UluYGbbWmYQU+Int6LtZj/lv8p6xnVjWxYI+rBPdtkpfFYRp+MJiXjgPw5B6UGuoruv7+vHjOLHOotRo+RdjZt7NqL9dAJnl1Qb2jfW6+d7NYQSI/bAwxO0sk4taQIT6Gsu/8kfZOPC2xk9rphGqCSS/4q3Os0MMjA1bcJLyoWLp13pqhK6bmiiHw0BBXH4fbEp4xjSbpPx4tHXzbdn8oDsHKZkWh3pPC2J/nVl0k/yF1KDVowVtMDXE47k6TGVcBoqe8PDXCG9+vjRpzIidqNo5qebaUZu6riWMWzldz8x3Z/jLWXuDiM7/Yscn0Z2GIlfoeyz+GwP2eTdOw9EUedHjEQuJY32bq8LICimJ4Ht+zMJKUyhwVQyAER8byzQBwTYmYP5U0wdsyIFitphw+/IH8+v08Ia1iBLPQAeAvRfTTIFLCs8foyUrj5Zv2B/wTYIZy6ioUM+qADeXyo45uBLLqkN90Rf6kiTqDld78NxwsfyR5MxtJLVDFkmf2IMMJHTqSfhbi+7QJaC11OOUJTD0v9wo0X/oO5GvZhe0ZaGHnm9zqTopALuFEAxcaQlc4R81wjC4wrIrqWnbcl2dxiBtD73KW+wcC9ymsLf4I8BEmiN25lx/OUc1IHNyXZJYSFkEfaxCEZWKcnbiyf5sqFSSlEqZLc4lUPJFAoP6s1FHVcyO0odWqdadhRZLZC9RCzQgPlMRtji/OXy5phh7diOBZv5UYp5nb+MZ2NAB/eFXm2JLguxjvEstuvTDmZDUb6Uqv++RdhO5gvKf/AcwU38ifaHQ9uvRuDocYwVxZS2nr9rOwZ8nAh+P2o4e0tEXjxFKQGhxXYkn75H3hhfnFYjik/2qunHBBZfcdG148MaNP6DjX33M238T9Zw/GyGx00JMogr2pdP4JAErv9a5yt4YR41KGf8guSOUbOXVARw6+ybh7+meb7w4BeTlj3aZkv8tVGdfIt3lrwVnlbzhLjeQY6PplKp3/a5Kr5yM0T4wJoKQQ6v3vSNmrhpbuAtKxpMILe8CQoo=",
                FileType = FileType.PrivateKeyCsd,
                Password = "12345678a",
                Tin = "EKU9003173C9", // RFC del certificado (Tax Identification Number)
            };

            // Subir el certificado
            var apiResponseCer = await _fiscalApiClient.TaxFiles.CreateAsync(certificadoCsd);
            // Subir la clave privada
            var apiResponseKey = await _fiscalApiClient.TaxFiles.CreateAsync(clavePrivadaCsd);

            // Construimos un objeto que muestre la respuesta de ambas creaciones
            var combinedResult = new
            {
                CertificadoCsdResponse = apiResponseCer,
                ClavePrivadaCsdResponse = apiResponseKey
            };

            // Si ambos fueron exitosos, retornamos 200 con el detalle de ambas respuestas
            // De otro modo, puedes decidir si retornas 400 de inmediato, o siempre 200 con parte de la información.
            // Para simplicidad, retornaremos 200 con las dos respuestas integrales. El cliente evaluará los .Succeeded
            return Ok(combinedResult);
        }

        /// <summary>
        /// Obtener un certificado por ID
        /// Equivale a ObtenerCertificadoById_Click en WinForms
        /// </summary>
        [HttpGet("obtener-certificado-by-id/{id}")]
        public async Task<IActionResult> ObtenerCertificadoById(string id)
        {
            // Tomado del WinForms: "ada2ed06-d060-4fb5-948d-16903f3844ee"

            var apiResponse = await _fiscalApiClient.TaxFiles.GetByIdAsync(id);

            if (apiResponse.Succeeded)
            {
                // Retornamos 200 con el taxFile
                return Ok(apiResponse.Data);
            }
            else
            {
                return BadRequest(apiResponse);
            }
        }

        /// <summary>
        /// Eliminar un certificado por ID 
        /// Equivale a EliminaCertificado_Click en WinForms
        /// </summary>
        [HttpDelete("eliminar-certificado/{id}")]
        public async Task<IActionResult> EliminarCertificado(string id)
        {
            var apiResponse = await _fiscalApiClient.TaxFiles.DeleteAsync(id);

            if (apiResponse.Succeeded)
            {
                return Ok(apiResponse);
            }
            else
            {
                return BadRequest(apiResponse);
            }
        }

        /// <summary>
        /// Obtener los últimos valores (certificados) válidos de una persona por su ID
        /// Equivale a CertDefaultValues_Click en WinForms
        /// </summary>
        [HttpGet("cert-default-values/{personId}")]
        public async Task<IActionResult> CertDefaultValues(string personId)
        {
            var apiResponse = await _fiscalApiClient.TaxFiles.GetDefaultValuesAsync(personId);

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
        /// Obtener las últimas referencias (IDs) a certificados válidos de una persona por su Id.
        /// Equivale a CertDefaultRefs_Click en WinForms
        /// </summary>
        [HttpGet("cert-default-refs/{personId}")]
        public async Task<IActionResult> CertDefaultRefs(string personId)
        {
            var apiResponse = await _fiscalApiClient.TaxFiles.GetDefaultReferencesAsync(personId);

            if (apiResponse.Succeeded)
            {
                return Ok(apiResponse.Data);
            }
            else
            {
                return BadRequest(apiResponse);
            }
        }
    }
}
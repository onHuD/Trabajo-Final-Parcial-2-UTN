using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CryptoWallet.Models;

namespace CryptoWallet.Controllers
{
    [ApiController]
    [Route("api/transacciones")]
    public class TransaccionesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;

        public TransaccionesController(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient();
        }

        // GET /api/transacciones
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var transacciones = await _context.Transacciones.ToListAsync();
            return Ok(transacciones);
        }

        // GET /api/transacciones/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var transaccion = await _context.Transacciones.FindAsync(id);
            if (transaccion == null)
                return NotFound();
            return Ok(transaccion);
        }

        // POST /api/transacciones
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Transaccion transaccion)
        {
            if (transaccion.CryptoAmount <= 0)
                return BadRequest("La cantidad debe ser mayor a 0");

            // Llamamos a CriptoYa para obtener el precio actual
            var exchange = "satoshitango";
            var url = $"https://criptoya.com/api/{exchange}/{transaccion.CryptoCode}/ars";

            var response = await _httpClient.GetFromJsonAsync<CriptoYaResponse>(url);

            if (response == null)
                return StatusCode(500, "Error al obtener el precio de CriptoYa");

            // Si es compra usamos ask, si es venta usar bid
            var precio = transaccion.Action == "purchase" ? response.Ask : response.Bid;

            transaccion.Money = transaccion.CryptoAmount * precio;

            _context.Transacciones.Add(transaccion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = transaccion.Id }, transaccion);
        }

        // PATCH /api/transacciones/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TransaccionUpdateDto dto)
        {
            var transaccion = await _context.Transacciones.FindAsync(id);
            if (transaccion == null)
                return NotFound();

            if (dto.CryptoCode != null) transaccion.CryptoCode = dto.CryptoCode;
            if (dto.Action != null) transaccion.Action = dto.Action;
            if (dto.CryptoAmount.HasValue) transaccion.CryptoAmount = dto.CryptoAmount.Value;
            if (dto.Money.HasValue) transaccion.Money = dto.Money.Value;
            if (dto.DateTime.HasValue) transaccion.DateTime = dto.DateTime.Value;

            await _context.SaveChangesAsync();
            return Ok(transaccion);
        }

        // DELETE /api/transacciones/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var transaccion = await _context.Transacciones.FindAsync(id);
            if (transaccion == null)
                return NotFound();

            _context.Transacciones.Remove(transaccion);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    // Clase para mapear la respuesta de CriptoYa
    public class CriptoYaResponse
    {
        public decimal Ask { get; set; }
        public decimal TotalAsk { get; set; }
        public decimal Bid { get; set; }
        public decimal TotalBid { get; set; }
    }

    // Clase para el PATCH (todos los campos opcionales)
    public class TransaccionUpdateDto
    {
        public string? CryptoCode { get; set; }
        public string? Action { get; set; }
        public decimal? CryptoAmount { get; set; }
        public decimal? Money { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
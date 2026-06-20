using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CryptoWallet.Models;

namespace CryptoWallet.Controllers
{
    [ApiController]
    [Route("api/portfolio")]
    public class PortfolioController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;

        public PortfolioController(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient();
        }

        // GET /api/portofolio
        [HttpGet]
        public async Task<IActionResult> GetPortfolio()
        {
            var transacciones = await _context.Transacciones.ToListAsync();

            // Agrupo por cripto y calculamos cantidad total
            var balances = transacciones
                .GroupBy(t => t.CryptoCode)
                .Select(g => new
                {
                    CryptoCode = g.Key,
                    Cantidad = g.Sum(t => t.Action == "purchase"
                        ? t.CryptoAmount
                        : -t.CryptoAmount)
                })
                .Where(b => b.Cantidad > 0)
                .ToList();

            if (!balances.Any())
                return Ok(new { items = new List<object>(), total = 0 });

            var exchange = "satoshitango";
            var items = new List<object>();
            decimal total = 0;

            foreach (var balance in balances)
            {
                var url = $"https://criptoya.com/api/satoshitango/{balance.CryptoCode}/ars";

                try
                {
                    var response = await _httpClient.GetFromJsonAsync<CriptoYaResponse>(url);

                    if (response == null || response.Bid == 0) continue;

                    var valorTotal = balance.Cantidad * response.Bid;
                    total += valorTotal;

                    items.Add(new
                    {
                        cryptoCode = balance.CryptoCode,
                        cantidad = balance.Cantidad,
                        precioActual = response.Bid,
                        valorEnArs = valorTotal
                    });
                }
                catch
                {
                    continue;
                }
            }

            return Ok(new { items, total });
        }
    }
}

/* En el dia 10/6 tube que poner //criptoya.com/api/satoshitango/ por problemas de conexion request..*/
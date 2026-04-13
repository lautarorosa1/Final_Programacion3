public class CriptoYaService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CriptoYaService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    private readonly string[] exchanges = new[] //SOLO EXCHANGES QUE DEVUELVAN DATOS ARS
    {
        "satoshitango",
        "buenbit",
        "ripio",
        "lemoncash",
        "argenbtc",
        "binance",
        "fiwind",
        "belo",
        "letsbit",
        "tiendacrypto"
    };

    public async Task<List<(string exchange, CriptoYaResponse data)>> ObtenerPreciosTodos(string cryptoCode)
    {
        var client = _httpClientFactory.CreateClient();

        var tasks = exchanges.Select(async ex =>
        {
            var url = $"https://criptoya.com/api/{ex}/{cryptoCode}/ars";

            try
            {
                var data = await client.GetFromJsonAsync<CriptoYaResponse>(url);
                return (exchange: ex, data);
            }
            catch
            {
                return (exchange: ex, data: null);
            }
        });

        var resultados = await Task.WhenAll(tasks);

        return resultados
            .Where(r => r.data != null)
            .Select(r => (r.exchange, r.data!))
            .ToList();
    }

    public async Task<List<object>> ObtenerRanking(string cryptoCode, string tipo)
    {
        var precios = await ObtenerPreciosTodos(cryptoCode);

        if (!precios.Any()) return new List<object>();

        var ranking = precios.Select(p => new {
            exchange = p.exchange,
            precioCompra = p.data.ask,
            precioVenta = p.data.bid
        }).ToList();

        // Ordenar según tipo
        if (tipo == "purchase")
            ranking = ranking.OrderBy(p => p.precioCompra).ToList();
        else
            ranking = ranking.OrderByDescending(p => p.precioVenta).ToList();

        return ranking.Cast<object>().ToList();
    }

    public class CriptoYaResponse
    {
        public decimal ask { get; set; }
        public decimal totalAsk { get; set; }
        public decimal bid { get; set; }
        public decimal totalBid { get; set; }
        public long time { get; set; }
    }
}
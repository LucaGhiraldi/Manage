using Manage.Models.NetWorth;
using Newtonsoft.Json;
using static Manage.Shared.MarketDataService;

namespace Manage.Shared
{
    public class MarketDataService
    {
        private readonly HttpClient _httpClient;
        private readonly string _finnhubApiKey;
        private readonly string _alphaVantageApiKey;
        private readonly string _apiKeyFinancialmodelingprep;

        public MarketDataService(HttpClient httpClient, string finnhubApiKey, string alphaVantageApiKey, string apiKeyFinancialmodelingprep)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            _finnhubApiKey = finnhubApiKey ?? throw new ArgumentNullException(nameof(finnhubApiKey));
            _alphaVantageApiKey = alphaVantageApiKey ?? throw new ArgumentNullException(nameof(alphaVantageApiKey));
            _apiKeyFinancialmodelingprep = apiKeyFinancialmodelingprep ?? throw new ArgumentNullException(nameof(apiKeyFinancialmodelingprep));
        }

        // Ottieni tutti i simboli di stock dall'API Finnhub
        public async Task<IEnumerable<StockSymbol>> GetAllStockSymbolsAsync()
        {
            var url = $"https://finnhub.io/api/v1/stock/symbol?exchange=US&token={_finnhubApiKey}";

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                return JsonConvert.DeserializeObject<IEnumerable<StockSymbol>>(response);
            }
            catch (Exception ex)
            {
                // Logga l'errore (aggiungere un logger qui)
                throw new Exception("Errore durante il recupero dei simboli delle azioni.", ex);
            }
        }

        // Ottieni il prezzo attuale di un'azione dall'API Finnhub
        public async Task<decimal> GetCurrentPriceStockAsync(string symbol)
        {
            var url = $"https://finnhub.io/api/v1/quote?symbol={symbol}&token={_finnhubApiKey}";

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                var priceData = JsonConvert.DeserializeObject<PriceResponse>(response);
                return priceData.Current;
            }
            catch (Exception ex)
            {
                // Logga l'errore (aggiungere un logger qui)
                throw new Exception($"Errore durante il recupero del prezzo corrente per il simbolo {symbol}.", ex);
            }
        }

        public async Task<DividendiAzione> GetDividendsForStockAsync(string symbol)
        {
            var url = $"https://www.alphavantage.co/query?function=DIVIDENDS&symbol={symbol}&apikey={_alphaVantageApiKey}";

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                return JsonConvert.DeserializeObject<DividendiAzione>(response);
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore durante il recupero dei dividendi per {symbol}.", ex);
            }
        }

        // Ottieni tutti i simboli di criptovaluta da CoinGecko
        public async Task<IEnumerable<CryptoSymbol>> GetAllCryptoSymbolsAsync()
        {
            var url = "https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd";

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                return JsonConvert.DeserializeObject<IEnumerable<CryptoSymbol>>(response);
            }
            catch (Exception ex)
            {
                // Logga l'errore (aggiungere un logger qui)
                throw new Exception("Errore durante il recupero dei simboli delle criptovalute.", ex);
            }
        }

        // Ottieni il prezzo attuale di una criptovaluta da CoinGecko
        public async Task<decimal> GetCurrentPriceCryptoAsync(string cryptoSymbol)
        {
            var url = $"https://api.coingecko.com/api/v3/simple/price?ids={cryptoSymbol}&vs_currencies=usd";

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                var cryptoPriceResponse = JsonConvert.DeserializeObject<Dictionary<string, CryptoCurrencyPrice>>(response);

                if (cryptoPriceResponse != null && cryptoPriceResponse.ContainsKey(cryptoSymbol))
                {
                    return cryptoPriceResponse[cryptoSymbol].usd;
                }

                throw new Exception($"Simbolo criptovaluta non trovato: {cryptoSymbol}");
            }
            catch (Exception ex)
            {
                // Logga l'errore (aggiungere un logger qui)
                throw new Exception($"Errore durante il recupero del prezzo per {cryptoSymbol}.", ex);
            }
        }

        // --------------------

        public async Task<IEnumerable<SymbolFinancial>> GetAllStockSymbolsAsyncFinancial()
        {
            var url = $"https://financialmodelingprep.com/api/v3/stock/list?apikey={_apiKeyFinancialmodelingprep}";

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                return JsonConvert.DeserializeObject<IEnumerable<SymbolFinancial>>(response);
            }
            catch (Exception ex)
            {
                // Logga l'errore (aggiungere un logger qui)
                throw new Exception($"Errore durante il recupero delle stocks.", ex);
            }
        }

        public async Task<IEnumerable<SymbolFinancial>> GetAllETFSymbolsAsyncFinancial()
        {
            var url = $"https://financialmodelingprep.com/api/v3/etf/list?apikey={_apiKeyFinancialmodelingprep}";

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                return JsonConvert.DeserializeObject<IEnumerable<SymbolFinancial>>(response);
            }
            catch (Exception ex)
            {
                // Logga l'errore (aggiungere un logger qui)
                throw new Exception($"Errore durante il recupero degli ETF.", ex);
            }
        }

        public async Task<StockSymbolFinancial> GetStockySymbolsAsyncFinancial(string symbol)
        {
            var url = $"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={_apiKeyFinancialmodelingprep}";

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                return JsonConvert.DeserializeObject<StockSymbolFinancial>(response);
            }
            catch (Exception ex)
            {
                // Logga l'errore (aggiungere un logger qui)
                throw new Exception($"Errore durante il recupero della stock {symbol}.", ex);
            }
        }

        // Ottieni il prezzo attuale di un'azione dall'API Finnhub
        public async Task<decimal> GetCurrentPriceStockAsyncFinancial(string symbol)
        {
            var url = $"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={_apiKeyFinancialmodelingprep}";

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                var priceData = JsonConvert.DeserializeObject<StockSymbolFinancial>(response);
                return priceData.price;
            }
            catch (Exception ex)
            {
                // Logga l'errore (aggiungere un logger qui)
                throw new Exception($"Errore durante il recupero del prezzo corrente per il simbolo {symbol}.", ex);
            }
        }

        public async Task<SymbolFinancial> GetETFSymbolsAsyncFinancial(string symbol)
        {
            var url = $"https://financialmodelingprep.com/api/v3/etf/list?apikey={_apiKeyFinancialmodelingprep}";

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                var values = JsonConvert.DeserializeObject<IEnumerable<SymbolFinancial>>(response);
                var value = values.FirstOrDefault(v => v.symbol.Contains(symbol));

                return value;
            }
            catch (Exception ex)
            {
                // Logga l'errore (aggiungere un logger qui)
                throw new Exception($"Errore durante il recupero dell'etf {symbol}.", ex);
            }
        }

        public async Task<StockDividendSymbolFinancial> GetStockyDividedSymbolsAsyncFinancial(string symbol)
        {
            var url = $"https://financialmodelingprep.com/api/v3/historical-price-full/stock_dividend/{symbol}?apikey={_apiKeyFinancialmodelingprep}";

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                return JsonConvert.DeserializeObject<StockDividendSymbolFinancial>(response);
            }
            catch (Exception ex)
            {
                // Logga l'errore (aggiungere un logger qui)
                throw new Exception($"Errore durante il recupero della stock {symbol}.", ex);
            }
        }

        public async Task<CommoditySymbolFinancial> GetCommoditySymbolsAsyncFinancial(string symbol)
        {
            var url = $"https://financialmodelingprep.com/api/v3/quote/{symbol}?apikey={_apiKeyFinancialmodelingprep}";

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                return JsonConvert.DeserializeObject<CommoditySymbolFinancial>(response);
            }
            catch (Exception ex)
            {
                // Logga l'errore (aggiungere un logger qui)
                throw new Exception($"Errore durante il recupero della stock {symbol}.", ex);
            }
        }

        // --------------------

        public class SymbolFinancial
        {
            public string symbol { get; set; }
            public string name { get; set; }
            public decimal price { get; set; }
            public string exchange { get; set; }
            public string exchangeShortName { get; set; }
            public string type { get; set; }
        }

        public class StockSymbolFinancial
        {
            public string symbol { get; set; }
            public decimal price { get; set; }
            public decimal beta { get; set; }
            public decimal volAvg { get; set; }
            public decimal mktCap { get; set; }
            public decimal lastDiv { get; set; }
            public string range { get; set; }
        }

        public class StockDividendSymbolFinancial
        {
            public string Symbol { get; set; }
            public List<HistoricalDividend> Historical { get; set; }
        }

        public class HistoricalDividend
        {
            public string Date { get; set; }
            public string Label { get; set; }
            public decimal AdjDividend { get; set; }
            public decimal Dividend { get; set; }
            public string RecordDate { get; set; }
            public string PaymentDate { get; set; }
            public string DeclarationDate { get; set; }
        }

        public class CommoditySymbolFinancial
        {
            public string symbol { get; set; }
            public string name { get; set; }
            public decimal price { get; set; }
        }

        // Modelli per la deserializzazione
        public class StockSymbol
        {
            public string Symbol { get; set; }
            public string Name { get; set; }
            public string Isin { get; set; }
        }

        public class CryptoSymbol
        {
            public string Id { get; set; }
            public string Symbol { get; set; }
            public string Name { get; set; }
        }

        public class PriceResponse
        {
            [JsonProperty("c")]
            public decimal Current { get; set; } // Prezzo attuale

            [JsonProperty("h")]
            public decimal High { get; set; }    // Prezzo massimo

            [JsonProperty("l")]
            public decimal Low { get; set; }     // Prezzo minimo

            [JsonProperty("o")]
            public decimal Open { get; set; }    // Prezzo di apertura

            [JsonProperty("pc")]
            public decimal PreviousClose { get; set; } // Prezzo di chiusura precedente
        }

        public class CryptoCurrencyPrice
        {
            public decimal usd { get; set; } // Prezzo della criptovaluta in USD
        }

    }
}

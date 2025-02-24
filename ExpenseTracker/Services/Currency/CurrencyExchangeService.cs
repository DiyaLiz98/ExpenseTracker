using Azure;
using ExpenseTracker.Interfaces;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json;

namespace ExpenseTracker.Services.Currency
{
    public sealed class CurrencyExchangeService : ICurrencyExchangeService
    {
        //private static readonly Lazy<CurrencyExchangeService> _instance = new(() => new CurrencyExchangeService());

        private readonly HttpClient _httpClient;
        private readonly Dictionary<string, decimal> _exchangeRates;
        private readonly string _apiUrl, _apiKey;
        public CurrencyExchangeService(HttpClient httpClient, string apiUrl, string apiKey)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiUrl = apiUrl ?? throw new ArgumentNullException(nameof(apiUrl));
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiUrl));
            _exchangeRates = new Dictionary<string, decimal>();
        }

        

        public async Task UpdateExchangeRatesAsync()
        {
            string requestUrl = $"{_apiUrl}?access_key={_apiKey}";
            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
            if(response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ExchangeRateResponse>(responseBody);

                if(data?.Rates != null)
                {
                    lock(_exchangeRates)
                    {
                        _exchangeRates.Clear();
                        foreach(var rate in data.Rates)
                        {
                            _exchangeRates[rate.Key] = rate.Value;
                        }
                    }
                }

            }

        }
        public decimal ConvertCurrency(decimal amount, string fromCurrency, string toCurrency)
        {
            lock (_exchangeRates)
            {
                if (_exchangeRates.ContainsKey(fromCurrency) && _exchangeRates.ContainsKey(toCurrency))
                {
                    decimal fromRate = _exchangeRates[fromCurrency];
                    decimal toRate = _exchangeRates[toCurrency];
                    return (amount / fromRate) * toRate;
                }
                throw new Exception("Currency rates not available.");
            }
        }

        public async Task<decimal?> GetExchangeRateAsync(string currencyCode)
        {
            lock(_exchangeRates)
            {
                if(_exchangeRates.ContainsKey(currencyCode))
                {
                    return _exchangeRates[currencyCode];
                }
            }

            await UpdateExchangeRatesAsync();

            lock (_exchangeRates)
            {
                if (_exchangeRates.ContainsKey(currencyCode))
                {
                    return _exchangeRates[currencyCode];
                }
            }
            return null;
        }
        private class ExchangeRateResponse
        {
            public Dictionary<string, decimal> Rates { get; set; }
        }
    }

}

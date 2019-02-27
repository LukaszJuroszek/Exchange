using Exchange.Core.DTOs;
using Exchange.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Exchange.Core.Services
{
    public class NbpApiService : INbpApiService
    {
        private readonly string _baseApiUri = "http://api.nbp.pl/api";
        private readonly IHttpClientFactory _httpClientFactory;

        public NbpApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<RateDto> GetExchangeRateAsync(string iso4217Code)
        {
            var client = _httpClientFactory.CreateClient();
            var result = await client.GetAsync($@"{_baseApiUri}/exchangerates/rates/a/{iso4217Code.ToLower()}/");

            //handle error
            result.EnsureSuccessStatusCode();

            var rates = await result.Content.ReadAsAsync<RatesDto>();

            return rates.Rates.FirstOrDefault();
        }

        public async Task<IEnumerable<Currency>> GetAllCurrenciesAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var result = await client.GetAsync($@"{_baseApiUri}/exchangerates/tables/a/");
            var test = await result.Content.ReadAsStringAsync();
            var allRates = await result.Content.ReadAsAsync<List<ExchangeRatesTableDto>>();

            return allRates.First().Rates.Select(x => new Currency { Iso4217CurrencyCode = x.Code });
        }
    }
}

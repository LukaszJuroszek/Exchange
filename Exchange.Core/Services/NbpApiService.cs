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
        private readonly string _baseApiUri = "http://api.nbp.pl/api/exchangerates";
        private readonly IHttpClientFactory _httpClientFactory;

        public NbpApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Currency> GetExchangeRateAsync(string iso4217Code)
        {
            var client = _httpClientFactory.CreateClient();
            var result = await client.GetAsync($@"{_baseApiUri}/rates/a/{iso4217Code.ToLower()}/");

            //handle error
            result.EnsureSuccessStatusCode();

            var rates = await result.Content.ReadAsAsync<RatesDto>();

            // handle null
            var rate = rates.Rates.FirstOrDefault();

            return new Currency { Iso4217CurrencyCode = rate.Code, Mid = rate.Mid, Name = rate.Currency };
        }

        public async Task<IEnumerable<Currency>> GetAllCurrenciesAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var result = await client.GetAsync($@"{_baseApiUri}/tables/a/");

            var allRates = await result.Content.ReadAsAsync<List<ExchangeRatesTableDto>>();

            return allRates.First().Rates.Select(x => new Currency { Iso4217CurrencyCode = x.Code, Mid = x.Mid, Name = x.Currency });
        }

        public async Task<IEnumerable<Currency>> GetCurrencyHistoryAsync(Currency currency, int lastDays = 30)
        {
            var client = _httpClientFactory.CreateClient();
            var result = await client.GetAsync($@"{_baseApiUri}/rates/a/{currency.Iso4217CurrencyCode.ToLower()}/last/{lastDays}");

            var exchangeRatesTableDto = await result.Content.ReadAsAsync<ExchangeRatesTableDto>();

            return exchangeRatesTableDto.Rates
                .Select(x =>
                    new Currency
                    {
                        Iso4217CurrencyCode = exchangeRatesTableDto.Code,
                        Mid = x.Mid,
                        Name = exchangeRatesTableDto.Currency,
                        EffectiveDate = x.EffectiveDate
                    });
        }
    }
}

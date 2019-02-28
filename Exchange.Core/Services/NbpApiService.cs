using Exchange.Core.DTOs;
using Exchange.Core.Models;
using System;
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
        //for base currency
        private readonly string _plnIso4217CurrencyCode = "PLN";
        private readonly string _plnName = "polski złoty";
        private readonly decimal _plnMid = 1.0m;

        public NbpApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Currency> GetExchangeRateAsync(string iso4217Code)
        {
            //Add missing pln currency that not present in api
            if (iso4217Code == _plnIso4217CurrencyCode)
                return new Currency { Iso4217CurrencyCode = _plnIso4217CurrencyCode, Mid = _plnMid, Name = _plnName };

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
            var currencies = allRates.First().Rates.Select(x =>
                new Currency
                {
                    Iso4217CurrencyCode = x.Code,
                    Mid = x.Mid,
                    Name = x.Currency
                }).ToList();

            //Add missing pln currency that not present in api
            currencies.Add(new Currency { Iso4217CurrencyCode = _plnIso4217CurrencyCode, Mid = _plnMid, Name = _plnName });

            return currencies;
        }

        public async Task<IEnumerable<Currency>> GetCurrencyHistoryAsync(Currency currency, int lastDays = 30)
        {
            //Add missing pln currency that not present in api

            if (currency.Iso4217CurrencyCode == _plnIso4217CurrencyCode)
            {
                var plnCurrencies = new List<Currency>();
                var startDate = DateTime.Now.Date;
                //check if current day is not weekend
                startDate = CheckIfDateIsInWeekendDay(startDate);
                for (int i = 0; i < lastDays; i++)
                {
                    plnCurrencies.Add(new Currency
                    {
                        Iso4217CurrencyCode = _plnIso4217CurrencyCode,
                        Mid = _plnMid,
                        Name = _plnName,
                        EffectiveDate = startDate
                    });
                    startDate = startDate.AddDays(-1);

                    //check if current day is not weekend
                    startDate = CheckIfDateIsInWeekendDay(startDate);
                }

                return plnCurrencies.AsEnumerable();
            }

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

        private DateTime CheckIfDateIsInWeekendDay(DateTime startDate)
        {
            if (startDate.DayOfWeek == DayOfWeek.Saturday)
                startDate = startDate.AddDays(-1);
            else if (startDate.DayOfWeek == DayOfWeek.Sunday)
                startDate = startDate.AddDays(-2);
            return startDate;
        }
    }
}

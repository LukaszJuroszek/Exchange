using Exchange.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Exchange.Core.Services
{
    public class ExchangeService : IExchangeService
    {
        private readonly INbpApiService _nbpApiService;

        public ExchangeService(INbpApiService nbpApiService)
        {
            _nbpApiService = nbpApiService;
        }

        public async Task<decimal> ExchangeAsync(string fromIso4217CurrencyCode, string toIso4217CurrencyCode, decimal amount)
        {
            var from = await GetCurrencyFromCodeAsync(fromIso4217CurrencyCode);
            var to = await GetCurrencyFromCodeAsync(toIso4217CurrencyCode);

            if (amount <= decimal.Zero)
                return 0.0m;

            if (from.Iso4217CurrencyCode == to.Iso4217CurrencyCode)
                return amount;

            var rate = await CurrentExchangeRateAsync(from, to);
            return amount * rate;
        }

        public async Task<decimal> GetMidFromCurrencyAsync(Currency currency)
        {
            var result = await _nbpApiService.GetExchangeRateAsync(currency.Iso4217CurrencyCode);
            return result.Mid;
        }

        private async Task<decimal> CurrentExchangeRateAsync(Currency from, Currency to)
        {
            var fromMid = await GetMidFromCurrencyAsync(from);
            var toMid = await GetMidFromCurrencyAsync(to);
            return fromMid / toMid;
        }

        public async Task<Currency> GetCurrencyFromCodeAsync(string iso4217CurrencyCode)
        {
            var result = await _nbpApiService.GetAllCurrenciesAsync();

            return result.SingleOrDefault(x => String.Equals(x.Iso4217CurrencyCode, iso4217CurrencyCode, StringComparison.CurrentCultureIgnoreCase));
        }

        public async Task<DateTime> GetDateOfAdvantageousExchangeAsync(string fromIso4217CurrencyCode, string toIso4217CurrencyCode, int inLastDay)
        {
            var from = await GetCurrencyFromCodeAsync(fromIso4217CurrencyCode);
            var to = await GetCurrencyFromCodeAsync(toIso4217CurrencyCode);

            var midFromCurrencyHistory = await _nbpApiService.GetCurrencyHistoryAsync(from, inLastDay);
            var midToCurrencyHistory = await _nbpApiService.GetCurrencyHistoryAsync(to, inLastDay);

            var currenciesParisByEffectiveDate = midFromCurrencyHistory.Select(fromCurrency => new
            {
                fromCurrency.EffectiveDate,
                FromCurrency = fromCurrency,
                ToCurrency = midToCurrencyHistory.FirstOrDefault(c => c.EffectiveDate == fromCurrency.EffectiveDate)
            });

            if (currenciesParisByEffectiveDate.Any(x => x.ToCurrency is null))
                throw new Exception("History pair not valid");

            var maxExchangeRatioDate = currenciesParisByEffectiveDate
                .FirstOrDefault(x => (x.FromCurrency.Mid / x.ToCurrency.Mid) == currenciesParisByEffectiveDate.Max(m => m.FromCurrency.Mid / m.ToCurrency.Mid)).EffectiveDate;

            return maxExchangeRatioDate;
        }
    }
}

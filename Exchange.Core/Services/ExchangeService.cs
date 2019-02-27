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

        public async Task<decimal> ExchangeAsync(Currency from, Currency to, decimal amount)
        {
            if (amount <= decimal.Zero)
                return 0.0m;

            if (from.Iso4217CurrencyCode == to.Iso4217CurrencyCode)
                return amount;

            var rate = await CurrencyRateAsync(from, to);

            return amount * rate;
        }


        public async Task<decimal> GetMidFromCurrencyAsync(Currency currency)
        {
            var result = await _nbpApiService.GetExchangeRateAsync(currency.Iso4217CurrencyCode);
            return result.Mid;
        }

        private async Task<decimal> CurrencyRateAsync(Currency from, Currency to)
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

        public async Task<decimal> ExchangeAsync(string fromIso4217CurrencyCode, string toIso4217CurrencyCode, decimal amount)
        {
            var from = await GetCurrencyFromCodeAsync(fromIso4217CurrencyCode);
            var to = await GetCurrencyFromCodeAsync(toIso4217CurrencyCode);

            return await ExchangeAsync(from, to, amount);
        }
    }
}

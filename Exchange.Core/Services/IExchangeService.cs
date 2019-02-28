using Exchange.Core.Models;
using System;
using System.Threading.Tasks;

namespace Exchange.Core.Services
{
    public interface IExchangeService
    {
        Task<decimal> ExchangeAsync(Currency from, Currency to, decimal amount);
        Task<decimal> ExchangeAsync(string fromIso4217CurrencyCode, string toIso4217CurrencyCode, decimal amount);
        Task<decimal> GetMidFromCurrencyAsync(Currency currency);
        Task<Currency> GetCurrencyFromCodeAsync(string iso4217CurrencyCode);
        Task<DateTime> GetDateOfAdvantageousExchangeAsync(Currency from, Currency to, int inLastDay = 30);
        Task<DateTime> GetDateOfAdvantageousExchangeAsync(string fromIso4217CurrencyCode, string toIso4217CurrencyCode, int inLastDay = 30);
    }
}

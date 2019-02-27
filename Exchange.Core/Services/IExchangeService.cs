using Exchange.Core.Models;
using System.Threading.Tasks;

namespace Exchange.Core.Services
{
    public interface IExchangeService
    {
        Task<decimal> ExchangeAsync(Currency from, Currency to, decimal amount);
        Task<decimal> ExchangeAsync(string fromIso4217CurrencyCode, string toIso4217CurrencyCode, decimal amount);
        Task<decimal> GetMidFromCurrencyAsync(Currency currency);
        Task<Currency> GetCurrencyFromCodeAsync(string iso4217CurrencyCode);
    }
}

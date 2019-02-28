using Exchange.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exchange.Core.Services
{
    public interface INbpApiService
    {
        Task<Currency> GetExchangeRateAsync(string iso4217Code);
        Task<IEnumerable<Currency>> GetAllCurrenciesAsync();
        Task<IEnumerable<Currency>> GetCurrencyHistoryAsync(Currency currency, int lastDays);
    }
}

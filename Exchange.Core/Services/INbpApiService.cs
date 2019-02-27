using Exchange.Core.DTOs;
using Exchange.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exchange.Core.Services
{
    public interface INbpApiService
    {
        Task<RateDto> GetExchangeRateAsync(string iso4217Code);
        Task<IEnumerable<Currency>> GetAllCurrenciesAsync();
    }
}

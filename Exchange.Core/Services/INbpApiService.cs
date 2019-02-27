using System.Threading.Tasks;
using Exchange.Core.DTO;

namespace Exchange.Core.Services
{
    public interface INbpApiService
    {
        Task<RateDTO> GetExchangeRate(string iso4217code);
    }
}
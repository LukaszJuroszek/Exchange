using Exchange.Core.DTO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

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

        public async Task<RateDTO> GetExchangeRate(string iso4217code)
        {
            var client = _httpClientFactory.CreateClient();
            var result = await client.GetAsync($@"{_baseApiUri}/exchangerates/rates/a/{iso4217code.ToLower()}/");

            //handle error
            result.EnsureSuccessStatusCode();

            var rates = await result.Content.ReadAsAsync<RatesDTO>();

            return rates.Rates.FirstOrDefault();
        }
    }
}

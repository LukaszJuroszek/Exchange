using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Exchange.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Exchange.ViewModels;
using System.Xml.Serialization;
using Exchange.Core.DTO;
using System.Collections.Generic;
using System.Linq;

namespace Exchange.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _baseApiUri = "http://api.nbp.pl/api";
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var result = await client.GetAsync($@"{_baseApiUri}/exchangerates/rates/a/chf/");
            result.EnsureSuccessStatusCode();
            var rate = await result.Content.ReadAsAsync<RatesDTO>();
            var test = await result.Content.ReadAsStringAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using Exchange.Core.Services;
using Exchange.Models;
using Exchange.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Exchange.Controllers
{
    public class HomeController : Controller
    {

        private readonly IExchangeService _exchangeService;
        private readonly INbpApiService _nbpApiService;

        public HomeController(IExchangeService exchangeService, INbpApiService nbpApiService)
        {
            _exchangeService = exchangeService;
            _nbpApiService = nbpApiService;
        }

        public async Task<IActionResult> Index()
        {
            var currentCurrencies = await _nbpApiService.GetAllCurrenciesAsync();
            var vm = new IndexViewModel
            {
                //Add PLN rate (not in Table A)
                Currencies = currentCurrencies,
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(IndexViewModel vm)
        {
            if (string.IsNullOrEmpty(vm.FromCurrency) || string.IsNullOrEmpty(vm.ToCurrency))
                return View(vm);

            var currentCurrencies = await _nbpApiService.GetAllCurrenciesAsync();
            var exchange = await _exchangeService.ExchangeAsync(vm.FromCurrency, vm.ToCurrency, vm.Amount);
            var dateOfAdvantageousExchange = await _exchangeService.GetDateOfAdvantageousExchangeAsync(vm.FromCurrency, vm.ToCurrency);

            vm.ExchangeValue = exchange;

            if (vm.FromCurrency != vm.ToCurrency)
                vm.DateOfAdvantageousExchange = dateOfAdvantageousExchange;

            vm.Currencies = currentCurrencies;

            return View(vm);
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

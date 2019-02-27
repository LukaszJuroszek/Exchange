using Exchange.Core.Models;
using System.Collections.Generic;

namespace Exchange.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Currency> Currencies { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Amount { get; set; }
        public decimal ExchangeValue { get; set; }
    }
}

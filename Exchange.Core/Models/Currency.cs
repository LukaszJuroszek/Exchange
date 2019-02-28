using System;

namespace Exchange.Core.Models
{
    public class Currency
    {
        public string Iso4217CurrencyCode { get; set; }
        public string Name { get; set; }
        public decimal Mid { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}

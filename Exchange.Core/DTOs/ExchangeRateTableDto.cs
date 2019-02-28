using System;
using System.Collections.Generic;

namespace Exchange.Core.DTOs
{
    public class ExchangeRatesTableDto
    {
        public string No { get; set; }
        public DateTime EffectiveDate { get; set; }
        public List<RateDto> Rates { get; set; }
        public string Currency { get; set; }
        public string Code { get; set; }
    }
}

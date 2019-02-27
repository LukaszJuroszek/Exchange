using System;

namespace Exchange.Core.DTOs
{
    public class RateDto
    {
        public string No { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public decimal Mid { get; set; }
        public string Code { get; set; }
        public string Currency { get; set; }
    }
}

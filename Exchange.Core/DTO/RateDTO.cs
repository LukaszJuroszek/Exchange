using System;

namespace Exchange.Core.DTO
{
    public class RateDTO
    {
        public string No { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public decimal Mid { get; set; }
    }
}

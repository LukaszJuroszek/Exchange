using System.Collections.Generic;

namespace Exchange.Core.DTOs
{
    public class RatesDto
    {
        public string Code { get; set; }
        public string Currency { get; set; }
        public List<RateDto> Rates { get; set; }
    }
}

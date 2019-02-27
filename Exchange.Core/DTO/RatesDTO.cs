using System;
using System.Collections.Generic;
using System.Text;

namespace Exchange.Core.DTO
{
    public class RatesDTO
    {
        public string Code { get; set; }
        public string Currency { get; set; }
        public List<RateDTO> Rates { get; set; }
    }
}

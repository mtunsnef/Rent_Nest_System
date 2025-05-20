using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Core.DTO
{
    public class PricingLookupDto
    {
        public int TimeUnitId { get; set; }
        public int PackageTypeId { get; set; }
        public int DurationValue { get; set; }
    }
}

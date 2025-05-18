using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Core.DTO
{
    public class DurationPriceDto
    {
        public int DurationValue { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string TimeUnitName { get; set; }
    }

}

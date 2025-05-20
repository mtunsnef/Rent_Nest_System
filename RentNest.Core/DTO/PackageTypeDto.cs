using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Core.DTO
{
    public class PackageTypeDto
    {
        public int PackageTypeId { get; set; }
        public string PackageTypeName { get; set; }
        public string Description { get; set; }
        public decimal? UnitPrice { get; set; }
        public string TimeUnitName { get; set; }
    }
}

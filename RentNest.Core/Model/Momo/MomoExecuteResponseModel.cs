using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentNest.Core.Model.Momo
{
    public class MomoExecuteResponseModel
    {
        public string OrderId { get; set; }
        public double Amount { get; set; }
        public string FullName { get; set; }
        public string OrderInfo { get; set; }

    }
}
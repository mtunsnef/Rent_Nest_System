using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Core.UtilHelper
{
    public static class BadgeHelper
    {
        public static string GetBadgeClass(string packageTypeName)
        {
            return packageTypeName switch
            {
                "VIP Kim Cương" => "bg-danger",
                "VIP Vàng" => "bg-warning",
                "VIP Bạc" => "bg-info",
                _ => ""
            };
        }
    }
}

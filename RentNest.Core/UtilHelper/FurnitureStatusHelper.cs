using RentNest.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Core.UtilHelper
{
    public static class FurnitureStatusHelper
    {
        public static string GetDisplayName(FurnitureStatusEnum status) => status switch
        {
            FurnitureStatusEnum.None => "Không có nội thất",
            FurnitureStatusEnum.Basic => "Nội thất cơ bản",
            FurnitureStatusEnum.Full => "Nội thất đầy đủ",
            _ => "Không xác định"
        };

        public static FurnitureStatusEnum Parse(string? value) => value switch
        {
            "Không có nội thất" => FurnitureStatusEnum.None,
            "Nội thất cơ bản" => FurnitureStatusEnum.Basic,
            "Nội thất đầy đủ" => FurnitureStatusEnum.Full,
            _ => FurnitureStatusEnum.None
        };
    }
}

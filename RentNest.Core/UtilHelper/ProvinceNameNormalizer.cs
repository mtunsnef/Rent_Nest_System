using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Core.UtilHelper
{
    public static class ProvinceNameNormalizer
    {
        private static readonly Dictionary<string, string> Mappings = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Trung ương Đà Nẵng"] = "Thành phố Đà Nẵng",
            ["Trung ương Hồ Chí Minh"] = "Thành phố Hồ Chí Minh",
            ["Trung ương Cần Thơ"] = "Thành phố Cần Thơ",
            ["Trung ương Hải Phòng"] = "Thành phố Hải Phòng",
        };

        public static string Normalize(string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            var lower = input.Trim().ToLowerInvariant();
            return Mappings.TryGetValue(lower, out var normalized)
                ? normalized
                : input;
        }
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Core.Enums
{
    public enum PackageTypeEnum
    {
        [Display(Name = "Tin thường")]
        Normal = 0,

        [Display(Name = "VIP Bạc")]
        Silver = 1,

        [Display(Name = "VIP Vàng")]
        Gold = 2,

        [Display(Name = "VIP Kim Cương")]
        Diamond = 3
    }
}

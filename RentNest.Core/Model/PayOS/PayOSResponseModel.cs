using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentNest.Core.Model.PayOS
{
    public class PayOSResponseModel
    {
        public int Amount { get; set; }
        public bool IsSuccess { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Thông tin cố định
        public string ItemName => "Gói đăng ký dịch vụ";
        public string PaymentMethod => "PayOS";
        public string Status => IsSuccess ? "Thành công" : "Đã hủy";
    }
}
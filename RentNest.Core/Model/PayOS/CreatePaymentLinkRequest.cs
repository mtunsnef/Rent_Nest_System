using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentNest.Core.Model.PayOS
{
    public record CreatePaymentLinkRequest(
    string productName,
    string description,
    int price,
    string returnUrl,
    string cancelUrl
);
}
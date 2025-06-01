using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentNest.Core.Model.PayOS
{
    public record ConfirmWebhook(
    string webhook_url
);
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RentNest.Core.Model.VNPay;

namespace RentNest.Web.Service.Interface
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
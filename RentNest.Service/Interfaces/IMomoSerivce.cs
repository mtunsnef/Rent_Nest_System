using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RentNest.Core.Model.Momo;

namespace RentNest.Service.Interfaces
{
    public interface IMomoSerivce
    {
        Task<MomoCreatePaymentResponseModel> CrearePaymentAsync(OrderInfoModel model);
        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);

    }
}
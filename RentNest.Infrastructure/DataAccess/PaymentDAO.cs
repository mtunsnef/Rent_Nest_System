using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.DataAccess
{
    public class PaymentDAO : BaseDAO<Payment>
    {
        public PaymentDAO(RentNestSystemContext context) : base(context) { }
    }
}

using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.DataAccess
{
    public class AccommodationImageDAO : BaseDAO<AccommodationImage>
    {
        public AccommodationImageDAO(RentNestSystemContext context) : base(context) { }
    }
}

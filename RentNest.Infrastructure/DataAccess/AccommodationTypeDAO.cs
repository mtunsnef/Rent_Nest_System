using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.DataAccess
{
    public class AccommodationTypeDAO : BaseDAO<AccommodationType>
    {
        public AccommodationTypeDAO(RentNestSystemContext context) : base(context) { }
    }
}
